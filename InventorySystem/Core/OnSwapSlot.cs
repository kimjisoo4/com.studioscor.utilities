using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using UnityEngine.EventSystems;

namespace StudioScor.InventorySystem
{
    public class OnSwapSlot<T> : MonoBehaviour, IEndDragHandler 
    {
        [SerializeField] private GraphicRaycaster _GraphicRaycaster;
        [SerializeField] private bool _UseOutsideRemove = false;

        private ISlot<T> _Slot;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            _Slot = transform.GetComponent<ISlot<T>>();
            _GraphicRaycaster = transform.root.GetComponent<GraphicRaycaster>();
        }
#endif
        void Awake()
        {
            if(_Slot is null || (Object)_Slot)
                _Slot = transform.GetComponent<ISlot<T>>();

            if(_GraphicRaycaster)
                _GraphicRaycaster = transform.root.GetComponent<GraphicRaycaster>();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            List<RaycastResult> results = new();

            _GraphicRaycaster.Raycast(eventData, results);

            bool hasSlot = false;


            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    if (result.gameObject.TryGetComponent(out ISlot<T> slot))
                    {
                        hasSlot = true;

                        TrySwapData(slot);

                        break;
                    }
                }
            }

            if (_UseOutsideRemove && !hasSlot)
            {
                _Slot.ResetData();
            }
        }

        public void TrySwapData(ISlot<T> slot)
        {
            int inventoryLayer = _Slot.GetInventoryLayer;
            int targetLayer = slot.GetInventoryLayer;

            if (inventoryLayer < targetLayer)
            {
                slot.SetData(_Slot.GetData);
            }
            else if (inventoryLayer == targetLayer)
            {
                var data = _Slot.GetData;

                _Slot.SetData(slot.GetData);
                slot.SetData(data);
            }
            else
            {
                if (_UseOutsideRemove)
                {
                    _Slot.ResetData();
                }
            }
        }
    }

}