using UnityEngine;

namespace StudioScor.BodySystem
{
    [System.Serializable]
    public class BodyPoint
    {
        public Transform Transform;
        public Vector3 PrevPosition;

        public BodyPoint(Transform transform)
        {
            Transform = transform;
            PrevPosition = Vector3.zero;
        }
    }

}
