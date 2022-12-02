using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.TraceSystem
{
    public class TraceEventReciver : MonoBehaviour
    {
        [SerializeField] private TraceBase _TraceComponent;

        public TraceBase TraceComponent => _TraceComponent;

        [Header("[ Use Unity Event ]")]
        public UnityEvent<RaycastHit> OnHit;
        public UnityEvent<Vector3> OnHit_Position;
        public UnityEvent<List<RaycastHit>> OnHits;
        public UnityEvent<RaycastHit> OnFirstHit;
        public UnityEvent<bool, List<Transform>> OnIsHit;

#if UNITY_EDITOR
        private void Reset()
        {
            TryGetComponent(out _TraceComponent);
        }
#endif

        private void Awake()
        {
            TraceComponent.OnHit += TraceComponent_OnHit;
            TraceComponent.OnHits += TraceComponent_OnHits;
            TraceComponent.OnFirstHit += TraceComponent_OnFirstHit;
            TraceComponent.OnIsHit += TraceComponent_OnIsHit;
        }

        private void TraceComponent_OnIsHit(TraceBase traceComponent, bool isHit, List<Transform> hitList = null)
        {
            OnIsHit?.Invoke(isHit, hitList);
            
        }

        private void TraceComponent_OnFirstHit(TraceBase traceComponent, RaycastHit hit)
        {
            OnFirstHit?.Invoke(hit);
        }

        private void TraceComponent_OnHits(TraceBase traceComponent, List<RaycastHit> hits)
        {
            OnHits?.Invoke(hits);
        }

        private void TraceComponent_OnHit(TraceBase traceComponent, RaycastHit hit)
        {
            OnHit?.Invoke(hit);
            OnHit_Position?.Invoke(hit.point);
        }
    }

}