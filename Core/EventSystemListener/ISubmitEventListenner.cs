using System;
using UnityEngine.EventSystems;

namespace StudioScor.Utilities
{
    public interface ISubmitEventListenner
    {
        public bool CanSubmit();
        public event Action<BaseEventData> OnSubmited;
    }
}


