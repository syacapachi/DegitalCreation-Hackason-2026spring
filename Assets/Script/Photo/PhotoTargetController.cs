namespace Syacapachi.Controller
{
    using Syacapachi.ScriptableObject;
    using UnityEngine;
    
    public class PhotoTargetController : MonoBehaviour
    {
        [SerializeField] PhotoTargetDataSO DataSO;
        public int Score => DataSO.Score;
        public Color Color => DataSO.DrawColor;
    }
}
