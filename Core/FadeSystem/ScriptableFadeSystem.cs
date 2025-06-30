using UnityEngine;


namespace StudioScor.Utilities.FadeSystem
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/FadeSystem/new Scriptable Fade System", fileName ="Fade_")]
    public class ScriptableFadeSystem : BaseScriptableObject, IFadeSystem
    {
        [Header(" [ Scriptable Fade System ] ")]
        [SerializeField] private FadeSystemComponent _fadeSystemComponent;

        private FadeSystemComponent _fadeSystemInstance;

        public FadeSystemComponent FadeSystemInstance
        {
            get
            {
                if (_fadeSystemInstance == null)
                {
                    _fadeSystemInstance = GameObject.Instantiate(_fadeSystemComponent);
                }

                return _fadeSystemInstance;
            }
        }

        public float Duration => FadeSystemInstance.Duration;
        public float Amount => FadeSystemInstance.Amount;
        public bool IsFading => FadeSystemInstance.IsFading;
        public EFadeState State => FadeSystemInstance.State;

        public event IFadeSystem.FadeSystemEventHandler OnFadeInStarted
        {
            add
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnFadeInStarted += value;
                else
                    return;
            }
            remove
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnFadeInStarted -= value;
                else
                    return;
            }
        }

        public event IFadeSystem.FadeSystemEventHandler OnFadeOutStarted
        {
            add
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnFadeOutStarted += value;
                else
                    return;
            }
            remove
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnFadeOutStarted -= value;
                else
                    return;
            }
        }

        public event IFadeSystem.FadeSystemEventHandler OnFadeInFinished
        {
            add
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnFadeInFinished += value;
                else
                    return;
            }
            remove
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnFadeInFinished -= value;
                else
                    return;
            }
        }
        public event IFadeSystem.FadeSystemEventHandler OnFadeOutFinished
        {
            add
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnFadeOutFinished += value;
                else
                    return;
            }
            remove
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnFadeOutFinished -= value;
                else
                    return;
            }
        }
        public event IFadeSystem.FadeSystemEventHandler OnFadingStarted
        {
            add
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnFadingStarted += value;
                else
                    return;
            }
            remove
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnFadingStarted -= value;
                else
                    return;
            }
        }
        public event IFadeSystem.FadeSystemEventHandler OnFadingFinished
        {
            add
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnFadingFinished += value;
                else
                    return;
            }
            remove
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnFadingFinished -= value;
                else
                    return;
            }
        }

        public event IFadeSystem.FadeSystemUpdateAmountEventHandler OnAmountUpdated
        {
            add
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnAmountUpdated += value;
                else
                    return;
            }
            remove
            {
                if (_fadeSystemInstance)
                    _fadeSystemInstance.OnAmountUpdated -= value;
                else
                    return;
            }
        }


        protected override void OnReset()
        {
            base.OnReset();

            _fadeSystemInstance = null;
        }

        public void EndFadeIn() => FadeSystemInstance.EndFadeIn();
        public void EndFadeOut() => FadeSystemInstance.EndFadeOut();
        public void StartFadeIn(float duration, bool playFromStart = false) => FadeSystemInstance.StartFadeIn(duration, playFromStart);
        public void StartFadeOut(float duration, bool playFromStart = false) => FadeSystemInstance.StartFadeOut(duration, playFromStart);
        public void UpdateFading(float deltaTime) => FadeSystemInstance.UpdateFading(deltaTime);
    }

}
