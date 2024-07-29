using UnityEngine;

namespace StudioScor.Utilities
{
    public class SimpleFadeComponent : BaseMonoBehaviour
    {
        [Header(" [ Simple Fade Component ] ")]
        [SerializeField] private FiniteStateMachineSystem<SimpleFadeState> stateMachine;
        [SerializeField] private FadeBase fade;
        [SerializeField] private SimpleFadeState fadeInState;
        [SerializeField] private SimpleFadeState fadeOutState;

        public FadeBase Fade => fade;
        public float Amount => fade.Amount;

        public void SetFadeAmount(float amount)
        {
            Log($"Set Fade Amount - [{amount:N2}]");

            fade.SetFadeAmount(amount);
        }

        private void Awake()
        {
            stateMachine.Setup();

            Fade_OnChangedFadeState(fade.State);
            fade.OnChangedFadeState += Fade_OnChangedFadeState;
        }
        private void OnDestroy()
        {
            fade.OnChangedFadeState -= Fade_OnChangedFadeState;
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

            stateMachine.TrySetDefaultState();
        }
        private void OnFadeIn()
        {
            Log(" On Fade In");

            stateMachine.TrySetState(fadeInState);
        }
        private void OnFadeOut()
        {
            Log(" On Fade Out");

            stateMachine.TrySetState(fadeOutState);
        }


    }

}
