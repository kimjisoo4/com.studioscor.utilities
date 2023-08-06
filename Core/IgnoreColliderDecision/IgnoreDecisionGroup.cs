using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Ignore Collider Decision /new Group Decision", fileName = "CollisionDecision_Group_")]
    public class IgnoreDecisionGroup : IgnoreColliderDecision
    {
        [Header(" [ Members ] ")]
        [SerializeField] private IgnoreColliderDecision[] ignoreColliderDecisions;

        public override bool Decision(Collider other)
        {
            return CheckMemberDecisions(other);
        }

        protected bool CheckMemberDecisions(Collider other)
        {
            foreach (var decision in ignoreColliderDecisions)
            {
                if (!decision.Decision(other))
                    return false;
            }

            return true;
        }
    }
}