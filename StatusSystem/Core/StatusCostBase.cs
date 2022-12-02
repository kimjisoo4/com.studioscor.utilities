using UnityEngine;

namespace StudioScor.StatusSystem
{
    [System.Serializable]
    public struct FStatusCost
    {
        public StatusTag Tag;
        public EConsumeType Type;
        public float Point;
        public float RatioPoint;
    }

    [System.Serializable]
    public abstract class StatusCostBase
    {
        private Status _Status;
        public abstract EConsumeType ConsumeType { get; }
        public abstract float ConsumePoint { get; }
        public abstract float ConsumeRatioPoint { get; }

        public Status Status => _Status;

        public StatusCostBase(StatusSystem statusSystem, StatusTag statusTag)
        {
            _Status = statusSystem.GetOrCreateValue(statusTag);
        }


        public void ConsumeCost()
        {
            if (_Status == null)
                return;

            _Status.DamagePoint(CurrentConsumeCost());
        }

        public bool CanConsumeCost()
        {
            if (_Status == null)
                return false;

            float consumePoint = CurrentConsumeCost();

            if (consumePoint < 0)
                return false;

            return _Status.CurrentPoint >= consumePoint;
        }

        public float CurrentConsumeCost()
        {
            if (_Status == null)
                return -1f;

            switch (ConsumeType)
            {
                case EConsumeType.Absolute:
                    return ConsumePoint;
                case EConsumeType.RatioInMax:
                    return _Status.MaxPoint * ConsumeRatioPoint * 0.01f;
                case EConsumeType.RatioInCurret:
                    return _Status.CurrentPoint * ConsumeRatioPoint * 0.01f;
                default:
                    return -1f;
            }
        }
    }

}

