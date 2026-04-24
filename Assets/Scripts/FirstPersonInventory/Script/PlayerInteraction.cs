using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

namespace FirstPersonInventory
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private LayerMask placementLayer;

        [Header("Inventory & Pooling")]
        [SerializeField] private PickupCube cubePrefab;
        [SerializeField] private int initialCubes = 0;

        private ObjectPool<PickupCube> cubePool;
        private int currentCubeCount;

        private void Awake()
        {
            currentCubeCount = initialCubes;
            
            cubePool = new ObjectPool<PickupCube>(
                createFunc: () => {
                    var cube = Instantiate(cubePrefab);
                    cube.gameObject.SetActive(false);
                    return cube;
                },
                actionOnGet: cube => cube.gameObject.SetActive(true),
                actionOnRelease: cube => cube.gameObject.SetActive(false),
                actionOnDestroy: cube => Destroy(cube.gameObject),
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 50
            );
        }

        private void Update()
        {
            if (Keyboard.current == null || Mouse.current == null || cameraTransform == null) return;

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                TryInteract();
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                TryPlaceCube();
            }
        }

        private void TryInteract()
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactionDistance, interactableLayer))
            {
                if (hit.collider.TryGetComponent(out PickupCube cubeInteractable))
                {
                    cubeInteractable.Interact(this.gameObject);
                }
            }
        }

        private void TryPlaceCube()
        {
            if (currentCubeCount <= 0 || cubePrefab == null) return;

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactionDistance, placementLayer))
            {
                Vector3 placePosition = hit.point + hit.normal * 0.5f; 
                
                PickupCube newCube = cubePool.Get();
                newCube.transform.position = placePosition;
                newCube.transform.rotation = Quaternion.identity;
                
                currentCubeCount--;
                Debug.Log($"Cube placed. Cubes remaining: {currentCubeCount}");
            }
        }

        public void CollectCube(PickupCube cubeToCollect)
        {
            cubePool.Release(cubeToCollect);
            currentCubeCount++;
            Debug.Log($"Cube picked up. Total cubes: {currentCubeCount}");
        }
    }
}
