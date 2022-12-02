using UnityEngine;

namespace StudioScor.InventorySystem
{
    public abstract class SlotInformation : MonoBehaviour
    {
        protected OnHoverSlot _OnHoverSlot;

        public void OnInformation(OnHoverSlot hoverSlot)
        {
            _OnHoverSlot = hoverSlot;

            if (UpdateInformation())
            {
                gameObject.SetActive(true);
            }
        }

        public void EndInfomation()
        {
            _OnHoverSlot = null;

            gameObject.SetActive(false);
        }

        public abstract bool UpdateInformation();
    }

}
