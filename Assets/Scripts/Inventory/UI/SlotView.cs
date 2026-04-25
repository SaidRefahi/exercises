using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using Inventory.Core;

namespace Inventory.UI
{
    public class SlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _quantityText;

        public int Index { get; private set; }
        
        public event Action<int, PointerEventData> OnSlotBeginDrag;
        public event Action<PointerEventData> OnSlotDrag;
        public event Action<int, PointerEventData> OnSlotEndDrag;
        public event Action<int> OnSlotDrop;
        public event Action<int> OnSlotRightClick;

        public void Initialize(int index)
        {
            Index = index;
            Clear();
        }

        public void UpdateView(InventorySlot slot)
        {
            if (slot.IsEmpty)
            {
                Clear();
                return;
            }

            _iconImage.sprite = slot.Item.Icon;
            _iconImage.enabled = true;
            
            _quantityText.text = slot.Quantity > 1 ? slot.Quantity.ToString() : "";
            _quantityText.enabled = true;
        }

        public void Clear()
        {
            _iconImage.sprite = null;
            _iconImage.enabled = false;
            _quantityText.text = "";
            _quantityText.enabled = false;
        }

        public void OnBeginDrag(PointerEventData eventData) => OnSlotBeginDrag?.Invoke(Index, eventData);
        public void OnDrag(PointerEventData eventData) => OnSlotDrag?.Invoke(eventData);
        public void OnEndDrag(PointerEventData eventData) => OnSlotEndDrag?.Invoke(Index, eventData);
        public void OnDrop(PointerEventData eventData) => OnSlotDrop?.Invoke(Index);

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnSlotRightClick?.Invoke(Index);
            }
        }
    }
}
