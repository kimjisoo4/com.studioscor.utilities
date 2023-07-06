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
        public static T GetComponentInParentOrChildren<T>(this Component component)
        {
            return component.gameObject.GetComponentInParentOrChildren<T>();
        }

        // Try GetCompoenent
        public static bool TryGetComponentInParentOrChildren<T>(this GameObject gameObject, out T component)
        {
            component = gameObject.GetComponentInParentOrChildren<T>();

            return component is not null;
        }
        public static bool TryGetComponentInParentOrChildren<T>(this Component target, out T component)
        {
            return target.gameObject.TryGetComponentInParentOrChildren(out component);
        }

        public static T GetComponentInRootOrParent<T>(this GameObject gameObject)
        {
            var component = gameObject.transform.root.GetComponent<T>();

            if (component is not null)
                return component;

            return gameObject.GetComponentInParent<T>();
        }
        public static T GetComponentInRootOrParent<T>(this Component component)
        {
            return component.gameObject.GetComponentInRootOrParent<T>();
        }

        public static bool TryGetComponentInRootOrParent<T>(this GameObject gameObject, out T component)
        {
            component = gameObject.GetComponentInRootOrParent<T>();

            return component is not null;
        }
        public static bool TryGetComponentInRootOrParent<T>(this Component target, out T component)
        {
            return target.gameObject.TryGetComponentInRootOrParent(out component);
        }


        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component)
        {
            component = gameObject.GetComponentInChildren<T>();

            return component is not null;
        }
        public static bool TryGetComponentInChildren<T>(this Component target, out T component)
        {
            return target.gameObject.TryGetComponentInChildren(out component);
        }
    }
}
