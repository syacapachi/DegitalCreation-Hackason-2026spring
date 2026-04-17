namespace Syacapachi.Controller
{
    using Syacapachi.ScriptableObject;
    using UnityEngine;
    
    public class PhotoTargetController : MonoBehaviour
    {
        [SerializeField] PhotoTargetDataSO DataSO;
        public PhotoTargetType TargetType => DataSO != null ? DataSO.TargetType : PhotoTargetType.Normal;
        public int Score => DataSO.Score;
        public Color Color => DataSO.DrawColor;
    }
}
