namespace Syacapachi.Manager
{
    using Syacapachi.Camera;
    using Syacapachi.Utils;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PhotoScoreManager : MonoBehaviour
    {
        //仕様維持用の一時敵オーバーロード
        [SerializeField] ScoreCalcrator calcrator;
        private Dictionary<GameObject, float> photoMaxScoreDic = new();
        private bool isUpdated = false;
        [SerializeField] private float totalScore = 0;
        public float TotalScore
        {
            get
            {
                {
                    if (!isUpdated) return totalScore;
                    totalScore = 0;
                    foreach (var kvp in photoMaxScoreDic)
                    {
                        totalScore += kvp.Value;
                    }
                    return totalScore;
                }
            }
        }

        public void AddPhoto(GameObject obj,float socre)
        {
            if (photoMaxScoreDic.ContainsKey(obj))
            {
                if (photoMaxScoreDic[obj] > socre) return;
            }
            photoMaxScoreDic[obj] = socre;
            isUpdated = true;
        }
        public float GetScore(GameObject gameObject)
        {
            if(!photoMaxScoreDic.ContainsKey(gameObject)) return 0f;
            return photoMaxScoreDic[gameObject];
        }
        [Obsolete]
        //仕様維持用の一時敵オーバーロード
        public float GetScore(PhotoAnalyzer.PhotoObjectInfo info)
        {
            return calcrator.CalcrateScore(info);
        }
    }
}