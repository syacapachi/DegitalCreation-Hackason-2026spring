namespace Syacapachi.Controller
{
    using Syacapachi.Attribute;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class KeyBoardController : MonoBehaviour
    {
        [Header("Move Setting")]
        [SerializeField] Transform playerRootTransform;
        [SerializeField] Rigidbody rb;
        [SerializeField] Vector3 moveSpeed = new Vector3(5f,0.2f,5f);
        [Header("Camra Setting")]
        [SerializeField] Camera m_camera;
        [SerializeField] float mouseSensitivity = 1f;
        [SerializeField] float yMinLimit = -89f;
        [SerializeField] float yMaxLimit = 89f;
        InputAction moveAction;
        InputAction lookAction;

        Vector2 cameraAngle;
        Vector2 moveInput = Vector2.zero;
        Vector2 lookInput = Vector2.zero;
        [SerializeField]private bool isFlying = true;
        public bool IsFlying
        { 
            get => isFlying;
            private set 
            {
                if(isFlying == value) return;
                isFlying = value;
                if (isFlying) 
                {
                    moveInput = Vector2.up;
                    rb.linearVelocity = new Vector3(0, -moveSpeed.y, 0);
                }
                else
                {
                    moveInput = Vector2.zero;
                    rb.linearVelocity = Vector3.zero;
                }
            }
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            moveAction = InputSystem.actions["Move"];
            lookAction = InputSystem.actions["Look"];
            moveInput = isFlying? Vector2.up : Vector2.zero;

        }
        private void OnEnable()
        {
            moveAction.performed += MoveHandleCallback;
            moveAction.canceled += MoveHandleCallback;
            lookAction.performed += LookHandleCallback;
            lookAction.canceled += LookHandleCallback;

            Vector3 angle = m_camera.transform.eulerAngles;
            //xとyを入れ替える。これにより、カメラの回転がプレイヤーの入力に対して正しく反応するようになる。
            cameraAngle.x = angle.y;
            cameraAngle.y = angle.x;
        }
        private void OnDisable()
        {
            moveAction.performed -= MoveHandleCallback;
            moveAction.canceled -= MoveHandleCallback;
            lookAction.performed -= LookHandleCallback;
            lookAction.canceled -= LookHandleCallback;
        }

        // Update is called once per frame
        private void Update()
        {
            if(moveInput == Vector2.zero) return;
            Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y) ;
            Vector3 moveVec = m_camera.transform.rotation * Vector3.Scale(inputDirection,moveSpeed);
            
            rb.linearVelocity = new Vector3(0, moveVec.y * moveSpeed.y, 0) ;
            moveVec.y = 0;
            //相対座標で移動させる。これにより、プレイヤーの向きに応じた移動が可能になる。
            playerRootTransform.Translate(moveVec * Time.deltaTime);
        }
        private void FixedUpdate()
        {
            
        }
        private void LateUpdate()
        {
            if(lookInput == Vector2.zero) return;
            cameraAngle.x += lookInput.x * mouseSensitivity;
            cameraAngle.y = ClampAngle(cameraAngle.y - lookInput.y * mouseSensitivity, yMinLimit, yMaxLimit);
            
            RotationChange(cameraAngle.x, cameraAngle.y);
            
        }
        private void MoveHandleCallback(InputAction.CallbackContext context)
        {
            if(isFlying) return;
            moveInput = context.ReadValue<Vector2>();
        }
        private void LookHandleCallback(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
        }
        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }
        private void RotationChange(float x, float y)
        {
            m_camera.transform.rotation = Quaternion.Euler(y, x, 0);
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
    }
}