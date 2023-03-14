using UnityEngine;


namespace StudioScor.Utilities
{
    public class ChangedFadeStateEventListener : BaseMonoBehaviour
    {
        [Header(" [ Changed Fade State Event Listner ] ")]
        [SerializeField] private FadeBase _Fade;
        [SerializeField] private ChangedFadeStateEvents _Events;


        private void OnEnable()
        {
            _Events.AddListener(_Fade);
        }
        private void OnDisable()
        {
            _Events.RemoveListener(_Fade);
        }
    }

}
