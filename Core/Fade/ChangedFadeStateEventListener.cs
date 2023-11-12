using UnityEngine;


namespace StudioScor.Utilities
{
    public class ChangedFadeStateEventListener : BaseMonoBehaviour
    {
        [Header(" [ Changed Fade State Event Listner ] ")]
        [SerializeField] private FadeBase _fade;
        [SerializeField] private ChangedFadeStateEvents events;


        private void OnEnable()
        {
            events.AddListener(_fade);
        }
        private void OnDisable()
        {
            events.RemoveListener(_fade);
        }
    }

}
