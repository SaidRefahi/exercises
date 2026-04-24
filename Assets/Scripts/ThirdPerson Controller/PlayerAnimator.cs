using UnityEngine;

namespace ThirdPersonController
{
    public class PlayerAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator _animator;
        [SerializeField] private ThirdPersonMovement _movementController;

        [Header("Settings")]
        [SerializeField] private float _speedDampTime = 0.1f;

        private readonly int _speedHash = Animator.StringToHash("Speed");
        private readonly int _isGroundedHash = Animator.StringToHash("IsGrounded");
        private readonly int _jumpHash = Animator.StringToHash("Jump");

        private float _targetSpeed;

        private void OnEnable()
        {
            if (_movementController == null) return;
            
            _movementController.OnSpeedChanged += HandleSpeedChanged;
            _movementController.OnJumped += HandleJumped;
            _movementController.OnGroundedChanged += HandleGroundedChanged;
        }

        private void OnDisable()
        {
            if (_movementController == null) return;

            _movementController.OnSpeedChanged -= HandleSpeedChanged;
            _movementController.OnJumped -= HandleJumped;
            _movementController.OnGroundedChanged -= HandleGroundedChanged;
        }

        private void HandleSpeedChanged(float speed)
        {
            _targetSpeed = speed;
        }

        private void HandleJumped()
        {
            if (_animator != null)
            {
                _animator.SetTrigger(_jumpHash);
            }
        }

        private void HandleGroundedChanged(bool isGrounded)
        {
            if (_animator != null)
            {
                _animator.SetBool(_isGroundedHash, isGrounded);
            }
        }

        private void Update()
        {
            if (_animator == null) return;

            _animator.SetFloat(_speedHash, _targetSpeed, _speedDampTime, Time.deltaTime);
        }
    }
}
