using UnityEngine;

namespace FirstPersonInventory
{
    public class PickupCube : MonoBehaviour, IInteractable
    {
        public void Interact(GameObject interactor)
        {
            if (interactor.TryGetComponent(out PlayerInteraction playerInteraction))
            {
                playerInteraction.CollectCube(this);
            }
        }
    }
}
