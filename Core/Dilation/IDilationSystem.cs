using UnityEngine;


namespace StudioScor.Utilities
{
    public interface IDilationSystem
    {
        public delegate void DilationEventHandler(IDilationSystem dilation, float currentDilation, float prevDilation);
        public Transform transform { get; }
        public GameObject gameObject { get; }
        public float Speed { get; }
        public bool WasChangedSpeed { get; }
        public void ResetDilation();
        public void SetDilation(float newDilation);

        public event DilationEventHandler OnChangedDilation;
    }
}