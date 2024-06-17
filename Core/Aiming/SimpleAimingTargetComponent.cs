using UnityEngine;


namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/Aiming/Simple Aiming Target Component", order: 0)]
    public class SimpleAimingTargetComponent : BaseMonoBehaviour, ITargeting
    {
        public Transform Point => transform;
        public bool CanTargeting => enabled;
    }
}