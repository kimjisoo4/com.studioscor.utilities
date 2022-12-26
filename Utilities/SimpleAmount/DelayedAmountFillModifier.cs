using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    public class DelayedAmountFillModifier : SimpleAmountModifier
    {
        [Header(" [ Delayed Amount Fill Modifier ] ")]
        [SerializeField] private Image _Fill;
        [Space(5f)]
        [SerializeField] private float _Delay = 0.2f;
        [SerializeField] private float _Speed = 5f;

        private float _RemainTime;
        private bool _IsUpdate;
        private bool _IsDelay;
        private float _CurrentPoint;
        private float _TargetPoint;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            gameObject.TryGetComponentInParentOrChildren(out _Fill);
        }
#endif

        public override void UpdateValue()
        {
            if (SimpleAmount.CurrentValue >= SimpleAmount.PrevValue)
            {
                _TargetPoint = SimpleAmount.NormalizedValue;

                return;
            }

            if (!_IsDelay && !_IsUpdate)
            {
                _CurrentPoint = SimpleAmount.PrevValue / SimpleAmount.MaxValue;
            }
            else if (!_IsDelay && _IsUpdate)
            {
                _CurrentPoint = _TargetPoint;
            }

            _RemainTime = _Delay;

            _IsUpdate = true;
            _IsDelay = true;

            _Fill.fillAmount = _CurrentPoint;
            _TargetPoint = SimpleAmount.NormalizedValue;
        }

        private void LateUpdate()
        {
            if (!_IsUpdate && !_IsDelay)
                return;

            float deltaTime = Time.deltaTime;

            if (_IsDelay)
            {
                _RemainTime -= deltaTime;

                if (_RemainTime <= 0f)
                {
                    _IsDelay = false;
                }

                return;
            }

            if (_IsUpdate)
            {
                _CurrentPoint = Mathf.Lerp(_CurrentPoint, _TargetPoint, deltaTime * _Speed);

                float interval = _CurrentPoint - _TargetPoint;
                
                if (interval.InRange(-0.01f, 0.01f))
                {
                    _IsUpdate = false;

                    _CurrentPoint = _TargetPoint;
                }

                _Fill.fillAmount = _CurrentPoint;
            }
        }

        
    }
}