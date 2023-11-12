using UnityEngine;
using System.Diagnostics;

namespace StudioScor.Utilities
{

    public abstract class BaseMonoBehaviour : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header(" [ Use Debug ] ")]
        public bool UseDebug;
#else
        [HideInInspector] public bool UseDebug = false;
#endif

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, string color = "gray")
        {
#if UNITY_EDITOR
            if (UseDebug)
                SUtility.Debug.Log($"{GetType().Name} [{name}] : {log}", this, color);
#endif
        }

        [Conditional("UNITY_EDITOR")]
        protected virtual void LogError(object log, string color = "red")
        {
#if UNITY_EDITOR
            SUtility.Debug.LogError($"{GetType().Name} [{name}] : {log}", this, color);
#endif
        }
    }
}