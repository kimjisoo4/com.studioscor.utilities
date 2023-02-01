using UnityEngine;

namespace StudioScor.Utilities
{
    public static partial class Utility
    {
        // GetComponenet
        public static T GetComponentInParentOrChildren<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponentInParent<T>();
            
            if (component)
                return component;

            return gameObject.GetComponentInChildren<T>();
        }

        // Try GetCompoenent
        public static bool TryGetComponentInParentOrChildren<T>(this GameObject gameObject, out T componenet) where T : Component
        {
            componenet = gameObject.GetComponentInParentOrChildren<T>();

            return componenet is not null;
        }
    }
}
