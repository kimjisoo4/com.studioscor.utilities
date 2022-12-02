using UnityEngine;
using System.Diagnostics;

namespace StudioScor.Utilities
{
    public class BaseMonoBehaviour : MonoBehaviour
    {
        [Header(" [ Use Debug ] ")]
        [SerializeField] private bool _UseDebug = false;

        protected bool UseDebug => _UseDebug;

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