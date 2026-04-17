using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    [SerializeField] private Camera mapCamera;
    [SerializeField] private Vector3 initialPos;
    [SerializeField] private Transform player;
    [SerializeField] private float ySolidPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
        MoveMapCamera(); 
    }

    private void MoveMapCamera()
    {
        Vector3 newPos = mapCamera.transform.position;
        //playerのx,z座標に合わせる
        newPos.x = player.position.x; 
        newPos.y = ySolidPos; //y座標は固定にするか可変にするかは要検討
        newPos.z = player.position.z;
        mapCamera.transform.position = newPos;
    }
}
