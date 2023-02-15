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
        [HideInInspector]public bool UseDebug = false;
#endif

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, bool isError = false)
        {
#if UNITY_EDITOR
            if (isError)
            {
                SUtility.Debug.LogError(GetType().Name + " [ " + name + " ] : " + log, this);

                return;
            }

            if (UseDebug)
                SUtility.Debug.Log(GetType().Name + " [ " + name + " ] : " + log, this);
#endif
        }
    }

    public abstract class BaseClass
    {

        [field: Header(" [ Use Debug ] ")]
        public virtual bool UseDebug { get; private set; } = false;
        [HideInInspector] public virtual Object Context { get; private set; } = null;

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, bool isError = false)
        {
#if UNITY_EDITOR
            if (isError)
            {
                SUtility.Debug.LogError(GetType().Name + " [ " + Context.name + " ] : " + log, Context);

                return;
            }
            
            if (UseDebug)
                SUtility.Debug.Log(GetType().Name + " [ " + Context.name + " ] : " + log, Context);
#endif
        }
    }

    public abstract class BaseMonoBehaviour : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header(" [ Use Debug ] ")]
        public bool UseDebug;
#else
        [HideInInspector]public bool UseDebug = false;
#endif

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, bool isError = false)
        {
#if UNITY_EDITOR
            if (isError)
            {
                SUtility.Debug.LogError(GetType().Name + " [ " + name + " ] : " + log, this);
                
                return;
            }

            if (UseDebug)
                SUtility.Debug.Log(GetType().Name + " [ " + name + " ] : " + log, this);
#endif
        }
    }
}