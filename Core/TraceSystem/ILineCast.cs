using UnityEngine;


namespace StudioScor.Utilities
{
    public interface ILineCast : IRaycast
    {
        public float TraceDistance { get; set; }
        public Vector3 TraceDirection { get; set; }

        public Vector3 StartPosition { get; }
        public Vector3 EndPosition { get; }
    }
}
