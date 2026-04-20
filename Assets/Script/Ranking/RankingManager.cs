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
        // キャッシュが未初期化の場合は、ファイルからロードして既存データを保持する
        if (rankingDataCache == null) LoadRanking();

        RankingData newData = new RankingData
        {
            score = score,
            nickName = nickName,
            timeStamp = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") // タイムスタンプを保存
        };

        rankingDataCache.rankArray.Add(newData);
        
        // MVPにおけるModelの役割：データの整理（スコア降順へのソートと上位10件の保持）
        rankingDataCache.rankArray = rankingDataCache.rankArray
            .OrderByDescending(x => x.score)
            .Take(10)
            .ToList();
        
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
