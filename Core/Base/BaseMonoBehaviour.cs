using UnityEngine;
using System.Diagnostics;

namespace StudioScor.Utilities
{
    public abstract class BaseScriptableObject : ScriptableObject
    {
#if UNITY_EDITOR
        [Header(" [ Use Debug ] ")]
        public bool UseDebug;
#else
        [HideInInspector]public const bool UseDebug = false;
#endif

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, bool isError = false)
        {
#if UNITY_EDITOR
            if (isError)
            {
                Utility.Debug.LogError(GetType().Name + " [ " + name + " ] : " + log, this);

                return;
            }

            if (UseDebug)
                Utility.Debug.Log(GetType().Name + " [ " + name + " ] : " + log, this);
#endif
        }
    }

    public abstract class BaseClass
    {

#if UNITY_EDITOR
        [Header(" [ Use Debug ] ")]
        public bool UseDebug;
#else
        [HideInInspector] public const bool UseDebug = false;
#endif

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, bool isError = false, Object context = null)
        {
#if UNITY_EDITOR
            if (isError)
            {
                Utility.Debug.LogError(GetType().Name + " [ " + context.name + " ] : " + log, context);

                return;
            }

            if (UseDebug)
                Utility.Debug.Log(GetType().Name + " [ " + context.name + " ] : " + log, context);
#endif
        }
    }

    public abstract class BaseMonoBehaviour : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header(" [ Use Debug ] ")]
        public bool UseDebug;
#else
        [HideInInspector]public const bool UseDebug = false;
#endif

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, bool isError = false)
        {
#if UNITY_EDITOR
            if (isError)
            {
                Utility.Debug.LogError(GetType().Name + " [ " + name + " ] : " + log, this);
                
                return;
            }

            if (UseDebug)
                Utility.Debug.Log(GetType().Name + " [ " + name + " ] : " + log, this);
#endif
        }
    }
}