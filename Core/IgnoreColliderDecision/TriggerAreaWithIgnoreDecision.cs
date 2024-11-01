using UnityEngine;

namespace StudioScor.Utilities
{
    public class TriggerAreaWithIgnoreDecision : TriggerAreaComponent
    {
        [Header(" [ Trigger Area With Ignore Decision Component ] ")]
        [SerializeField] private IgnoreColliderDecision[] _ignoreDecisions;

        protected override bool CanTriggerEnter(Collider other)
        {
            if (!base.CanTriggerEnter(other))
                return false;

            if (_ignoreDecisions is null || _ignoreDecisions.Length == 0)
                return true;

            foreach (var ignoreDecision in _ignoreDecisions)
            {
                if (!ignoreDecision.Decision(other))
                    return false;
            }

            return true;
        }
    }
}