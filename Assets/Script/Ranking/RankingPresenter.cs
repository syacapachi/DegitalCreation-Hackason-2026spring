using UnityEngine;
using System.Collections.Generic;

public class RankingPresenter : MonoBehaviour
{
    [SerializeField] private RankingView view;
    // Modelへの参照
    [SerializeField] private RankingManager rankingManager;

    void Start()
    {
        // インスペクタで未設定なら取得を試みる（安全策）
        if (rankingManager == null)
        {
            rankingManager = FindAnyObjectByType<RankingManager>();
        }

        view.OnCloseButtonClicked += OnCloseButtonHandler;
        
        // ランキングの表示更新
        RefreshRanking();
    }

    private void OnCloseButtonHandler()
    {
        view.rankPanel.SetActive(false);
    }

    // Viewへの表示指示を出す（Presenterの役割）
    public void RefreshRanking()
    {
        if (rankingManager == null) return;

        view.ClearRankingArea();

        // Modelからデータ取得
        List<RankingData> rankingList = rankingManager.GetRankingData();

        for (int i = 0; i < rankingList.Count; i++)
        {
            RankingData data = rankingList[i];
            
            // Textの設定用に文字列を組み立てる
            string rankStr = $"{i + 1}位";
            
            // タイムスタンプをニックネームと一緒に表示するかどうかはお好みで変更してください
            string nameStr = string.IsNullOrEmpty(data.nickName) ? "Unknown" : data.nickName;
            // nameStr += $" ({data.timeStamp})"; // ◀もしタイムスタンプを表示したい場合はこの行のコメントを外す
            
            string scoreStr = $"{data.score}";
            
            // データに基づいてViewに描画指示
            view.AddRankingLine(rankStr, nameStr, scoreStr);
        }
    }
    
    // 他のスクリプト（リザルト画面など）から新スコアを登録する時に呼ぶメソッド
    public void RegisterNewScore(float score, string nickname)
    {
        if (rankingManager != null)
        {
            rankingManager.AddRankingData(score, nickname);
            RefreshRanking();
        }
    }
}
