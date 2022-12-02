using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.InventorySystem
{
    public interface IInventory
    {
        public int InventoryLayer { get; }
    }

    [System.Serializable]
    public class Inventory<T> : IInventory
    {
        [SerializeField] private int _InventoryLayer = 0;

        [SerializeField] private RectTransform _SlotContainer;
        [SerializeField] private InventorySlot<T> _InventorySlot;

        [SerializeField] private Vector2 _SlotCount = new Vector2(4,6);
        [SerializeField] private Vector2 _SlotSize = new Vector2(100,100);
        [SerializeField] private float _Spacing = 5f;
        
        [SerializeField] private bool _CreateToStart = false;

        [ContextMenuItem("Create Slots", "CreateSlots")]
        [ContextMenuItem("Clear Slots", "RemoveSlots")]
        [SerializeField] private InventorySlot<T>[] _Slots;

        public int InventoryLayer => _InventoryLayer;
        public IReadOnlyCollection<InventorySlot<T>> Slots => _Slots;

        public void Setup()
        {
            if (_CreateToStart)
            {
                CreateSlots();
            }
            else
            {
                SetupSlot();
            }
        }

        public void CreateSlots()
        {
            SetContainerSize();

            RemoveSlots();

            int count = Mathf.RoundToInt(_SlotCount.x * _SlotCount.y);

            _Slots = new InventorySlot<T>[count];

            for(int i = 0; i < count; i++)
            {
                _Slots[i] = Object.Instantiate(_InventorySlot, _SlotContainer);

                _Slots[i].gameObject.SetActive(true);
            }

            SetupSlot();
        }
        private void SetupSlot()
        {
            foreach (var slot in _Slots)
            {
                slot.SetInventory(this);

                slot.UpdateData();
            }
        }
        public void RemoveSlots()
        {
            if (_Slots is not null)
            {
                for(int i = 0; i < _Slots.Length; i++)
                {
                    if (_Slots[i])
                    {
                        if (Application.isEditor)
                        {
                            Object.DestroyImmediate(_Slots[i].gameObject);
                        }
                        else
                        {
                            Object.Destroy(_Slots[i].gameObject);
                        }
                    }
                   
                }
            }

            _Slots = null;
        }


        public void SetContainerSize()
        {
            float slotX = _SlotCount.x;
            float slotSizeX = _SlotSize.x;

            float width = (slotX * slotSizeX) + ((slotX - 1) * _Spacing);

            float slotY = _SlotCount.y;
            float slotSizeY = _SlotSize.y;

            float height = (slotY * slotSizeY) + ((slotY - 1) * _Spacing);

            _SlotContainer.sizeDelta = new Vector2(width, height);
        }

        protected bool FindSlots(T data, out InventorySlot<T> slot)
        {
            for (int i = 0; i < _Slots.Length; i++)
            {
                if (_Slots[i].TrySetData(data))
                {
                    slot = _Slots[i];

                    return true;
                }
            }

            slot = null;

            return false;
        }
        protected bool FindEmptySlot(out InventorySlot<T> slot)
        {
            for (int i = 0; i < _Slots.Length; i++)
            {
                if (_Slots[i].SlotData is null)
                {
                    slot = _Slots[i];

                    return true;
                }
            }

            slot = null;

            return false;
        }

        public void AddItem(T newItem)
        {
            if (FindEmptySlot(out InventorySlot<T> slot))
            {
                slot.SetData(newItem);
            }
        }
        public void RemoveItem(T removeItem)
        {
            if (FindSlots(removeItem, out InventorySlot<T> slot))
            {
                slot.ResetData();
            }
        }

        public void ClearInventory()
        {
            foreach (var item in _Slots)
            {
                item.ResetData();
            }
        }
    }

}