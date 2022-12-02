using UnityEngine;

namespace StudioScor.TraceSystem
{
    public class TraceResult : MonoBehaviour
    {
        [SerializeField] private SphereTraceBase _TraceComponent;
        public SphereTraceBase TraceComponent => _TraceComponent;

        private void Awake()
        {
            TraceComponent.OnHit += TraceComponent_OnHit;
        }
        private void TraceComponent_OnHit(TraceBase trace, RaycastHit hit)
        {
            Debug.Log(hit.transform);
        }
    }
}