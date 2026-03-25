namespace Syacapachi.ScriptableObject
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName = "PhotoTargetData")]
    [Serializable]
    public class PhotoTargetDataSO : ScriptableObject
    {
        [SerializeField] int score;
        [SerializeField] Color drawColor = Color.white;
        public int Score => score;
        public Color DrawColor => drawColor;
    }
}
