using UnityEngine;


namespace StudioScor.Utilities
{
    public interface IAimingSystem
    {
        public delegate void AimingSystemEventHandler(IAimingSystem aimingSystem, Transform target);
        public void OnAiming();
        public void EndAiming();
        public bool IsPlaying { get; }
        public Vector3 AimPosition { get; }
        public ITargeting Target { get; }

        public void AddIgnoreTarget(Transform target);
        public void RemoveIgnoreTarget(Transform target);

        public event AimingSystemEventHandler OnChangedTarget;
    }
}