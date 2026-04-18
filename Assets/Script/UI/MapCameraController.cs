using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    [SerializeField] private Camera mapCamera;
    [SerializeField] private Vector3 initialPos;
    [SerializeField] private Transform player;
    [SerializeField] private float ySolidPos;
    private Transform cameraTransform;

    private void Awake()
    {
        if (mapCamera != null)
        {
            // Transformへのアクセスは毎フレーム呼ぶとコストがかかるためキャッシュする
            cameraTransform = mapCamera.transform;
        }
    }

    // プレイヤーの移動(Update)が完了した後にカメラを動かすことで画面のカクつきを防ぐ
    private void LateUpdate()
    {
        if (cameraTransform == null || player == null) return;

        // プロパティ(C++側要素)へのアクセス回数を1回に抑えることで計算コストを削減
        Vector3 playerPos = player.position;

        // new Vector3で直接代入
        cameraTransform.position = new Vector3(playerPos.x, ySolidPos, playerPos.z);
    }
}
