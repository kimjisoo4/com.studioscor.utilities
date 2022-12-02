using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.InventorySystem
{
    public abstract class InventorySlot<T> : MonoBehaviour, ISlot<T>
    {
        [SerializeField] protected Image _IconImage;

        private IInventory _Inventory;
        private T _SlotData;

        public IInventory Inventory => _Inventory;
        public T SlotData => _SlotData;
        public T GetData => _SlotData;
        public int GetInventoryLayer => _Inventory.InventoryLayer;

        public bool HasData => _HasData;

        private bool _HasData = false;

        public void SetInventory(IInventory inventory)
        {
            _Inventory = inventory;
        }

        public bool CanSetData(T slotData)
        {
            return slotData is null && !_SlotData.Equals(slotData);
        }
        public bool TrySetData(T slotData)
        {
            if(CanSetData(slotData))
            {
                SetData(slotData);

                return true;
            }

            return false;
        }

        public void SetData(T slotData)
        {
            _SlotData = slotData;

            _HasData = _SlotData is not null;

            UpdateData();
        }
        public void ResetData()
        {
            _HasData = false;

            _SlotData = default;

            UpdateData();
        }

        public abstract void UpdateData();
    }
}