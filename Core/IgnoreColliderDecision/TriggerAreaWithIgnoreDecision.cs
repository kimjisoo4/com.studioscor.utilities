using UnityEngine;

namespace StudioScor.Utilities
{
    public class TriggerAreaWithIgnoreDecision : TriggerAreaComponent
    {
        [Header(" [ Trigger Area With Ignore Decision Component ] ")]
        [SerializeField] private IgnoreColliderDecision[] _ignoreDecisions;

        protected override bool CanTrigger(Collider other)
        {
            if (!base.CanTrigger(other))
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