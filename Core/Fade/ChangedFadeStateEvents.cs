using UnityEngine;
using UnityEngine.Events;


namespace StudioScor.Utilities
{
    [System.Serializable]
    public class ChangedFadeStateEvents
    {
        [SerializeField] private UnityEvent OnStartedFadeIn;
        [SerializeField] private UnityEvent OnFinishedFadeIn;
        [SerializeField] private UnityEvent OnStartedFadeOut;
        [SerializeField] private UnityEvent OnFinishedFadeOut;

        public void AddListener(FadeBase fade)
        {
            fade.OnChangedFadeState += Fade_OnChangedFadeState;
        }
        public void RemoveListener(FadeBase fade)
        {
            fade.OnChangedFadeState -= Fade_OnChangedFadeState;
        }

        private void Fade_OnChangedFadeState(EFadeState state)
        {
            OnTriggerEvent(state);
        }
        
        public void OnTriggerEvent(EFadeState state)
        {
            switch (state)
            {
                case EFadeState.None:
                    break;
                case EFadeState.Fade:
                    OnFinishedFadeIn?.Invoke();
                    break;
                case EFadeState.NotFade:
                    OnFinishedFadeOut?.Invoke();
                    break;
                case EFadeState.FadeIn:
                    OnStartedFadeIn?.Invoke();
                    break;
                case EFadeState.FadeOut:
                    OnStartedFadeOut?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }

}
