using UnityEngine;

namespace StudioScor.StatusSystem
{
    [CreateAssetMenu(menuName ="Status/New Status Cost", fileName = "Cost_")]
    public class StatusCost : ScriptableObject
    {
        [SerializeField] private StatusTag _StatusTag;
        [SerializeField] private EConsumeType _ConsumeType;
        [SerializeField] private float _ConsumePoint;
        [SerializeField, Range(0f, 100f)] private float _ConsumeRatioPoint;

        public EConsumeType ConsumeType => _ConsumeType; 
        public float ConsumePoint => _ConsumePoint;
        public float ConsumeRatioPoint => _ConsumeRatioPoint;

        public StatusCostSpec CreateSpec(StatusSystem statusSystem)
        {
            var spec = new StatusCostSpec(this, statusSystem, _StatusTag);

            return spec;
        }
    }
}

