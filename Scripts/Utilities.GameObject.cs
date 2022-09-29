using UnityEngine;

namespace KimScor.Utilities
{
    public static partial class Utilities
    {
        #region GameObject
        public static T GetComponentInParentOrChildren<T>(this GameObject gameObject) where T : class
        {
            var component = gameObject.GetComponentInParent<T>();
            if (component != null)
                return component;

            return gameObject.GetComponentInChildren<T>();
        }

        public static bool TryGetComponentInParentOrChildren<T>(this GameObject gameObject, out T componenet) where T : class
        {
            componenet = gameObject.GetComponentInParentOrChildren<T>();

            return componenet != null;
        }
        #endregion
    }
}
