using StudioScor.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class TraceTask : Task, ISubTask
    {
        [Header(" [ Trace Action Task ] ")]
        [SerializeField][Range(0f, 1f)] private float _startTime = 0.4f;
        [SerializeField][Range(0f, 1f)] private float _endTime = 0.6f;
        [SerializeField] private Variable_LayerMask _traceLayer;
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IPositionVariable _tracePosition = new WorldPositionVariable();
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IFloatVariable _traceRadius = new DefaultFloatVariable(1f);

        [Header(" Hit Target Action ")]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITaskActionDecorator[] _hitDecorators;


#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITaskAction[] _onHittingActionTasks;


#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITaskAction[] _onHittingSelfActionTasks;


#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITaskAction[] _onHitSelfActionTasks;


        [Header(" Use Debug ")]
        [SerializeField] private bool _useDebug = false;

        private RaycastHit[] _hitResults = new RaycastHit[10];
        private List<Transform> _ignoreTransforms = new();
        private float _start;
        private float _end;
        private float _radius;
        private LayerMask _layers;
        private Vector3 _prevPosition;
        private bool _debug = false;


        public bool IsFixedUpdate => true;

        private TraceTask _original = null;

        private bool wasTrace = false;

        protected override void SetupTask()
        {
            base.SetupTask();

            _tracePosition.Setup(Owner);
            _traceRadius.Setup(Owner);

            _onHittingActionTasks.Setup(Owner);
            if (_onHittingActionTasks is not null && _onHittingActionTasks.Length > 0)
            {
            }


            _onHittingSelfActionTasks.Setup(Owner);
            if (_onHittingSelfActionTasks is not null && _onHittingSelfActionTasks.Length > 0)
            {
            }


            _onHitSelfActionTasks.Setup(Owner);
            if (_onHitSelfActionTasks is not null && _onHitSelfActionTasks.Length > 0)
            {
            }

            _hitDecorators.Setup(Owner);
        }


        public override ITask Clone()
        {
            var clone = new TraceTask();

            clone._original = this;
            clone._tracePosition = _tracePosition.Clone();
            clone._traceRadius = _traceRadius.Clone();

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
            _radius = hasOriginal ? _original._traceRadius.GetValue() : _traceRadius.GetValue();
            _layers = hasOriginal ? _original._traceLayer.Value : _traceLayer.Value;

            _debug = hasOriginal ? _original._useDebug : _useDebug;

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
                    EndTask();
                }
            }
        }
        private void OnTrace()
        {
            if (wasTrace)
                return;

            wasTrace = true;
            _ignoreTransforms.Add(Owner.transform);

            _prevPosition = _tracePosition.GetValue();
        }
        private void EndTrace()
        {
            if (!wasTrace)
                return;

            wasTrace = false;

            _ignoreTransforms.Clear();
        }

        private void UpdateTrace()
        {
            if (!wasTrace)
                return;

            Vector3 startPosition = _prevPosition;
            Vector3 endPosition = _tracePosition.GetValue();

            _prevPosition = endPosition;

            int hitCount = SUtility.Physics.DrawSphereCastAllNonAlloc(startPosition, endPosition, _radius, _hitResults, _layers, useDebug:_debug);
            bool isHit = false;

            for (int i = 0; i < hitCount; i++)
            {
                var hitResult = _hitResults[i];

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
                            actionTask.Action(actor.gameObject);
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
