using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IActor
    {
        public GameObject gameObject { get; }
        public Transform transform { get; }
    }
}