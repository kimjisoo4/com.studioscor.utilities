using UnityEngine;

using System.Diagnostics;

#if UNITY_EDITOR
#endif

namespace StudioScor.Utilities
{
    public abstract class BaseClass
    {
        [field: Header(" [ Use Debug ] ")]
        public virtual bool UseDebug { get; private set; } = false;
        [HideInInspector] public virtual Object Context { get; private set; } = null;

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, bool isError = false, string color = "red")
        {
#if UNITY_EDITOR
            if (isError)
            {
                SUtility.Debug.LogError($"{GetType().Name} [ {(Context? Context.name : "Empty")} ] : {log}", Context);

                return;
            }
            
            if (UseDebug)
                SUtility.Debug.Log($"{GetType().Name} [ {(Context ? Context.name : "Empty")} ] : {log}", Context);
#endif
        }
    }
}