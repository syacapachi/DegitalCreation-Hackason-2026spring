namespace Syacapachi.Controller
{
    using Syacapachi.Attribute;
    using System;
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
            BoostAction.Invoke();
        }
    }
}