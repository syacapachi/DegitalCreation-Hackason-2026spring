using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPanelPresenter : MonoBehaviour
{
    [Header("Models")]
    [SerializeField] private ResultPanelModel resultPanelModel;
    
    [Header("Ranking")]
    [SerializeField] private RankingPresenter rankingPresenter; // UIがあれば更新用
    [SerializeField] private RankingManager rankingManager; // データ保存用
    
    [Header("UI Views")]
    [SerializeField] public ResultPanelView resultPanelView;

    private void OnEnable()
    {
        ShowResults();
    }

    public void ShowResults()
    {
        if (resultPanelView == null || resultPanelModel == null) return;

        resultPanelView.RefreshPicture();

        var photos = resultPanelModel.GetPhotoData();
        foreach (var photo in photos)
        {
            resultPanelView.AddPhotoData(photo);
        }

        resultPanelView.SetScoreText(resultPanelModel.ScoreText);

        // --- ランキングへのスコア自動登録処理 ---
        // RankingManagerを探して直接データを保存させる（UIが非アクティブでも確実に保存）
        if (rankingManager == null)
        {
            rankingManager = FindAnyObjectByType<RankingManager>();
        }

        if (rankingManager != null)
        {
            rankingManager.AddRankingData(resultPanelModel.TotalScore, "Player");
        }
        else
        {
            Debug.LogWarning("RankingManagerが見つからないためスコアが保存されませんでした。");
        }

        // もしランキングUI（Presenter）がアクティブなら表示を更新
        if (rankingPresenter == null)
        {
            rankingPresenter = FindAnyObjectByType<RankingPresenter>();
        }
        
        if (rankingPresenter != null)
        {
            rankingPresenter.RefreshRanking();
        }
    }

    void Start()
    {
        // ゲーム終了時にホーム画面に戻るボタンを押した際の処理
        resultPanelView.OnToHomeButtonClick += () => SceneManager.LoadScene("Home");
    }
}
