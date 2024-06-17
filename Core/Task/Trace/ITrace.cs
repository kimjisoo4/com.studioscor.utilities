using System;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public interface ITrace
    {
        public FTraceInfo TraceInfo { get; }
        public IReadOnlyCollection<RaycastHit> HitResults { get; }
        public GameObject Owner { get; }
        public void Setup(GameObject owner);
        public ITrace Clone();
        public void OnTrace();
        public int UpdateTrace();
        public void EndTrace();
    }

    [Serializable]
    public abstract class TraceVariable : ITrace
    {
        [Header(" [ Trace Variable ] ")]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITraceDecorator[] _traceDecorators;
        [SerializeField] protected int _resultSize = 10;
        [SerializeField] protected bool _useDebug = false;

        protected TraceVariable _original;

        public FTraceInfo TraceInfo => _traceInfo;
        public IReadOnlyCollection<RaycastHit> HitResults => _hitResults;

        protected FTraceInfo _traceInfo;
        protected RaycastHit[] _hitResults;

        protected int _size;
        protected bool _debug;
        public bool IsPlaying { get; private set; }
        public GameObject Owner { get; private set; }

        public ITrace Clone()
        {
            var clone = OnClone() as TraceVariable;

            clone._original = this;

            int decoratorCount = _traceDecorators.Length;
            clone._traceDecorators = new ITraceDecorator[decoratorCount];

            for (int i = 0; i < decoratorCount; i++)
            {
                clone._traceDecorators[i] = _traceDecorators[i].Clone();
            }

            return clone;
        }

        protected abstract ITrace OnClone();
        protected abstract RaycastHit CreateHitResult();
        public void OnTrace()
        {
            if (IsPlaying)
                return;

            IsPlaying = true;

            _size = _original is null ? _resultSize : _original._resultSize;
            _debug = _original is null ? _useDebug : _original._useDebug;

            if (_hitResults is null || _hitResults.Length != _size)
                _hitResults = new RaycastHit[_size];

            OnEnter();
        }
        public void EndTrace()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;

            OnExit();
        }
        public int UpdateTrace()
        {
            if (!IsPlaying)
                return -1;

            int hitCount = OnUpdate();

            if (hitCount > 0)
            {
                foreach (var decorator in _traceDecorators)
                {
                    hitCount = decorator.CheckCondition(hitCount, ref _hitResults);

                    if (hitCount == 0)
                    {
                        _hitResults[0] = CreateHitResult();
                        break;
                    }
                }
            }
            else
            {
                _hitResults[0] = CreateHitResult();
            }

            return hitCount;
        }



        public void Setup(GameObject owner)
        {
            Owner = owner;

            foreach (var decorator in _traceDecorators)
            {
                decorator.Setup(owner);
            }

            OnSetup();
        }

        protected virtual void OnSetup()
        {

        }
        protected virtual void OnEnter()
        {

        }
        protected abstract int OnUpdate();
        protected virtual void OnExit()
        {

        }
    }





}
