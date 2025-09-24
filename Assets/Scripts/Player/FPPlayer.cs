using System;
using Gameplay;
using Machines;
using UI.TabletUI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using Database;
using UnityEngine.InputSystem;

namespace Core
{
    public class FPPlayer : Player
    {        
        [Header("Refs")]
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private TabletUI _tabletUI;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _camera;

        [Header("Movement Settings")]
        [SerializeField] private float _speed = 3.0f;

        [Header("Look Settings")]
        [SerializeField] private float _mouseSensitivity = 3.0f;
        [SerializeField] private bool _invertY = false;
        [SerializeField] private bool _invertX = true;
        [SerializeField] private float _lookClamp = 80.0f;

        public bool IsMovingForward = false;

        private Vector2 currentMoveInput;
        private Vector2 currentLookInput;
        private float cameraPitch = 0.0f; 
        private float cameraYaw = 0.0f;

        private InputAction moveAction;
        private InputAction lookAction;

        private bool isMoving = true;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            moveAction = playerInput.actions["Move"];
            lookAction = playerInput.actions["Look"];

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _canvas.enabled = !isMoving;
            Cursor.lockState = isMoving ? CursorLockMode.Locked : CursorLockMode.Confined;
            Cursor.visible = !isMoving;
        }

        private void Update()
        {
            if (isMoving)
            {
                HandleMovement();
                HandleMouseLook();
            }            
        }

        public override void ReleaseAllGrabbables()
        { 
            
        }

        public override void ReleaseHandle()
        {
            
        }

        public override void Init()
        {
            _tabletUI.Init();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            currentMoveInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            currentLookInput = context.ReadValue<Vector2>();
        }

        public void OnShowUI(InputAction.CallbackContext context)
        {
            isMoving = !isMoving;
            _canvas.enabled = !isMoving;
            Cursor.lockState = isMoving ? CursorLockMode.Locked : CursorLockMode.Confined;
            Cursor.visible = !isMoving;
        }

        public void OnClick(InputAction.CallbackContext context)
        {

        }

        private void HandleMovement()
        {
            if (currentMoveInput.sqrMagnitude >= 0.01f)
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);

                Vector3 movement = (forward * currentMoveInput.y + right * currentMoveInput.x) * _speed;
                characterController.Move(movement * Time.deltaTime);
            }                
        }

        private void HandleMouseLook()
        {
            if (currentLookInput.sqrMagnitude >= 0.01f)
            {
                float mouseX = currentLookInput.x * _mouseSensitivity * Time.deltaTime;
                float mouseY = currentLookInput.y * _mouseSensitivity * Time.deltaTime;

                transform.Rotate(Vector3.up * mouseX);

                cameraPitch += _invertY ? mouseY : -mouseY;
                cameraYaw += _invertX ? mouseX : -mouseX;

                cameraPitch = Mathf.Clamp(cameraPitch, -_lookClamp, _lookClamp);
                _camera.transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
                transform.rotation = Quaternion.Euler(0f, cameraYaw, 0f);

            }
        }
    }
}