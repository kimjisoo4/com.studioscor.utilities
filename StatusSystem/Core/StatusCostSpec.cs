namespace StudioScor.StatusSystem
{
    [System.Serializable]
    public class StatusCostSpec : StatusCostBase
    {
        private StatusCost _StatusCost;
        public override EConsumeType ConsumeType => _StatusCost.ConsumeType;
        public override float ConsumePoint => _StatusCost.ConsumePoint;
        public override float ConsumeRatioPoint => _StatusCost.ConsumeRatioPoint;

        private StatusSystem _StatusSystem;
        public StatusSystem StatusSystem => _StatusSystem;

        public StatusCostSpec(StatusCost statusCostSO, StatusSystem statusSystem, StatusTag statusTag) : base(statusSystem, statusTag)
        {
            _StatusCost = statusCostSO;
            _StatusSystem = statusSystem;
        }
    }
}

