using UnityEngine;


namespace StudioScor.Utilities
{
    public interface IOverlapSphere : IRaycast
    {
        public bool UseInLocalSpace { get; set; }
        public Vector3 TracePosition { get; set; }
        public float TraceRadius { get; set; }
    }
}
