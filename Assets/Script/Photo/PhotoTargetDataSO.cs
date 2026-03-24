namespace Syacapachi.ScriptableObject
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName = "PhotoTargetData")]
    [Serializable]
    public class PhotoTargetDataSO : ScriptableObject
    {
        [SerializeField] int score;
        public int Score => score;
    }
}
