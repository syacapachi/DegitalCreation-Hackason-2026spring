using System;
using System.Collections.Generic;

[Serializable]
public class RankingData
{
    public float score;  // スコア
    public string nickName; // ニックネーム
    public string timeStamp; // タイムスタンプ
}

[Serializable]
public class RankingArray
{
    public List<RankingData> rankArray = new List<RankingData>();
}
