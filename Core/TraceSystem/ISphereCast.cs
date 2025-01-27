using UnityEngine;


namespace StudioScor.Utilities
{
    public interface ISphereCast : IRaycast
    {
        public float TraceRadius { get; set; }
        public Vector3 StartPosition { get; }
        public Vector3 EndPosition { get; }
    }
}
