using UnityEngine;

namespace StudioScor.Utilities
{
    public static class ActorUtility
    {
        public static IActor GetActor(this GameObject target)
        {
            var reroute = target.GetComponent<IRerouteActor>();

            return reroute is null ? null : reroute.Actor;
        }
        public static IActor GetActor(this Component component)
        {
            return GetActor(component.gameObject);
        }
        public static bool TryGetActor(this GameObject target, out IActor actor)
        {
            actor = target.GetActor();

            return actor is not null;
        }
        public static bool TryGetActor(this Component target, out IActor actor)
        {
            actor = target.gameObject.GetActor();

            return actor is not null;
        }
    }
}