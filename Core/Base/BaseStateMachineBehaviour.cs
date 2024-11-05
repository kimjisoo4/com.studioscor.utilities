using UnityEngine;

namespace StudioScor.Utilities
{
    public class BaseStateMachineBehaviour : StateMachineBehaviour
    {
#if UNITY_EDITOR
        [Header(" [ Base State Machine Behaviour ] ")]
        [Header(" Use Debug ")]
        public bool UseDebug = false;
#else
        public bool UseDebug => false;
#endif

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected void Log(object message, Animator context, string color = SUtility.STRING_COLOR_DEFAULT)
        {
#if UNITY_EDITOR
            if (!UseDebug)
                return;

            SUtility.Debug.Log($"{$"{GetType().Name} [{context.gameObject.name}]".ToBold()} : {message}", context.gameObject, color);
#endif
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected void LogError(object message, Animator context, string color = SUtility.STRING_COLOR_RED)
        {
#if UNITY_EDITOR
            if (!UseDebug)
                return;

            SUtility.Debug.LogError($"{$"{GetType().Name} [{context.gameObject.name}]".ToBold()} : {message}", context.gameObject, color);
#endif
        }
    }
}