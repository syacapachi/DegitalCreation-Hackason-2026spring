namespace Syacapachi.Manager
{
    using Syacapachi.Camera;
    using Syacapachi.Utils;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using static Syacapachi.Camera.CameraCapture;

    public class PhotoScoreManager : MonoBehaviour
    {
        //仕様維持用の一時敵オーバーロード
        [SerializeField] ScoreCalcrator calcrator;
        private Dictionary<GameObject, float> photoObjectMaxScoreDic = new();
        private bool isUpdated = false;
        [SerializeField] private float totalScore = 0;
        public float TotalScore
        {
            get
            {
                {
                    if (!isUpdated) return totalScore;
                    totalScore = 0;
                    foreach (var kvp in photoObjectMaxScoreDic)
                    {
                        totalScore += kvp.Value;
                    }
                    return totalScore;
                }
            }
        }
        private void Start()
        {
            totalScore = 0;
            photoObjectMaxScoreDic.Clear();
        }

        public void AddScore(GameObject obj,float socre)
        {
            if (photoObjectMaxScoreDic.ContainsKey(obj))
            {
                if (photoObjectMaxScoreDic[obj] > socre) return;
            }
            photoObjectMaxScoreDic[obj] = socre;
            isUpdated = true;
        }
        public float GetScore(GameObject gameObject)
        {
            if(!photoObjectMaxScoreDic.ContainsKey(gameObject)) return 0f;
            return photoObjectMaxScoreDic[gameObject];
        }
        [Obsolete]
        //仕様維持用の一時敵オーバーロード
        public float GetScore(PhotoAnalyzer.PhotoObjectInfo info)
        {
            return calcrator.CalcrateScore(info);
        }
    }
}