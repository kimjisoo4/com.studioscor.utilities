using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Fade/new Simple Fade", fileName = "SimpleFade")]
    public class SimpleFade : FadeBase
    {
        [Header(" [ Simple Fade ] ")]
        [SerializeField] private SimpleFadeComponent _Fader;
        [SerializeField] private Variable_Float _Amounts;

        private SimpleFadeComponent _Instance;

        public override float Amount => _Amounts.Value;

        protected override void OnReset()
        {
            base.OnReset();

            _Instance = null;
        }

        protected override void OnEndFade()
        {
            if (!_Instance)
                SpawnInstance();
        }

        protected override void OnSetFadeIn()
        {
            if (!_Instance)
                SpawnInstance();
        }
        protected override void OnSetFadeOut()
        {
            if (!_Instance)
                SpawnInstance();
        }
        protected override void OnFadeIn()
        {
            if (!_Instance)
                SpawnInstance();
        }

        protected override void OnFadeOut()
        {
            if (!_Instance)
                SpawnInstance();
        }

        protected void SpawnInstance()
        {
            _Instance = Instantiate(_Fader);

            DontDestroyOnLoad(_Instance);
        }

        protected override void UpdateFadeAmount(float amount)
        {
            _Amounts.SetValue(amount);
        }
    }

}
