namespace Syacapachi.ScriptableObject
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName = "PhotoTargetData")]
    [Serializable]
    public class PhotoTargetDataSO : ScriptableObject
    {
        [SerializeField] private PhotoTargetType targetType = PhotoTargetType.Normal;
        [SerializeField] int score;
        [SerializeField] Color drawColor = Color.white;
        public PhotoTargetType TargetType => targetType;
        public int Score => score;
        public Color DrawColor => drawColor;
    }
}
