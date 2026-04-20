using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class RankingManager : MonoBehaviour
{
    private RankingArray rankingDataCache;

    // 保存先パス (Application.persistentDataPathを使用して各環境で正常に保存されるようにする)
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, "rankingData.json");

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadRanking();
    }

    // 外部（Presenter等）からデータを追加するためのメソッド
    public void AddRankingData(float score, string nickName)
    {
        if (rankingDataCache == null) rankingDataCache = new RankingArray();

        RankingData newData = new RankingData
        {
            score = score,
            nickName = nickName,
            timeStamp = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") // タイムスタンプを保存
        };

        rankingDataCache.rankArray.Add(newData);
        
        // MVPにおけるModelの役割：データの整理（スコア降順へのソート）
        rankingDataCache.rankArray = rankingDataCache.rankArray.OrderByDescending(x => x.score).ToList();
        
        SaveRanking();
    }

    // 最新のランキングデータを返す
    public List<RankingData> GetRankingData()
    {
        if (rankingDataCache == null) LoadRanking();
        return rankingDataCache.rankArray;
    }

    // JSONへの保存
    private void SaveRanking()
    {
        if (rankingDataCache == null) return;
        string json = JsonUtility.ToJson(rankingDataCache, true);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log("Ranking saved to: " + SaveFilePath);
    }

    // JSONからの読み込み
    private void LoadRanking()
    {
        if (File.Exists(SaveFilePath))
        {
            try
            {
                string json = File.ReadAllText(SaveFilePath);
                rankingDataCache = JsonUtility.FromJson<RankingArray>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Ranking Load Failed: " + e.Message);
                rankingDataCache = new RankingArray();
            }
        }
        else
        {
            rankingDataCache = new RankingArray();
        }
    }
}
