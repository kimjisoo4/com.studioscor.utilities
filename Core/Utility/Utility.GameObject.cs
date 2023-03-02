using UnityEngine;

namespace StudioScor.Utilities
{
    public static partial class SUtility
    {
        // GetComponenet
        public static T GetComponentInParentOrChildren<T>(this GameObject gameObject)
        {
            var component = gameObject.GetComponentInParent<T>();
            
            if (component is not null)
                return component;

            return gameObject.GetComponentInChildren<T>();
        }

        // Try GetCompoenent
        public static bool TryGetComponentInParentOrChildren<T>(this GameObject gameObject, out T componenet)
        {
            componenet = gameObject.GetComponentInParentOrChildren<T>();

            return componenet is not null;
        }
    }
}
