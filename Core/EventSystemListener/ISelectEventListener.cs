using System;
using UnityEngine.EventSystems;

namespace StudioScor.Utilities
{
    public interface ISelectEventListener
    {
        public bool CanSelect();
        public event Action<BaseEventData> OnSelected;
    }
}


