using UnityEngine;


namespace StudioScor.Utilities.FadeSystem
{
    public class CanvasGroupAlphaFader : BaseMonoBehaviour
    {
        [Header(" [ Canvas Group Alpha Fader ] ")]
        [SerializeField] private GameObject _fadeUIActor;
        [SerializeField] private GameObject _fadeSystemActor;
        [SerializeField] private CanvasGroup _canvasGroup;

        private IFadeSystem _fadeSystem;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if(!_fadeSystemActor)
            {
                _fadeSystemActor = gameObject.GetGameObjectByType<IFadeSystem>();
            }

            if(!_canvasGroup)
            {
                _canvasGroup = GetComponentInChildren<CanvasGroup>();
            }
#endif
        }
        private void Awake()
        {
            _fadeSystem = _fadeSystemActor.GetComponent<IFadeSystem>();

            _canvasGroup.alpha = _fadeSystem.Amount;

            _fadeSystem.OnStartedFadeOut += _fadeSystem_OnStartedFadeOut;
            _fadeSystem.OnFinishedFadeIn += _fadeSystem_OnFinishedFadeIn;
            _fadeSystem.OnUpdatedAmount += _fadeSystem_OnUpdatedAmount;

            if (_fadeSystem.State == EFadeState.FadeIn && !_fadeSystem.IsFading)
            {
                _fadeUIActor.SetActive(true);
            }
            else
            {
                _fadeUIActor.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (_fadeSystem is not null)
            {
                _fadeSystem.OnStartedFadeOut -= _fadeSystem_OnStartedFadeOut;
                _fadeSystem.OnFinishedFadeIn -= _fadeSystem_OnFinishedFadeIn;
                _fadeSystem.OnUpdatedAmount -= _fadeSystem_OnUpdatedAmount;
            }
        }


        private void _fadeSystem_OnFinishedFadeIn(IFadeSystem fadeSystem)
        {
            _fadeUIActor.SetActive(false);
        }

        private void _fadeSystem_OnStartedFadeOut(IFadeSystem fadeSystem)
        {
            _fadeUIActor.SetActive(true);
        }

       

        private void _fadeSystem_OnUpdatedAmount(IFadeSystem fadeSystem, float amount)
        {
            _canvasGroup.alpha = amount;
        }
    }

}
