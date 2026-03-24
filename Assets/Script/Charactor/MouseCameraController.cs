namespace Syacapachi.Controller
{
    using UnityEngine;
    [RequireComponent(typeof(PlayerInputReciever))]
    public class MouseCameraController : MonoBehaviour
    {
        [SerializeField] PlayerInputReciever reciever;
        [Header("Camra Setting")]
        [SerializeField] float mouseSensitivity = 1f;
        [SerializeField] float yMinLimit = -89f;
        [SerializeField] float yMaxLimit = 89f;
        Vector2 cameraAngle;

        private void OnEnable()
        {
            Vector3 angle = reciever.Camera.transform.eulerAngles;
            //xとyを入れ替える。これにより、カメラの回転がプレイヤーの入力に対して正しく反応するようになる。
            cameraAngle.x = angle.y;
            cameraAngle.y = angle.x;
        }
        private void LateUpdate()
        {
            if (reciever .LookInput == Vector2.zero) return;
            cameraAngle.x += reciever .LookInput.x * mouseSensitivity;
            cameraAngle.y = ClampAngle(cameraAngle.y - reciever.LookInput.y * mouseSensitivity, yMinLimit, yMaxLimit);

            RotationChange(cameraAngle.x, cameraAngle.y);

        }
        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }
        private void RotationChange(float x, float y)
        {
            reciever.Camera.transform.rotation = Quaternion.Euler(y, x, 0);
        }
        private void Reset()
        {
            reciever = GetComponent<PlayerInputReciever>(); 
        }
    }
}