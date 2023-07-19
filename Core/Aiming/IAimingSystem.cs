using UnityEngine;


namespace StudioScor.Utilities
{
    public interface IAimingSystem
    {
        public void OnAiming();
        public void EndAiming();

        public bool IsPlaying { get; }
        public Vector3 AimPosition { get; }
        public ITargeting Target { get; }
    }
}