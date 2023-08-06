using UnityEngine;

namespace StudioScor.Utilities
{
    public abstract class IgnoreColliderDecision : BaseScriptableObject
    {
        public abstract bool Decision(Collider other);
    }
}