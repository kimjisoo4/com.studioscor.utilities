using UnityEngine;


namespace StudioScor.Utilities
{
    [System.Serializable]
    public struct FSphereTrace
    {
        public Vector3 Start;
        public Vector3 End;
        public float Radius;
        public LayerMask LayerMask;

        public FSphereTrace(Vector3 start, Vector3 end, float radius, LayerMask layerMask)
        {
            Start = start;
            End = end;
            Radius = radius;
            LayerMask = layerMask;
        }
    }
}
