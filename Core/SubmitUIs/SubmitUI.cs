using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    public abstract class SubmitUI : BaseMonoBehaviour, ISubmitHandler, IPointerClickHandler
    {
        [Header(" [ Submit UI ] ")]
        [SerializeField] private Selectable selectable;

        public event UnityAction<SubmitUI> OnSubmitUI;

        protected virtual void Reset()
        {
            selectable = GetComponentInChildren<Selectable>();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Log("On Pointer Click Handler");

            TrySubmitUI();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Log("On Sumbit Handler");

            TrySubmitUI();
        }

        public bool TrySubmitUI()
        {
            Log(" Try Submit UI ");

            if (!CanSubmitUI())
            {
                Log(" Can Not Submit", SUtility.NAME_COLOR_RED);

                return false;
            }

            OnSubmit();

            return true;
        }
        public virtual bool CanSubmitUI()
        {
            return selectable.interactable;
        }

        public void OnSubmit()
        {
            Log("On Submit");

            Submit();

            OnSubmitUI?.Invoke(this);
        }

        protected abstract void Submit();
    }
}