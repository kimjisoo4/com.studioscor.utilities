using UnityEngine;

namespace StudioScor.Utilities
{
    public class SimpleFadeComponent : BaseMonoBehaviour
    {
        [Header(" [ Simple Fade Component ] ")]
        [SerializeField] private FiniteStateMachineSystem<SimpleFadeState> _StateMachine;
        [SerializeField] private FadeBase _Fade;
        [SerializeField] private SimpleFadeState _FadeInState;
        [SerializeField] private SimpleFadeState _FadeOutState;

        public FadeBase Fade => _Fade;
        public float Amount => _Fade.Amount;

        public void SetFadeAmount(float amount)
        {
            Log($"Set Fade Amount - [{amount:N2}]");

            _Fade.SetFadeAmount(amount);
        }

        private void Awake()
        {
            _StateMachine.Setup();
        }

        private void Start()
        {
            Fade_OnChangedFadeState(_Fade.State);

            _Fade.OnChangedFadeState += Fade_OnChangedFadeState;
        }

        private void OnDestroy()
        {
            _Fade.OnChangedFadeState -= Fade_OnChangedFadeState;
        }

        private void Fade_OnChangedFadeState(EFadeState state)
        {
            switch (state)
            {
                case EFadeState.FadeIn:
                    OnFadeIn();
                    break;
                case EFadeState.FadeOut:
                    OnFadeOut();
                    break;
                default:
                    EndFade();
                    break;
            }
        }

        private void EndFade()
        {
            Log(" End Fade");

            _StateMachine.TrySetDefaultState();
        }
        private void OnFadeIn()
        {
            Log(" On Fade In");

            _StateMachine.TrySetState(_FadeInState);
        }
        private void OnFadeOut()
        {
            Log(" On Fade Out");

            _StateMachine.TrySetState(_FadeOutState);
        }


    }

}
