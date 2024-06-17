using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class TaskActionWithHitTransform : TraceTaskAction
    {
        [Header(" [ Task Action With Hit Transform ] ")]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITaskAction[] _taskActions;

        public override void Setup(GameObject owner)
        {
            base.Setup(owner);

            _taskActions.Setup(owner);
        }
        public override ITraceTaskAction Clone()
        {
            var clone = new TaskActionWithHitTransform();

            clone._taskActions = _taskActions.CloneTask();

            return clone;
        }

        public override void Action(FTraceInfo traceInfo, RaycastHit hit)
        {
            var target = hit.transform.gameObject;

            for (int i = 0; i < _taskActions.Length; i++)
            {
                if (!target)
                    break;

                var task = _taskActions[i];

                task.Action(target);
            }
        }
    }
}