using UnityEngine;

namespace StudioScor.RelationshipSystem
{
    public static class RelationshipUtilities
    {
        public static RelationshipSystemComponent GetRelationshipSystem(this GameObject gameObject)
        {
            return gameObject.GetComponent<RelationshipSystemComponent>();
        }
        public static RelationshipSystemComponent GetRelationshipSystem(this Component component)
        {
            return component.GetRelationshipSystem();
        }

        public static bool TryGetReleationshipSystem(this GameObject gameObject, out RelationshipSystemComponent relationshipSystem)
        {
            return gameObject.TryGetComponent(out relationshipSystem);
        }
        public static bool TryGetReleationshipSystem(this Component component, out RelationshipSystemComponent relationshipSystem)
        {
            return component.gameObject.TryGetReleationshipSystem(out relationshipSystem);
        }

        public static ERelationship CheckRelationship(this TeamData lhs, RelationshipSystemComponent rhs)
        {
            return lhs.CheckRelationship(rhs.Team);
        }
        public static ERelationship CheckRelationship(this RelationshipSystemComponent lhs, TeamData rhs)
        {
            return lhs.Team.CheckRelationship(rhs);
        }
        public static ERelationship CheckRelationship(this RelationshipSystemComponent lhs, RelationshipSystemComponent rhs)
        {
            return lhs.Team.CheckRelationship(rhs.Team);
        }


        public static bool IsHostile(this TeamData lhs, RelationshipSystemComponent rhs)
        {
            return lhs.CheckRelationship(rhs.Team) == ERelationship.Hostile;
        }
        public static bool IsHostile(this RelationshipSystemComponent lhs, TeamData rhs)
        {
            return lhs.Team.CheckRelationship(rhs) == ERelationship.Hostile;
        }
        public static bool IsHostile(this RelationshipSystemComponent lhs, RelationshipSystemComponent rhs)
        {
            return lhs.Team.CheckRelationship(rhs.Team) == ERelationship.Hostile;
        }
    }
}