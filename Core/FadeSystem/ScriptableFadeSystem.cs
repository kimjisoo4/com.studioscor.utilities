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


        public event IFadeSystem.FadeSystemEventHandler OnStartedFadeIn { add => FadeSystemInstance.OnStartedFadeIn += value; remove => FadeSystemInstance.OnStartedFadeIn -= value; }
        public event IFadeSystem.FadeSystemEventHandler OnStartedFadeOut { add => FadeSystemInstance.OnStartedFadeOut += value; remove => FadeSystemInstance.OnStartedFadeOut -= value; }
        public event IFadeSystem.FadeSystemEventHandler OnFinishedFadeIn { add => FadeSystemInstance.OnFinishedFadeIn += value; remove => FadeSystemInstance.OnFinishedFadeIn -= value; }
        public event IFadeSystem.FadeSystemEventHandler OnFinishedFadeOut { add => FadeSystemInstance.OnFinishedFadeOut += value; remove => FadeSystemInstance.OnFinishedFadeOut -= value; }
        public event IFadeSystem.FadeSystemEventHandler OnStartedFading { add => FadeSystemInstance.OnStartedFading += value; remove => FadeSystemInstance.OnStartedFading -= value; }
        public event IFadeSystem.FadeSystemEventHandler OnFinishedFading { add => FadeSystemInstance.OnFinishedFading += value; remove => FadeSystemInstance.OnFinishedFading -= value; }
        public event IFadeSystem.FadeSystemUpdateAmountEventHandler OnUpdatedAmount { add => FadeSystemInstance.OnUpdatedAmount += value; remove => FadeSystemInstance.OnUpdatedAmount -= value; }

        public void EndFadeIn() => FadeSystemInstance.EndFadeIn();

        public void EndFadeOut() => FadeSystemInstance.EndFadeOut();
        public void StartFadeIn(float duration, bool playFromStart = false) => FadeSystemInstance.StartFadeIn(duration, playFromStart);

        public void StartFadeOut(float duration, bool playFromStart = false) => FadeSystemInstance.StartFadeOut(duration, playFromStart);
        public void UpdateFading(float deltaTime) => FadeSystemInstance.UpdateFading(deltaTime);
    }

}
