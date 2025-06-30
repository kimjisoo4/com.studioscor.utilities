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

            if (_fadeSystem.IsFading)
            {
                _fadeUIActor.SetActive(true);
            }
            else
            {
                _fadeUIActor.SetActive(false);
            }

            _fadeSystem.OnFadingStarted += _fadeSystem_OnFadingStarted;
            _fadeSystem.OnFadeInFinished += _fadeSystem_OnFadeInFinished;
            _fadeSystem.OnAmountUpdated += _fadeSystem_OnAmountUpdated;
        }



        private void OnDestroy()
        {
            if (_fadeSystem is not null)
            {
                _fadeSystem.OnFadingStarted -= _fadeSystem_OnFadingStarted;
                _fadeSystem.OnFadeInFinished -= _fadeSystem_OnFadeInFinished;
                _fadeSystem.OnAmountUpdated -= _fadeSystem_OnAmountUpdated;
            }
        }


        private void _fadeSystem_OnFadingStarted(IFadeSystem fadeSystem)
        {
            _fadeUIActor.SetActive(true);
        }
        private void _fadeSystem_OnFadeInFinished(IFadeSystem fadeSystem)
        {
            _fadeUIActor.SetActive(false);
        }

        private void _fadeSystem_OnAmountUpdated(IFadeSystem fadeSystem, float amount)
        {
            _canvasGroup.alpha = amount;
        }
    }

}
