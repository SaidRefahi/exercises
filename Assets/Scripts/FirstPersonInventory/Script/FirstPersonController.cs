using UnityEngine;
using UnityEngine.InputSystem;

namespace FirstPersonInventory
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The transform that the Cinemachine Virtual Camera follows/looks at")]
        [SerializeField] private Transform lookTarget;
        
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Look")]
        [SerializeField] private float mouseSensitivity = 15f;
        [SerializeField] private float maxLookAngle = 85f;

        private CharacterController controller;
        private Vector3 velocity;
        private float verticalLookRotation;
        
        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleLook();
            HandleMovement();
        }

        private void HandleLook()
        {
            if (Mouse.current == null) return;

            Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * Time.deltaTime;

            verticalLookRotation -= mouseDelta.y;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -maxLookAngle, maxLookAngle);
            
            if (lookTarget != null)
            {
                lookTarget.localEulerAngles = new Vector3(verticalLookRotation, 0f, 0f);
            }

            transform.Rotate(Vector3.up * mouseDelta.x);
        }

        private void HandleMovement()
        {
            if (Keyboard.current == null) return;

            bool isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float moveX = 0f;
            float moveZ = 0f;

            if (Keyboard.current.wKey.isPressed) moveZ += 1f;
            if (Keyboard.current.sKey.isPressed) moveZ -= 1f;
            if (Keyboard.current.dKey.isPressed) moveX += 1f;
            if (Keyboard.current.aKey.isPressed) moveX -= 1f;

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move.normalized * walkSpeed * Time.deltaTime);

            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
