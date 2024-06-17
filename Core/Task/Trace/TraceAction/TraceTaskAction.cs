using UnityEngine;

namespace StudioScor.Utilities
{
    public struct FTraceInfo
    {
        public Vector3 TraceStart { get; set; }
        public Vector3 TraceEnd { get; set;}

        public readonly Vector3 TraceDirection => TraceEnd - TraceStart;
        public readonly float TraceDistance => TraceDirection.magnitude;

        public FTraceInfo(Vector3 traceStart, Vector3 traceEnd)
        {
            TraceStart = traceStart;
            TraceEnd = traceEnd;
        }
    }

    public interface ITraceTaskAction
    {
        public GameObject Owner { get; }

        public ITraceTaskAction Clone();
        public void Setup(GameObject owner);
        public void Action(FTraceInfo traceInfo, RaycastHit hit);
    }

    public abstract class TraceTaskAction : ITraceTaskAction
    {
        public GameObject Owner { get; protected set; }

        public virtual void Setup(GameObject owner)
        {
            Owner = owner;
        }
        public abstract ITraceTaskAction Clone();
        public abstract void Action(FTraceInfo traceInfo, RaycastHit hit);
    }
}
