using UnityEngine;

public class PhotoTargetController : MonoBehaviour
{
    [SerializeField] PhotoTargetDataSO DataSO;
    public int Score => DataSO.Score;
}
