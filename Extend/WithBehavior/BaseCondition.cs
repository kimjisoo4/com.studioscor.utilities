#if SCOR_ENABLE_BEHAVIOR
using StudioScor.Utilities;
using Unity.Behavior;
using UnityEngine;

namespace StudioScor.PlayerSystem.Behavior
{
    public abstract class BaseCondition : Condition
    {
#if UNITY_EDITOR
        [SerializeReference] public BlackboardVariable<bool> UseDebugKey = new BlackboardVariable<bool>(false);
        protected bool UseDebug => UseDebugKey.Value;
#else
        protected bool UseDebug => false;
#endif
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected void Log(object message, string color = SUtility.STRING_COLOR_DEFAULT)
        {
            if (!UseDebug)
                return;

            SUtility.Debug.Log($"{$"{GetType().Name} [{GameObject.name}]".ToBold()} : {message}", GameObject, color);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected void LogError(object message, string color = SUtility.STRING_COLOR_RED)
        {
            if (!UseDebug)
                return;

            SUtility.Debug.LogError($"{$"{GetType().Name} [{GameObject.name}]".ToBold()} : {message}", GameObject, color);
        }

    }
}
#endif