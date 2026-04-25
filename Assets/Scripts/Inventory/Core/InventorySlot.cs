using Inventory.Data;
using System;

namespace Inventory.Core
{
    [Serializable]
    public struct InventorySlot
    {
        public ItemData Item;
        public int Quantity;

        public bool IsEmpty => Item == null || Quantity <= 0;

        public InventorySlot(ItemData item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public void Clear()
        {
            Item = null;
            Quantity = 0;
        }
    }
}
