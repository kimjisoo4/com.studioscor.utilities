using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StudioScor.Utilities
{

    [Serializable]
    public class TraceTask : Task, ISubTask
    {
        [Header(" [ Trace Action Task ] ")]
        [SerializeField][Range(0f, 1f)] private float _startTime = 0.4f;
        [SerializeField][Range(0f, 1f)] private float _endTime = 0.6f;
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITrace _trace = new SphereCast();

        [Header(" Hit Target Action ")]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITaskActionDecorator[] _hitDecorators;


#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITraceTaskAction[] _onHittingActionTasks;


#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITaskAction[] _onHittingSelfActionTasks;


#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITaskAction[] _onHitSelfActionTasks;

        private List<Transform> _ignoreTransforms = new();
        private float _start;
        private float _end;
        public bool IsFixedUpdate => true;

        private TraceTask _original = null;

        private bool wasTrace = false;

        protected override void SetupTask()
        {
            base.SetupTask();

            _trace.Setup(Owner);

            _onHittingActionTasks.Setup(Owner);
            _onHittingSelfActionTasks.Setup(Owner);
            _onHitSelfActionTasks.Setup(Owner);

            _hitDecorators.Setup(Owner);
        }


        public override ITask Clone()
        {
            var clone = new TraceTask();

            clone._original = this;
            clone._trace = _trace.Clone();

            clone._onHitSelfActionTasks = _onHitSelfActionTasks.CloneTask();
            clone._onHittingActionTasks = _onHittingActionTasks.CloneTask();
            clone._onHittingSelfActionTasks = _onHittingSelfActionTasks.CloneTask();
            clone._hitDecorators = _hitDecorators.CloneTask();

            return clone;
        }
        

        protected override void EnterTask()
        {
            base.EnterTask();

            wasTrace = false;

            bool hasOriginal = _original is not null;

            _start = hasOriginal ? _original._startTime : _startTime;
            _end = hasOriginal ? _original._endTime : _endTime;
        }
        protected override void ExitTask()
        {
            base.ExitTask();

            EndTrace();
        }

        public void UpdateSubTask(float deltaTime, float normalizedTime)
        {
            return;
        }
        public void FixedUpdateSubTask(float deltaTime, float normalizedTime)
        {
            if (!IsPlaying)
                return;

            if(!wasTrace)
            {
                if(_start <= normalizedTime)
                {
                    OnTrace();

                    UpdateTrace();
                }
            }
            else
            {
                UpdateTrace();

                if(_end <= normalizedTime)
                {
                    ComplateTask();
                }
            }
        }
        private void OnTrace()
        {
            if (wasTrace)
                return;

            wasTrace = true;
            _ignoreTransforms.Add(Owner.transform);

            _trace.OnTrace();
        }
        private void EndTrace()
        {
            if (!wasTrace)
                return;

            wasTrace = false;
            _ignoreTransforms.Clear();

            _trace.EndTrace();
        }

        private void UpdateTrace()
        {
            if (!wasTrace)
                return;

            int hitCount = _trace.UpdateTrace();
            bool isHit = false;

            var traceInfo = _trace.TraceInfo;

            for (int i = 0; i < hitCount; i++)
            {
                var hitResult = _trace.HitResults.ElementAt(i);

                var actor = hitResult.rigidbody ? hitResult.rigidbody.transform : hitResult.transform;

                if (!_ignoreTransforms.Contains(actor))
                {
                    _ignoreTransforms.Add(hitResult.transform);

                    if (!_hitDecorators.CheckAllCondition(actor.gameObject))
                        continue;

                    isHit = true;

                    if(_onHittingActionTasks is not null && _onHittingActionTasks.Length > 0)
                    {
                        foreach (var actionTask in _onHittingActionTasks)
                        {
                            actionTask.Action(traceInfo, hitResult);
                        }
                    }

                    if (_onHittingSelfActionTasks is not null && _onHittingSelfActionTasks.Length > 0)
                    {
                        foreach (var actionTask in _onHittingSelfActionTasks)
                        {
                            actionTask.Action(Owner);
                        }
                    }
                }
            }

            if (isHit)
            {
                if (_onHitSelfActionTasks is not null && _onHitSelfActionTasks.Length > 0 )
                {
                    foreach (var actionTask in _onHitSelfActionTasks)
                    {
                        actionTask.Action(Owner);
                    }
                }
            }
        }
    }
}
