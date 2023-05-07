using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/SimpleAmount/Modifier/Delayed Amount Fill Modifier", order: 0)]
    public class DelayedAmountFillModifier : SimpleAmountModifier
    {
        public enum EDelayedState
        {
            None,
            Delay,
            Update,
        }

        [Header(" [ Delayed Amount Fill Modifier ] ")]
        [SerializeField] private Image _Fill;
        [Space(5f)]
        [SerializeField] private float _Delay = 0.2f;
        [SerializeField] private float _Speed = 5f;

        private float _RemainTime;

        private float _CurrentPoint;
        private float _TargetPoint;

        private EDelayedState _State = EDelayedState.None;

        protected override void Reset()
        {
#if UNITY_EDITOR
            base.Reset();

            gameObject.TryGetComponentInParentOrChildren(out _Fill);
#endif
        }

        private void LateUpdate()
        {
            switch (_State)
            {
                case EDelayedState.None:
                    return;
                case EDelayedState.Delay:
                    UpdateDelay();
                    break;
                case EDelayedState.Update:
                    UpdateAmount();
                    break;
                default:
                    break;
            }
        }

        public override void UpdateModifier()
        {
            if (SimpleAmount.CurrentValue > SimpleAmount.PrevValue)
            {
                _TargetPoint = SimpleAmount.NormalizedValue;
                _CurrentPoint = _TargetPoint;

                _Fill.fillAmount = _CurrentPoint;

                _State = EDelayedState.None;

                return;
            }


            switch (_State)
            {
                case EDelayedState.None:
                    break;
                case EDelayedState.Delay:
                    break;
                case EDelayedState.Update:
                    _CurrentPoint = _TargetPoint;

                    _Fill.fillAmount = _CurrentPoint;
                    break;
                default:
                    break;
            }

            _TargetPoint = SimpleAmount.NormalizedValue;
            _RemainTime = _Delay;
            _State = EDelayedState.Delay;
        }

        private void UpdateDelay()
        {
            float deltaTime = Time.deltaTime;

            _RemainTime -= deltaTime;

            if (_RemainTime <= 0f)
                _State = EDelayedState.Update;
        }

        private void UpdateAmount()
        {
            float deltaTime = Time.deltaTime;

            float amount = Mathf.Lerp(_CurrentPoint, _TargetPoint, deltaTime * _Speed);

            if (amount.SafeEquals(_TargetPoint, 0.01f))
            {
                _State = EDelayedState.None;

                _CurrentPoint = _TargetPoint;
            }
            else
            {
                _CurrentPoint = amount;
            }

            _Fill.fillAmount = _CurrentPoint;
        }

        
    }
}