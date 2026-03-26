namespace Syacapachi.Controller
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerInputReciever : MonoBehaviour
    {
        [SerializeField] Camera m_camera;
        InputAction moveAction;
        InputAction lookAction;
        InputAction boostAction;

        Vector2 moveInput = Vector2.zero;
        Vector2 lookInput = Vector2.zero;
        private bool canBoost = true;
        [Tooltip("1につき0.1秒待つ")]
        [SerializeField] int waitCount = 20;
        private static WaitForSeconds wait01s = new WaitForSeconds(0.1f);
        private int proggressCount = 0;


        public Camera Camera => m_camera;
        public Vector2 MoveInput => moveInput;
        public Vector2 LookInput => lookInput;

        public event Action BoostAction;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            moveAction = InputSystem.actions["Move"];
            lookAction = InputSystem.actions["Look"];
            boostAction = InputSystem.actions["Accelarate"];
        }
        private void OnEnable()
        {
            moveAction.performed += MoveHandleCallback;
            moveAction.canceled += MoveHandleCallback;
            lookAction.performed += LookHandleCallback;
            lookAction.canceled += LookHandleCallback;
            boostAction.performed += boostHandleCallback;
        }
        private void OnDisable()
        {
            moveAction.performed -= MoveHandleCallback;
            moveAction.canceled -= MoveHandleCallback;
            lookAction.performed -= LookHandleCallback;
            lookAction.canceled -= LookHandleCallback;
            boostAction.performed -= boostHandleCallback;
        }
        private void MoveHandleCallback(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        private void LookHandleCallback(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
        }
        private void boostHandleCallback(InputAction.CallbackContext context) 
        {
            if (!canBoost) return;
            BoostAction.Invoke();
            StartCoroutine(BoostCorutine());
        }
        private IEnumerator BoostCorutine()
        {
            canBoost = false;
            for(proggressCount = 0; proggressCount < waitCount; proggressCount++)
            {
                yield return wait01s;
            }
            canBoost = true;
        }
        private void OnGUI()
        {
            if (canBoost) return;

            GUI.Label(new Rect(10, 10, 100, 30), $"{proggressCount}");
        }
    }
}