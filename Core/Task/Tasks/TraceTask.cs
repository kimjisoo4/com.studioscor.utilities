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
        [SerializeReference]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReferenceDropdown]
#endif
        private IPositionVariable _tracePosition = new WorldPositionVariable();
        [SerializeReference]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReferenceDropdown]
#endif
        private IFloatVariable _traceRadius = new DefaultFloatVariable(1f);

        [Header(" Hit Target Action ")]
        [SerializeReference]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReferenceDropdown]
#endif
        private ITaskAction[] _onHitActionTasks;

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

        private bool wasTrigger = false;

        protected override void SetupTask()
        {
            base.SetupTask();

            _tracePosition.Setup(Owner);
            _traceRadius.Setup(Owner);

            foreach (var actionTask in _onHitActionTasks)
            {
                actionTask.Setup(Owner);
            }
        }


        public override ITask Clone()
        {
            var clone = new TraceTask();

            clone._original = this;
            clone._tracePosition = _tracePosition.Clone();
            clone._traceRadius = _traceRadius.Clone();

            clone._onHitActionTasks = new ITaskAction[_onHitActionTasks.Length];

            for(int i = 0; i < _onHitActionTasks.Length; i++)
            {
                clone._onHitActionTasks[i] = _onHitActionTasks[i].Clone();
            }

            return clone;
        }
        

        protected override void EnterTask()
        {
            base.EnterTask();

            wasTrigger = false;

            bool hasOriginal = _original is not null;

            _start = hasOriginal ? _original._startTime : _startTime;
            _end = hasOriginal ? _original._endTime : _endTime;
            _radius = hasOriginal ? _original._traceRadius.GetValue() : _traceRadius.GetValue();
            _layers = hasOriginal ? _original._traceLayer.Value : _traceLayer.Value;

            _debug = hasOriginal ? _original._useDebug : _useDebug;

        }
        public void UpdateSubTask(float normalizedTime)
        {
            if (!IsPlaying)
                return;

            if(!wasTrigger)
            {
                if(_start <= normalizedTime)
                {
                    OnTrace();
                }
            }
            else
            {
                UpdateTrace();

                if(_end <= normalizedTime)
                {
                    EndTrace();
                }
            }
        }
        private void OnTrace()
        {
            if (wasTrigger)
                return;

            wasTrigger = true;
            _ignoreTransforms.Add(Owner.transform);

            _prevPosition = _tracePosition.GetValue();
        }
        private void EndTrace()
        {
            if (!wasTrigger)
                return;

            wasTrigger = false;

            _ignoreTransforms.Clear();
        }

        private void UpdateTrace()
        {
            if (!wasTrigger)
                return;

            Vector3 startPosition = _prevPosition;
            Vector3 endPosition = _tracePosition.GetValue();

            _prevPosition = endPosition;

            int hitCount = SUtility.Physics.DrawSphereCastAllNonAlloc(startPosition, endPosition, _radius, _hitResults, _layers, useDebug:_debug);

            for (int i = 0; i < hitCount; i++)
            {
                var hitResult = _hitResults[i];

                var actor = hitResult.rigidbody ? hitResult.rigidbody.transform : hitResult.transform;

                if (!_ignoreTransforms.Contains(actor))
                {
                    _ignoreTransforms.Add(hitResult.transform);

                    foreach (var actionTask in _onHitActionTasks)
                    {
                        actionTask.Action(actor.gameObject);
                    }
                }
            }
        }
    }
}
