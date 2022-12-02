using UnityEngine;

namespace StudioScor.InventorySystem
{
    public class OnHovering : MonoBehaviour
    {
        [SerializeField] private float _HoveringTime = 1f;
        [SerializeField] private SlotInformation _SlotInformation;

        private OnHoverSlot _CurrentHoverSlot;
        private float _RemainTime = 0f;
        private bool _IsHovering = false;
        private bool _WasHovered = false;

        public void EnterHover(OnHoverSlot slot)
        {
            _CurrentHoverSlot = slot;

            _IsHovering = true;

            if (_WasHovered)
            {
                _RemainTime = 0f;

                OnHover();
            }
            else
            {
                _RemainTime = _HoveringTime;
            }
        }
        public void ExitHover(OnHoverSlot slot)
        {
            if (_CurrentHoverSlot != slot)
                return;

            _CurrentHoverSlot = null;

            _IsHovering = false;

            EndHover();
        }

        private void Update()
        {
            if(_IsHovering)
            {
                if (_WasHovered)
                    return;

                _RemainTime -= Time.deltaTime;

                if (_RemainTime <= 0f)
                {
                    _WasHovered = true;

                    OnHover();
                }
            }
            else
            {
                if (!_WasHovered)
                    return;

                _RemainTime += Time.deltaTime;

                if (_RemainTime >= _HoveringTime)
                {
                    _WasHovered = false;
                }
            }
        }
        protected void OnHover()
        {
            _SlotInformation.OnInformation(_CurrentHoverSlot);
        }
        protected void EndHover()
        {
            _SlotInformation.EndInfomation();
        }
    }

}
