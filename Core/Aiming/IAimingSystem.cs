using UnityEngine;


namespace StudioScor.Utilities
{
    public interface IAimingSystem
    {
        public delegate void AimingSystemTargetEventHandler(IAimingSystem aimingSystem, ITargeting currentTarget, ITargeting prevTarget);
        public delegate void AimingSystemStateEventHandler(IAimingSystem aimingSystem);
        public GameObject gameObject { get; }
        public bool enabled { get; }
        public bool WasLocked { get; }
        public Vector3 AimPosition { get; }
        public ITargeting Target { get; }
        
        public void OnAiming();
        public void FixedTick(float deltaTime);
        public void EndAiming();

        public void SetLockState(bool isLock);

        public void AddIgnoreTarget(Transform target);
        public void RemoveIgnoreTarget(Transform target);

        public event AimingSystemTargetEventHandler OnChangedTarget;
        public event AimingSystemStateEventHandler OnChangedLockedState;
    }
}