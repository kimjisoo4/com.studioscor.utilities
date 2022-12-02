using UnityEngine;

namespace StudioScor.Utilities
{
    
    public class FadeInState : FadeState
    {
        [SerializeField] private CanvasGroup _CanvasGroup;

        private Timer _Timer;
        public override float FadeAmount => _CanvasGroup.alpha;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            gameObject.TryGetComponentInParentOrChildren(out _CanvasGroup);
        }
#endif
        private void Awake()
        {
            _Timer = new Timer(SimpleFade.FadeInTime);
            _Timer.OnFinishedTimer += Timer_OnFinishedTimer;
        }

        private void Timer_OnFinishedTimer(Timer timer)
        {
            SimpleFade.EndFadeIn();
        }

        private void OnEnable()
        {
            float fadeTime = SimpleFade.FadeInTime;

            _Timer.OnTimer(fadeTime);

            if(!SimpleFade.IsPlayFromStart)
            {
                float alpha = _CanvasGroup.alpha;

                _Timer.JumpTimer(fadeTime * alpha);
            }
        }

        private void Update()
        {
            _Timer.UpdateTimer(Time.deltaTime);

            _CanvasGroup.alpha = _Timer.NormalizedTime;
        }
    }

}
