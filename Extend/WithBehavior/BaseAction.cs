#if SCOR_ENABLE_BEHAVIOR
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace StudioScor.Utilities.UnityBehavior
{

    public abstract class BaseAction : Action
    {
#if UNITY_EDITOR
        [SerializeReference] public BlackboardVariable<bool> UseDebugKey = new BlackboardVariable<bool>(false);
        protected bool UseDebug => UseDebugKey.Value;
#else
        protected bool UseDebug => false;
#endif

        protected override Status OnStart()
        {
            var status = base.OnStart();

            Log($"{nameof(OnStart)}");

            return status;
        }
        protected override void OnEnd()
        {
            base.OnEnd();

            Log($"{nameof(OnEnd)}");
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected void Log(object message, string color = SUtility.STRING_COLOR_DEFAULT)
        {
            if (!UseDebug)
                return;

            SUtility.Debug.Log($"{GetType().Name} [{GameObject.name}] : {message}", GameObject, color); 
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected void LogError(object message, string color = SUtility.STRING_COLOR_RED)
        {
            if (!UseDebug)
                return;

            SUtility.Debug.LogError($"{GetType().Name} [{GameObject.name}] : {message}", GameObject, color);
        }
    }
}
#endif