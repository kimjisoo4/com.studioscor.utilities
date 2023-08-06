using UnityEngine;
using System.Linq;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Ignore Collider Decision /new Tag Decision", fileName = "CollisionDecision_Tag_")]
    public class IgnoreTagDecision : IgnoreColliderDecision
    {
        [Header(" [ Tags ] ")]
        [SerializeField][STagSelector] private string[] tags;
        [SerializeField] private bool isIgnoreTag = false;

        public override bool Decision(Collider other)
        {
            return CheckIgnoreTag(other);
        }

        protected bool CheckIgnoreTag(Collider other)
        {
            if (tags.Contains(other.tag))
                return !isIgnoreTag;
            else
                return isIgnoreTag;
        }
    }
}