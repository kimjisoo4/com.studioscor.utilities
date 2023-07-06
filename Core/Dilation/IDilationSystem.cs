using UnityEngine;


namespace StudioScor.Utilities
{
    public delegate void DilationEventHandler(IDilationSystem dilation, float currentDilation, float prevDilation);
    public interface IDilationSystem
    {
        public Transform transform { get; }
        public GameObject gameObject { get; }
        public float Speed { get; }
        public void ResetDilation();
        public void SetDilation(float newDilation);

        public event DilationEventHandler OnChangedDilation;
    }
}