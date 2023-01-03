using UnityEngine;
using System.Diagnostics;

namespace StudioScor.Utilities
{
    public class BaseScriptableObject : ScriptableObject
    {
#if UNITY_EDITOR
        [Header(" [ Use Debug ] ")]
        [SerializeField] private bool _UseDebug = false;
        public bool UseDebug => _UseDebug;
#else
        public bool UseDebug = false;
#endif

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, bool isError = false)
        {
#if UNITY_EDITOR
            if (isError)
            {
                Utility.Debug.LogError(name + " [ " + GetType().Name + " ] : " + log, this);

                return;
            }

            if (UseDebug)
                Utility.Debug.Log(name + " [ " + GetType().Name + " ] : " + log, this);
#endif
        }
    }

    public abstract class BaseClass
    {
#if UNITY_EDITOR
        public abstract bool UseDebug { get; }
#else
        public bool UseDebug = false;
#endif

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, bool isError = false, Object context = null)
        {
#if UNITY_EDITOR
            if (isError)
            {
                Utility.Debug.LogError(context.name + " [ " + GetType().Name + " ] : " + log, context);

                return;
            }

            if (UseDebug)
                Utility.Debug.Log(context.name + " [ " + GetType().Name + " ] : " + log, context);
#endif
        }
    }

    public class BaseMonoBehaviour : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header(" [ Use Debug ] ")]
        [SerializeField] private bool _UseDebug = false;
        public bool UseDebug => _UseDebug;
#else
        public bool UseDebug => false;
#endif

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, bool isError = false)
        {
#if UNITY_EDITOR
            if (isError)
            {
                Utility.Debug.LogError(name + " [ " + GetType().Name + " ] : " + log, this);
                
                return;
            }

            if (UseDebug)
                Utility.Debug.Log(name + " [ " + GetType().Name + " ] : " + log, this);
#endif
        }
    }
}