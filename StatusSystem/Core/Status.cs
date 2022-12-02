using UnityEngine;

namespace StudioScor.StatusSystem
{
    [System.Serializable]
    public class Status
    {
        #region Event
        public delegate void ChangedPointHandler(Status status, float currentPoint, float prevPoint);
        public delegate void ChangedPointStateHandler(Status status);
        #endregion

        [SerializeField] private string _Name;
        [SerializeField] private float _MaxPoint;
        [SerializeField] private float _CurrentPoint;

        private StatusTag _StatusTag = null;

        private float _NormalizedValue;
        private bool _IsFullPoint;
        private bool _IsEmptyPoint;
        public string Name => _Name;
        public float MaxPoint => _MaxPoint;
        public float CurrentPoint => _CurrentPoint;
        public float NormalizedValue => _NormalizedValue;

        public StatusTag StatusTag => _StatusTag;
        public bool IsFullPoint => _IsFullPoint;
        public bool IsEmptyPoint => _IsEmptyPoint;

        public event ChangedPointHandler OnChangedMaxPoint;
        public event ChangedPointHandler OnChangedPoint;

        public event ChangedPointStateHandler OnFullStatusPoint;
        public event ChangedPointStateHandler OnConsumeStatusPoint;
        public event ChangedPointStateHandler OnEmptyStatusPoint;


        public Status(StatusTag statusTag, float maxPoint, float currentPoint)
        {
            _Name = statusTag.StatusName;
            _StatusTag = statusTag;

            SetPoint(maxPoint, currentPoint);
        }

        public Status(string name, float maxPoint, float currentPoint)
        {
            _Name = name;

            SetPoint(maxPoint, currentPoint);
        }

        public void SetPoint(float maxPoint, float currentPoint)
        {
            SetMaxPoint(maxPoint);
            SetCurrentPoint(currentPoint);
        }

        public void SetMaxPoint(float maxPoint, bool useSetNormalizedPoint = false)
        {
            float prevMaxPoint = _MaxPoint;

            _MaxPoint = maxPoint;

            if (useSetNormalizedPoint && prevMaxPoint != _CurrentPoint)
            {
                OnChangedMaxPoint?.Invoke(this, _MaxPoint, prevMaxPoint);

                if (_MaxPoint > prevMaxPoint)
                {
                    SetCurrentPoint(_CurrentPoint * (_MaxPoint / prevMaxPoint));
                }
                else
                {
                    SetCurrentPoint(_CurrentPoint * (prevMaxPoint / _MaxPoint));
                }
            }
        }
        public void SetCurrentPoint(float currentPoint)
        {
            float prevPoint = _CurrentPoint;

            _CurrentPoint = currentPoint;

            _CurrentPoint = Mathf.Clamp(_CurrentPoint, 0, _MaxPoint);

            if (prevPoint != _CurrentPoint)
            {
                _NormalizedValue = _CurrentPoint / _MaxPoint;

                OnChangedPoint?.Invoke(this, _CurrentPoint, prevPoint);

                if (CurrentPoint >= MaxPoint)
                {
                    _IsFullPoint = true;

                    OnFullStatusPoint?.Invoke(this);
                }
                else if(CurrentPoint <= 0f)
                {
                    _IsEmptyPoint = true;

                    OnEmptyStatusPoint?.Invoke(this);
                }
                else if (prevPoint >= MaxPoint || prevPoint <= 0f)
                {
                    _IsFullPoint = false;
                    _IsEmptyPoint = false;

                    OnConsumeStatusPoint?.Invoke(this);
                }
            }
        }

        public void HealPoint(float heal)
        {
            float point = _CurrentPoint + heal;

            SetCurrentPoint(point);
        }
        public void DamagePoint(float damage)
        {
            float point = _CurrentPoint - damage;

            SetCurrentPoint(point);
        }

        public bool TryConsumeCost(EConsumeType consumeType, float value)
        {
            if (CanConsumeCost(consumeType, value))
            {
                ConsumeCost(consumeType, value);

                return true;
            }
            else
            {
                return false;
            }
        }

        public void ConsumeCost(EConsumeType consumeType, float value)
        {
            float point = _CurrentPoint - CurrentConsumeCost(consumeType, value);

            if (point < 0.1f)
            {
                point = 0f;
            }

            SetCurrentPoint(point);
        }

        public bool CanConsumeCost(EConsumeType consumeType, float value)
        {
            float consumePoint = CurrentConsumeCost(consumeType, value);

            if (consumePoint < 0)
                return false;

            return CurrentPoint >= consumePoint;
        }

        public float CurrentConsumeCost(EConsumeType consumeType, float value)
        {
            switch (consumeType)
            {
                case EConsumeType.Absolute:
                    return value;
                case EConsumeType.RatioInMax:
                    return MaxPoint * value * 0.01f;
                case EConsumeType.RatioInCurret:
                    return CurrentPoint * value * 0.01f;
                default:
                    return -1f;
            }
        }
    }
}