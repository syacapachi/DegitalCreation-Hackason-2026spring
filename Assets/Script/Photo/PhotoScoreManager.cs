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
        [SerializeField] private float bonusScore = 0;
        [Header("Bonus Score")]
        [SerializeField] private float fireBonus = 1000;

        [Header("Event")]
        [SerializeField] private VoidEventSO OnFireEventCaptured;
        public float TotalScore
        {
            get
            {
                if (!isUpdated) return totalScore;

                float sum = 0;
                foreach (var kvp in photoObjectMaxScoreDic)
                {
                    sum += kvp.Value;
                }
                totalScore = sum + bonusScore;
                isUpdated = false;
                return totalScore;
            }
        }

        /*
         * この辺の実装が複雑になっているので要修正
         * とりあえず動作する
         */
        /// <summary>
        /// ボーナススコアの合計値を返すメソッドを想定(将来的にメソッドの構造を変えて活用する)
        /// </summary>
        public float BonusScore
        {
            get { return bonusScore; }
        }

        /// <summary>
        /// 一回当たりのボーナスポイントを返すプロパティ
        /// </summary>
        public float FireBonus
        {
            get { return fireBonus; }
        }

        private void Awake()
        {
            OnFireEventCaptured.Register(AddBonusScore);
        }
        private void Start()
        {
            totalScore = 0;
            bonusScore = 0;
            photoObjectMaxScoreDic.Clear();
            isUpdated = false;
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
        /// <summary>
        /// 火災イベントを撮影したときに基本点に加えてボーナスポイントを加算
        /// </summary>
        private void AddBonusScore()
        {
            bonusScore += fireBonus;
            isUpdated = true;
            Debug.Log($"ボーナスポイント({fireBonus})を加算しました。現在のボーナス合計: {bonusScore}");
        }

        [Obsolete]
        //仕様維持用の一時敵オーバーロード
        public float GetScore(PhotoAnalyzer.PhotoObjectInfo info)
        {
            return calcrator.CalcrateScore(info);
        }
    }
}