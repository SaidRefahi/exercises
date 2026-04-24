using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPersonController
{
    public class TargetLock : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionReference lockOnAction;

        [Header("Targeting Settings")]
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float lockOnRadius = 15f;
        
        [Header("Camera Support")]
        [SerializeField] private GameObject lockOnCameraObject;
        [Tooltip("An empty GameObject that the Cinemachine Camera looks at. This script will move it to the enemy.")]
        [SerializeField] private Transform lockOnLookAtPoint;

        public Transform CurrentTarget { get; private set; }
        public bool HasTarget => CurrentTarget != null;

        private void OnEnable()
        {
            if (lockOnAction != null)
            {
                lockOnAction.action.Enable();
                lockOnAction.action.performed += OnLockOnToggle;
            }
        }

        private void OnDisable()
        {
            if (lockOnAction != null)
            {
                lockOnAction.action.Disable();
                lockOnAction.action.performed -= OnLockOnToggle;
            }
        }

        private void Update()
        {
            // Keep the LookAt point snapped to the enemy so Cinemachine can track it
            if (HasTarget && lockOnLookAtPoint != null)
            {
                lockOnLookAtPoint.position = CurrentTarget.position;
            }
        }

        private void OnLockOnToggle(InputAction.CallbackContext context)
        {
            if (HasTarget)
            {
                ClearTarget();
            }
            else
            {
                FindAndLockTarget();
            }
        }

        private void FindAndLockTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, lockOnRadius, enemyLayer);
            
            if (colliders.Length > 0)
            {
                // Find closest
                float closestDistance = Mathf.Infinity;
                Transform closestTarget = null;

                foreach (var col in colliders)
                {
                    float dist = Vector3.Distance(transform.position, col.transform.position);
                    if (dist < closestDistance)
                    {
                        closestDistance = dist;
                        closestTarget = col.transform;
                    }
                }

                CurrentTarget = closestTarget;
                
                if (lockOnCameraObject != null)
                {
                    lockOnCameraObject.SetActive(true);
                }
            }
        }

        private void ClearTarget()
        {
            CurrentTarget = null;
            
            if (lockOnCameraObject != null)
            {
                lockOnCameraObject.SetActive(false);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, lockOnRadius);
        }
    }
}
