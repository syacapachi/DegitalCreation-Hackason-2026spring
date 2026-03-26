using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPanelPresenter : MonoBehaviour
{
    [Header("Models")]
    [SerializeField] private ResultPanelModel resultPanelModel;
    
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
    }

    void Start()
    {
        // ゲーム終了時にホーム画面に戻るボタンを押した際の処理
        resultPanelView.OnToHomeButtonClick += () => SceneManager.LoadScene("Home");
    }
}
