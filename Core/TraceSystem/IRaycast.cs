using System.Collections.Generic;
using UnityEngine;


namespace StudioScor.Utilities
{
    public interface IRaycast
    {
        public delegate void RaycastStateHandler(IRaycast raycast);

        public GameObject Owner { get; }
        public bool IsPlaying { get; }
        public int MaxHitCount { get; set; }
        public LayerMask TraceLayer { get; set; }
        public int HitCount { get; }
        public IReadOnlyCollection<RaycastHit> HitResults { get; }
        public IReadOnlyList<Transform> IgnoreTransforms { get; }

        public void SetOwner(GameObject newOwner);
        public void OnTrace();
        public void EndTrace();

        public void AddIgnoreTransform(Transform transform);

        public (int hitCount, RaycastHit[] raycastHits) UpdateTrace();

        public event RaycastStateHandler OnStartedRaycast;
        public event RaycastStateHandler OnEndedRaycast;
    }
}
