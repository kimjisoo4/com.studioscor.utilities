using UnityEngine;
using UnityEngine.Events;


namespace StudioScor.Utilities
{

    public abstract class FadeBase : BaseScriptableObject, ISerializationCallbackReceiver
    {
        private EFadeState _State;
        public abstract float Amount { get; }

        public EFadeState State => _State;

        public event UnityAction<EFadeState> OnChangedFadeState;

        public void OnAfterDeserialize()
        {
            OnReset();
        }

        public void OnBeforeSerialize()
        {
        }

        protected override void OnReset()
        {
            base.OnReset();

            _State = Amount.SafeEquals(1f) ? EFadeState.Fade : EFadeState.NotFade;
        }

        protected void TransitionState(EFadeState newState)
        {
            _State = newState;

            Callback_OnChanged();
        }

        public void SetFadeOut()
        {
            SetFadeAmount(1);

            OnSetFadeIn();
        }

        public void SetFadeIn()
        {
            SetFadeAmount(0);
        }

        public void FadeOut()
        {
            TransitionState(EFadeState.FadeOut);

            OnFadeOut();
        }
        public void FadeIn()
        {
            TransitionState(EFadeState.FadeIn);

            OnFadeIn();
        }
        public void EndFade()
        {
            if(Amount > 0.5f)
            {
                SetFadeAmount(1);
            }
            else
            {
                SetFadeAmount(0);
            }

            OnEndFade();
        }

        public void SetFadeAmount(float amount)
        {
            amount = Mathf.Clamp01(amount);

            UpdateFadeAmount(amount);

            Log($"Set Fade Amount - [ {amount:N2} ] ");

            if (Amount >= 1f)
            {
                TransitionState(EFadeState.Fade);
            }
            else if (Amount <= 0f)
            {
                TransitionState(EFadeState.NotFade);
            }
        }
        protected abstract void UpdateFadeAmount(float amount);

        protected virtual void OnSetFadeIn()
        {
        }
        protected virtual void OnSetFadeOut()
        {
        }
        protected abstract void OnFadeIn();
        protected abstract void OnFadeOut();
        protected abstract void OnEndFade();


        protected void Callback_OnChanged()
        {
            Log($"On Changed Fade State - {_State} ");

            OnChangedFadeState?.Invoke(_State);
        }
    }

}
