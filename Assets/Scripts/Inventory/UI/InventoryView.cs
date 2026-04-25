using UnityEngine;
using Inventory.Core;
using Inventory.Data;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using System.Collections.Generic;

namespace Inventory.UI
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private InventoryModel _model;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private RectTransform _slotContainer;
        [SerializeField] private Image _draggedIcon;
        [SerializeField] private Canvas _canvas;

        private ObjectPool<SlotView> _slotPool;
        private List<SlotView> _activeSlots = new List<SlotView>();
        private int _draggingIndex = -1;

        private void Awake()
        {
            _slotPool = new ObjectPool<SlotView>(
                createFunc: () => {
                    Transform container = _slotContainer != null ? _slotContainer : transform;
                    GameObject go = Instantiate(_slotPrefab, container);
                    return go.GetComponent<SlotView>();
                },
                actionOnGet: slot => slot.gameObject.SetActive(true),
                actionOnRelease: slot => slot.gameObject.SetActive(false),
                actionOnDestroy: slot => Destroy(slot.gameObject),
                collectionCheck: false,
                defaultCapacity: _model.Size,
                maxSize: 100
            );
        }

        private void OnEnable()
        {
            _model.OnSlotChanged += HandleSlotChanged;
            
            for (int i = 0; i < _model.Size; i++)
            {
                SlotView slot = _slotPool.Get();
                slot.Initialize(i);
                
                slot.OnSlotBeginDrag += HandleBeginDrag;
                slot.OnSlotDrag += HandleDrag;
                slot.OnSlotEndDrag += HandleEndDrag;
                slot.OnSlotDrop += HandleDrop;
                slot.OnSlotRightClick += HandleRightClick;
                
                slot.UpdateView(_model.GetSlot(i));
                _activeSlots.Add(slot);
            }

            if (_draggedIcon != null)
            {
                _draggedIcon.enabled = false;
            }
        }

        private void OnDisable()
        {
            _model.OnSlotChanged -= HandleSlotChanged;

            foreach (var slot in _activeSlots)
            {
                slot.OnSlotBeginDrag -= HandleBeginDrag;
                slot.OnSlotDrag -= HandleDrag;
                slot.OnSlotEndDrag -= HandleEndDrag;
                slot.OnSlotDrop -= HandleDrop;
                slot.OnSlotRightClick -= HandleRightClick;
                
                _slotPool.Release(slot);
            }
            _activeSlots.Clear();
        }

        private void HandleSlotChanged(int index, InventorySlot slot)
        {
            if (index >= 0 && index < _activeSlots.Count)
            {
                _activeSlots[index].UpdateView(slot);
            }
        }

        private void HandleBeginDrag(int index, PointerEventData eventData)
        {
            InventorySlot slot = _model.GetSlot(index);
            if (slot.IsEmpty) return;

            _draggingIndex = index;

            if (_draggedIcon != null)
            {
                _draggedIcon.sprite = slot.Item.Icon;
                _draggedIcon.enabled = true;
                UpdateDraggedIconPosition(eventData);
            }
        }

        private void HandleDrag(PointerEventData eventData)
        {
            if (_draggingIndex == -1 || _draggedIcon == null) return;
            UpdateDraggedIconPosition(eventData);
        }

        private void HandleEndDrag(int index, PointerEventData eventData)
        {
            _draggingIndex = -1;
            if (_draggedIcon != null)
            {
                _draggedIcon.enabled = false;
            }
        }

        private void HandleDrop(int dropIndex)
        {
            if (_draggingIndex != -1 && _draggingIndex != dropIndex)
            {
                _model.SwapOrMergeItems(_draggingIndex, dropIndex);
            }
        }

        private void HandleRightClick(int index)
        {
            _model.SplitItemToEmptySlot(index);
        }

        private void UpdateDraggedIconPosition(PointerEventData eventData)
        {
            if (_canvas == null) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform, 
                eventData.position, 
                _canvas.worldCamera, 
                out Vector2 localPoint);
                
            _draggedIcon.rectTransform.localPosition = localPoint;
        }
    }
}
