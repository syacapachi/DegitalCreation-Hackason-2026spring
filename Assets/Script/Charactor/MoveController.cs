using Syacapachi.Attribute;
using Syacapachi.Controller;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] PlayerInputReciever reciever;
    [Header("MoveSetting")]
    [SerializeField] Transform playerRootTransform;
    [SerializeField] Rigidbody rb;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] private bool isFlying = true;
    [Header("FlySetting")]
    [SerializeField,EnableIf(nameof(isFlying))]
    float gravity;

    [SerializeField, EnableIf(nameof(isFlying))]
    float yCorrection = 0.05f;
    [Tooltip("旋回時慣性がどれくらい残るか")]
    [SerializeField,EnableIf(nameof(isFlying))]
    float steerPower = 5f;

    [Tooltip("揚力")]
    [SerializeField,EnableIf(nameof(isFlying))]
    float liftPower = 2f;

    [Tooltip("空気抵抗")]
    [SerializeField, EnableIf(nameof(isFlying)), Range(0f, 1f)]
    float drag = 0.5f;

    [SerializeField,EnableIf(nameof(isFlying))]
    float maxSpeed = 10f;

    [Tooltip("失速開始速度")]
    [SerializeField, EnableIf(nameof(isFlying))]
    float stallSpeed = 5f;

    [Tooltip("ボタン加速")]
    [SerializeField, EnableIf(nameof(isFlying))]
    float force = 5f;

    //Vector3 velocity = Vector3.zero;
    //Vector3 correction = new Vector3(1, ,1);

    public bool IsFlying
    {
        get => isFlying;
        private set
        {
            if (isFlying == value) return;
            isFlying = value;
            if(!isFlying)
            {
                rb.linearVelocity = Vector3.zero;
            }
        }
    }
    private void OnEnable()
    {
        reciever.BoostAction += BoostActionHandle;
    }
    private void OnDisable()
    {
        reciever.BoostAction -= BoostActionHandle;
    }
    private void BoostActionHandle()
    {
        Vector3 foward = reciever.Camera.transform.forward;
        rb.AddForce(force * foward, ForceMode.Acceleration);
    }

    

    private void Update()
    {
        if (!isFlying)
        {
            Vector2 moveInput = reciever.MoveInput;
            if (moveInput == Vector2.zero) return;
            Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y);
            Vector3 moveVec = reciever.Camera.transform.rotation * inputDirection * moveSpeed;
            moveVec.y = 0;
            //相対座標で移動させる。これにより、プレイヤーの向きに応じた移動が可能になる。
            playerRootTransform.Translate(moveVec * Time.deltaTime);
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isFlying)
        {
            Vector3 velocity = rb.linearVelocity;
            float speed = velocity.magnitude;

            //// ① 重力（常に下）
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

            // ② 揚力（速度の２乗に依存）
            if (speed > 0.1f)
            {
                float lift = speed * speed * liftPower;

                // 失速処理
                if (speed < stallSpeed)
                    lift *= 0.2f;

                rb.AddForce(transform.up * lift, ForceMode.Acceleration);
            }

            // ③ カメラ方向への誘導（操作）
            Vector3 targetDir = reciever.Camera.transform.forward;
            Vector3 steer = (targetDir - velocity.normalized) * steerPower;

            rb.AddForce(steer, ForceMode.Acceleration);

            // ④ 空気抵抗
            rb.AddForce(-velocity * drag, ForceMode.Acceleration);

            // ⑤ 速度制限
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }
        
    }
    [OnInspectorButton]
    private void SwitchState()
    {
        IsFlying = !IsFlying;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsFlying = false;
        }
    }
    private void Reset()
    {
        reciever = GetComponent<PlayerInputReciever>();
    }
}
