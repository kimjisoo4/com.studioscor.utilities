using UnityEngine;

namespace StudioScor.Utilities
{
    public static class RelationshipUtilities
    {
        public static IRelationshipSystem GetRelationshipSystem(this GameObject gameObject)
        {
            return gameObject.GetComponent<IRelationshipSystem>();
        }
        public static IRelationshipSystem GetRelationshipSystem(this Component component)
        {
            return component.GetRelationshipSystem();
        }

        public static bool TryGetReleationshipSystem(this GameObject gameObject, out IRelationshipSystem relationshipSystem)
        {
            return gameObject.TryGetComponent(out relationshipSystem);
        }
        public static bool TryGetReleationshipSystem(this Component component, out IRelationshipSystem relationshipSystem)
        {
            return component.gameObject.TryGetReleationshipSystem(out relationshipSystem);
        }

        public static ERelationship CheckRelationship(this TeamData lhs, IRelationshipSystem rhs)
        {
            return lhs.CheckRelationship(rhs.Team);
        }
        public static ERelationship CheckRelationship(this IRelationshipSystem lhs, TeamData rhs)
        {
            return lhs.Team.CheckRelationship(rhs);
        }
        public static ERelationship CheckRelationship(this IRelationshipSystem lhs, IRelationshipSystem rhs)
        {
            return lhs.Team.CheckRelationship(rhs.Team);
        }
    }
}