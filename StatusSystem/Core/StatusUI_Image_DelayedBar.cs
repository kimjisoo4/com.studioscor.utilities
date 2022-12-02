using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.StatusSystem
{
    public class StatusUI_Image_DelayedBar : StatusUIModifier
    {
        [SerializeField] private Image _Image;
        [SerializeField] private float _Delay = 0.2f;
        [SerializeField] private float _Speed = 5f;

        private bool _IsUpdate = false;
        private bool _IsDelay = false;
        private float _RemainTime = 0f;
        private float _CurrentPoint = 1f;
        private float _TargetPoint = 1f;

        public override void StatusUpdate(Status status, float currentPoint, float prevPoint)
        {
            if (currentPoint >= prevPoint)
            {
                _TargetPoint = status.NormalizedValue;

                return;
            }

            if(!_IsDelay && !_IsUpdate)
            {
                _CurrentPoint = prevPoint / status.MaxPoint;
            }
            else if (!_IsDelay && _IsUpdate)
            {
                _CurrentPoint = _TargetPoint;
            }

            _RemainTime = _Delay;

            _IsUpdate = true;
            _IsDelay = true;

            _Image.fillAmount = _CurrentPoint;
            _TargetPoint = status.NormalizedValue;
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

                if (Mathf.Abs(_CurrentPoint - _TargetPoint) < 0.01f)
                {
                    _IsUpdate = false;

                    _CurrentPoint = _TargetPoint;
                }

                _Image.fillAmount = _CurrentPoint;
            }
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            _Image = GetComponent<Image>();
        }
#endif
    }
}