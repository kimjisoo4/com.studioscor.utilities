using UnityEngine;

namespace StudioScor.Utilities
{



    public static partial class SUtility
    {
        public static bool IsNullOrDestroyed(this System.Object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return true;

            if (obj is UnityEngine.Object)
                return (obj as UnityEngine.Object) == null;

            return false;
        }

        public static GameObject GetGameObject(this Collider collider)
        {
            if (!collider)
                return null;

            return collider.attachedRigidbody ? collider.attachedRigidbody.gameObject : collider.gameObject;
        }

        public static GameObject GetGameObjectByTypeInParentOrChildren<T>(this GameObject target)
        {
            var gameObject = target.GetGameObjectByTypeInParent<T>();

            if (gameObject)
                return gameObject;

            return target.GetGameObjectByTypeInChildren<T>();
        }
        public static GameObject GetGameObjectByTypeInChildren<T>(this GameObject target)
        {
            var targetClasses = target.GetComponentsInChildren<T>();

            foreach ( var targetClass in targetClasses )
            {
                if (targetClass is Component component)
                    return component.gameObject;
            }

            return null;
        }
        public static GameObject GetGameObjectByTypeInParent<T>(this GameObject target)
        {
            var targetClasses = target.GetComponentsInParent<T>();

            foreach (var targetClass in targetClasses)
            {
                if (targetClass is Component component)
                    return component.gameObject;
            }

            return null;
        }
        public static GameObject GetGameObjectByType<T>(this GameObject target)
        {
            var targetClass = target.GetComponent<T>();

            if (targetClass is Component component)
                return component.gameObject;

            return null;
        }
        public static bool TryGetGameObjectByType<T>(this GameObject target, out GameObject gameObject)
        {
            gameObject = target.GetGameObjectByType<T>();

            return gameObject;
        }
        public static bool TryGetGameObjectByTypeInParentOrChildren<T>(this GameObject target, out GameObject gameObject)
        {
            gameObject = target.GetGameObjectByTypeInParentOrChildren<T>();

            return gameObject;
        }
        public static bool TryGetGameObjectByTypeInParent<T>(this GameObject target, out GameObject gameObject)
        {
            gameObject = target.GetGameObjectByTypeInParent<T>();

            return gameObject;
        }
        public static bool TryGetGameObjectByTypeInChildren<T>(this GameObject target, out GameObject gameObject)
        {
            gameObject = target.GetGameObjectByTypeInChildren<T>();

            return gameObject;
        }



        // GetComponenet
        public static T GetComponentInParentOrChildren<T>(this GameObject gameObject)
        {
            if (!gameObject)
                return default;

            var component = gameObject.GetComponentInChildren<T>();
            
            if (component is not null)
                return component;

            return gameObject.GetComponentInParent<T>();
        }
        public static T GetComponentInChildrenOrParent<T>(this GameObject gameObject)
        {
            if (!gameObject)
                return default;

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
        public static bool TryGetComponentInChildrenOrParent<T>(this GameObject gameObject, out T component)
        {
            component = gameObject.GetComponentInChildrenOrParent<T>();

            return component is not null;
        }
        public static bool TryGetComponentInParentOrChildren<T>(this Component target, out T component)
        {
            return target.gameObject.TryGetComponentInParentOrChildren(out component);
        }
        public static bool TryGetComponentInChildrenOrParent<T>(this Component target, out T component)
        {
            return target.gameObject.TryGetComponentInChildrenOrParent(out component);
        }


        public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component)
        {
            component = gameObject.GetComponentInParent<T>();

            return component is not null;
        }
        public static bool TryGetComponentInParent<T>(this Component target, out T component)
        {
            return target.gameObject.TryGetComponentInParent(out component);
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
