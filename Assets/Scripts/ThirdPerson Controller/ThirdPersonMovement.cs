using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPersonController
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonMovement : MonoBehaviour
    {
        public event Action<float> OnSpeedChanged;
        public event Action OnJumped;
        public event Action<bool> OnGroundedChanged;

        [Header("References")]
        [SerializeField] private Transform mainCamera;
        [SerializeField] private TargetLock targetLock;
        
        [Header("Input Actions")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference jumpAction;

        [Header("Movement Settings")]
        [SerializeField] private float speed = 6f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float turnSmoothTime = 0.1f;

        private CharacterController controller;
        private Vector3 velocity;
        private float turnSmoothVelocity;
        
        private float currentSpeed;
        private bool wasGrounded = true;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            if (moveAction != null) moveAction.action.Enable();
            if (jumpAction != null) jumpAction.action.Enable();
        }

        private void OnDisable()
        {
            if (moveAction != null) moveAction.action.Disable();
            if (jumpAction != null) jumpAction.action.Disable();
        }

        private void Update()
        {
            Vector3 horizontalMove = HandleMovement();
            HandleGravityAndJump(horizontalMove);
        }

        private Vector3 HandleMovement()
        {
            Vector2 input = moveAction.action.ReadValue<Vector2>();
            Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;

            float inputMagnitude = input.magnitude;
            if (Mathf.Abs(currentSpeed - inputMagnitude) > 0.01f)
            {
                currentSpeed = inputMagnitude;
                OnSpeedChanged?.Invoke(currentSpeed);
            }

            if (direction.magnitude >= 0.1f)
            {
                // Calculate target angle based on camera's rotation
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
                
                // Handle Rotation
                if (targetLock != null && targetLock.HasTarget)
                {
                    // If locked on, always face the target
                    Vector3 directionToTarget = (targetLock.CurrentTarget.position - transform.position).normalized;
                    directionToTarget.y = 0f;
                    transform.forward = directionToTarget;
                }
                else
                {
                    // If not locked on, smoothly rotate towards movement direction
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }

                // Return horizontal movement vector
                return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * speed;
            }
            else if (targetLock != null && targetLock.HasTarget)
            {
                // Even if not moving, face the target when locked on
                Vector3 directionToTarget = (targetLock.CurrentTarget.position - transform.position).normalized;
                directionToTarget.y = 0f;
                transform.forward = directionToTarget;
            }

            return Vector3.zero;
        }

        private void HandleGravityAndJump(Vector3 horizontalMove)
        {
            bool isGrounded = controller.isGrounded;

            if (isGrounded != wasGrounded)
            {
                wasGrounded = isGrounded;
                OnGroundedChanged?.Invoke(isGrounded);
            }

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // Keep grounded safely
            }

            if (jumpAction.action.WasPressedThisFrame() && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                OnJumped?.Invoke();
            }

            velocity.y += gravity * Time.deltaTime;
            
            // Combine horizontal and vertical movement
            Vector3 finalMove = horizontalMove + velocity;
            controller.Move(finalMove * Time.deltaTime);
        }
    }
}
