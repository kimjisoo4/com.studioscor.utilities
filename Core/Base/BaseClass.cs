using UnityEngine;
using System.Diagnostics;

namespace StudioScor.Utilities
{
    public abstract class BaseClass
    {
        [field: Header(" Use Debug ")]
        [field: SerializeField] protected virtual bool UseDebug { get; private set; } = false;
        [HideInInspector] protected virtual Object Context { get; private set; } = null;


        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, string color = "gray")
        {
#if UNITY_EDITOR
            if (UseDebug)
                SUtility.Debug.Log($"{GetType().Name} [ {(Context ? Context.name : "Empty")} ] : {log}", Context);
#endif
        }

        [Conditional("UNITY_EDITOR")]
        protected virtual void LogError(object log, string color = "red")
        {
#if UNITY_EDITOR
            SUtility.Debug.LogError($"{GetType().Name} [ {(Context ? Context.name : "Empty")} ] : {log}", Context);
#endif
        }
    }
}