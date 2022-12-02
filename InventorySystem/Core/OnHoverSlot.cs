using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

namespace StudioScor.InventorySystem
{

    public class OnHoverSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private OnHovering _OnHovering;

        private bool _IsEnter = false;

#if UNITY_EDITOR
        void Reset()
        {
            _OnHovering = GetComponentInParent<OnHovering>();
        }
#endif
        private void OnDisable()
        {
            if (_IsEnter)
            {
                _IsEnter = false;

                _OnHovering.ExitHover(this);
            }
                
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            _IsEnter = true;

            _OnHovering.EnterHover(this);   
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _IsEnter = false;

            _OnHovering.ExitHover(this);
        }
    }

}
