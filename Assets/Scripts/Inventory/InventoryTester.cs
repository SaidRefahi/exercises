using UnityEngine;
using UnityEngine.InputSystem;
using Inventory.Core;
using Inventory.Data;

namespace Inventory
{
    public class InventoryTester : MonoBehaviour
    {
        [SerializeField] private InventoryModel _model;
        [SerializeField] private ItemData[] _testItems;

        private void Update()
        {
            if (Keyboard.current == null) return;

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                AddItem(0);
            }
            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                AddItem(1);
            }
            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                AddItem(2);
            }
            
            if (Keyboard.current.backspaceKey.wasPressedThisFrame)
            {
                _model.RemoveItem(0, 1); // Test removing 1 item from first slot
            }
        }

        private void AddItem(int index)
        {
            if (_testItems == null || _testItems.Length <= index || _testItems[index] == null) return;
            
            _model.AddItem(_testItems[index], 1);
        }
    }
}
