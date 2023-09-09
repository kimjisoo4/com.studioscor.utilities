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
        protected virtual void Log(object log, bool isError = false, string color = "gray")
        {
#if UNITY_EDITOR
            if (isError)
            {
                SUtility.Debug.LogError($"{GetType().Name} [{name}] : {log}", this, color);

                return;
            }

            if (UseDebug)
                SUtility.Debug.Log($"{GetType().Name} [{name}] : {log}", this, color);
#endif
        }
    }
}