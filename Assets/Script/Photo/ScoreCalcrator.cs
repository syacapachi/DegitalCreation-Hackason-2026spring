namespace Syacapachi.Camera
{
    using Syacapachi.Contracts;
    using Syacapachi.Utils;
    using UnityEngine;
    public class ScoreCalcrator : MonoBehaviour, IScoreCalcrator
    {
        public float CalcrateScore(PhotoAnalyzer.PhotoObjectInfo info)
        {
            float centerScore = CalcrateCenterScore(info.MinX, info.MinY, info.MaxX,info.MaxY,info.centerPosition.x,info.centerPosition.y);

            return info.Score * centerScore * info.size;
        }
        /// <summary>
        /// 中心度0~1
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        public static float CalcrateCenterScore(
            float minX,
            float minY,
            float maxX,
            float maxY,
            float centerX,
            float centerY
            )
        {
            float half = 0.5f;
            //画像が中心を覆っているなら満点
            if (minX <= half
                && half <= maxX
                && minY <= half
                && half <= maxY
            )
                return 1f;

            //写真中央から、最寄りの長方形上の点

            float nearX = Mathf.Clamp(centerX, minX, maxX);
            float nearY = Mathf.Clamp(centerY, minY, maxY);

            float dist = Vector2.Distance(
                new Vector2(nearX, nearY),
                new Vector2(half, half)
                );

            return 1f - Mathf.Clamp01(dist);
        }
    }
}