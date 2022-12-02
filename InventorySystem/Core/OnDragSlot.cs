using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

namespace StudioScor.InventorySystem
{

    public class OnDragSlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _Image;
        public void OnBeginDrag(PointerEventData eventData)
        {
            _Image.transform.SetParent(transform.root);

            _Image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _Image.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _Image.transform.SetParent(transform);

            _Image.transform.localPosition = Vector3.zero;

            _Image.raycastTarget = true;
        }
    }

}