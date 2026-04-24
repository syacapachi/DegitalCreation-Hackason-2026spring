using Syacapachi.Attribute;
using Syacapachi.Controller;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] PlayerInputReciever reciever;
    [SerializeField] private Camera mainCamera;
    [Header("MoveSetting")]
    [SerializeField] Transform playerRootTransform;
    [SerializeField] Rigidbody rb;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] private bool isFlying = true;
    [Header("FlySetting")]
    [SerializeField,EnableIf(nameof(isFlying))]
    float gravity = 9.81f;

    [Tooltip("旋回時慣性がどれくらい残るか")]
    [SerializeField, EnableIf(nameof(isFlying)), Range(0f, 1f)]
    float steerRate = 0.5f;

    [Tooltip("揚力の割合")]
    [SerializeField ,EnableIf(nameof(isFlying)), Range(0f, 1f)]
    float liftRate = 0.2f;

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
    private static Vector3 correction = new Vector3(1,0 ,1);

    // プレイヤーの初期位置
    private Vector3 playerInitialPos;

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
        reciever.RespawnAction += RespawnActionHandle;
    }
    private void OnDisable()
    {
        reciever.BoostAction -= BoostActionHandle;
        reciever.RespawnAction -= RespawnActionHandle;
    }
    private void BoostActionHandle()
    {
        //Vector3 foward = reciever.Camera.transform.forward;
        //rb.AddForce(force * foward, ForceMode.Acceleration);

        //if (rb.linearVelocity.magnitude > maxSpeed)
        //{
        //    rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        //}
        rb.linearVelocity = reciever.Camera.transform.up * maxSpeed;
    }

    // リスポーン処理を定義
    private void RespawnActionHandle()
    {
        // 物理エンジンの影響で元に戻らないように、Rigidbody側も直接移動させ、速度をリセットする
        rb.position = playerInitialPos;
        playerRootTransform.position = playerInitialPos;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        mainCamera.transform.rotation = new Quaternion(0,0,0,0); //カメラの方向を正面に調整する
    }

    private void Start()
    {
        //プレイヤーの初期位置を取得
        playerInitialPos = playerRootTransform.position;
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
            //float speed = Vector3.Scale(velocity, correction).magnitude;
            float speed = velocity.magnitude;
            float speedMul = speed * speed;
            Vector3 moveDir = velocity.normalized;
            Vector3 targetDir = reciever.Camera.transform.forward;

            //// ① 重力（常に下）
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
            

            // ② 揚力（速度の２乗に依存）
            if (speed > 0.1f)
            {
                float lift = 0.5f * speedMul * liftRate;

                // 失速処理
                if (speed < stallSpeed)
                    lift *= 0.2f;

                //ピッチで揚力変化（リアル化）
                float pitch = Vector3.Dot(targetDir, Vector3.up);
                //lift *= (1 - pitch);
                if(pitch >= 0)
                    rb.AddForce(playerRootTransform.up * lift, ForceMode.Acceleration);
            }
            
            // ③ カメラ方向への誘導（操作）
           
            Vector3 steer = 0.5f * speedMul * steerRate * (targetDir - moveDir);

            rb.AddForce(steer, ForceMode.Acceleration);

            // ④ 空気抵抗
            rb.AddForce(-0.5f * drag * speedMul * moveDir, ForceMode.Acceleration);


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
