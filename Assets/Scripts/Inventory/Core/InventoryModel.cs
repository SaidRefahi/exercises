using Inventory.Data;
using System;
using UnityEngine;

namespace Inventory.Core
{
    public class InventoryModel : MonoBehaviour
    {
        [SerializeField] private int _inventorySize = 24;
        
        private InventorySlot[] _slots;

        public event Action<int, InventorySlot> OnSlotChanged;
        public int Size => _inventorySize;

        private void Awake()
        {
            EnsureInitialized();
        }

        public void EnsureInitialized()
        {
            if (_slots != null) return;
            
            _slots = new InventorySlot[_inventorySize];
            for (int i = 0; i < _inventorySize; i++)
            {
                _slots[i] = new InventorySlot(null, 0);
            }
        }

        public InventorySlot GetSlot(int index)
        {
            EnsureInitialized();
            if (index < 0 || index >= _slots.Length) return new InventorySlot(null, 0);
            return _slots[index];
        }

        public bool AddItem(ItemData item, int quantity)
        {
            if (item == null || quantity <= 0) return false;

            for (int i = 0; i < _slots.Length; i++)
            {
                if (!_slots[i].IsEmpty && _slots[i].Item == item && _slots[i].Quantity < item.MaxStackSize)
                {
                    int spaceAvailable = item.MaxStackSize - _slots[i].Quantity;
                    int amountToAdd = Mathf.Min(quantity, spaceAvailable);
                    
                    _slots[i].Quantity += amountToAdd;
                    quantity -= amountToAdd;
                    
                    OnSlotChanged?.Invoke(i, _slots[i]);

                    if (quantity <= 0) return true;
                }
            }

            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].IsEmpty)
                {
                    int amountToAdd = Mathf.Min(quantity, item.MaxStackSize);
                    
                    _slots[i].Item = item;
                    _slots[i].Quantity = amountToAdd;
                    quantity -= amountToAdd;
                    
                    OnSlotChanged?.Invoke(i, _slots[i]);

                    if (quantity <= 0) return true;
                }
            }

            return false;
        }

        public void RemoveItem(int index, int quantity)
        {
            if (index < 0 || index >= _slots.Length || _slots[index].IsEmpty) return;

            _slots[index].Quantity -= quantity;
            if (_slots[index].Quantity <= 0)
            {
                _slots[index].Clear();
            }

            OnSlotChanged?.Invoke(index, _slots[index]);
        }

        public void SwapOrMergeItems(int sourceIndex, int destIndex)
        {
            if (sourceIndex == destIndex) return;
            if (sourceIndex < 0 || sourceIndex >= _slots.Length || destIndex < 0 || destIndex >= _slots.Length) return;

            InventorySlot sourceSlot = _slots[sourceIndex];
            InventorySlot destSlot = _slots[destIndex];

            if (sourceSlot.IsEmpty) return;

            if (!destSlot.IsEmpty && sourceSlot.Item == destSlot.Item)
            {
                int spaceAvailable = destSlot.Item.MaxStackSize - destSlot.Quantity;
                if (spaceAvailable > 0)
                {
                    int amountToMove = Mathf.Min(sourceSlot.Quantity, spaceAvailable);
                    
                    _slots[destIndex].Quantity += amountToMove;
                    _slots[sourceIndex].Quantity -= amountToMove;

                    if (_slots[sourceIndex].Quantity <= 0)
                    {
                        _slots[sourceIndex].Clear();
                    }

                    OnSlotChanged?.Invoke(destIndex, _slots[destIndex]);
                    OnSlotChanged?.Invoke(sourceIndex, _slots[sourceIndex]);
                }
            }
            else
            {
                _slots[destIndex] = sourceSlot;
                _slots[sourceIndex] = destSlot;

                OnSlotChanged?.Invoke(destIndex, _slots[destIndex]);
                OnSlotChanged?.Invoke(sourceIndex, _slots[sourceIndex]);
            }
        }

        public void SplitItemToEmptySlot(int sourceIndex)
        {
            if (sourceIndex < 0 || sourceIndex >= _slots.Length) return;
            
            InventorySlot sourceSlot = _slots[sourceIndex];
            if (sourceSlot.IsEmpty || sourceSlot.Quantity <= 1) return;

            int halfQuantity = sourceSlot.Quantity / 2;
            
            int emptyIndex = -1;
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].IsEmpty)
                {
                    emptyIndex = i;
                    break;
                }
            }

            if (emptyIndex != -1)
            {
                _slots[emptyIndex].Item = sourceSlot.Item;
                _slots[emptyIndex].Quantity = halfQuantity;
                
                _slots[sourceIndex].Quantity -= halfQuantity;
                
                OnSlotChanged?.Invoke(emptyIndex, _slots[emptyIndex]);
                OnSlotChanged?.Invoke(sourceIndex, _slots[sourceIndex]);
            }
        }
    }
}
