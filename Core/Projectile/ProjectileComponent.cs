using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class ProjectileComponent : BaseMonoBehaviour, IProjectile
    {
        [System.Serializable]
        public class UnityEvents
        {
            [SerializeField]private UnityEvent _onStartedProjectile;
            [SerializeField]private UnityEvent _onEndedProjectile;

            public void AddUnityEvent(IProjectile projectile)
            {
                projectile.OnStartedProjectile += Projectile_OnStartedProjectile;
                projectile.OnEndedProjectile += Projectile_OnEndedProjectile;
            }
            public void RemoveUnityEvent(IProjectile projectile)
            {
                projectile.OnStartedProjectile -= Projectile_OnStartedProjectile;
                projectile.OnEndedProjectile -= Projectile_OnEndedProjectile;
            }
            

            private void Projectile_OnStartedProjectile(IProjectile projectile)
            {
                _onStartedProjectile?.Invoke();
            }
            private void Projectile_OnEndedProjectile(IProjectile projectile)
            {
                _onEndedProjectile?.Invoke();
            }
        }

        [Header(" [ Projectile ] ")]
        [SerializeField] private Projectile _projectile;

        [Header(" Unity Events ")]
        [SerializeField] private bool _useUnityEvent = true;
        [SerializeField][SCondition(nameof(_useUnityEvent))] private UnityEvents _unityEvents;

        public event IProjectile.ProjectileStateHandler OnStartedProjectile { add => _projectile.OnStartedProjectile += value; remove => _projectile.OnStartedProjectile -= value; }
        public event IProjectile.ProjectileStateHandler OnEndedProjectile { add => _projectile.OnEndedProjectile += value; remove => _projectile.OnEndedProjectile -= value; }

        public bool IsPlaying => _projectile.IsPlaying;
        public float StartSpeed { get => _projectile.StartSpeed; set => _projectile.StartSpeed = value; }
        public float TargetSpeed { get => _projectile.TargetSpeed; set => _projectile.TargetSpeed = value; }
        public float Acceleration { get => _projectile.Acceleration; set => _projectile.Acceleration = value; }
        public float Deceleration { get => _projectile.Deceleration; set => _projectile.Deceleration = value; }
        public Vector3 Direction { get => _projectile.Direction; set => _projectile.Direction = value; }
        public Transform Target { get => _projectile.Target; set => _projectile.Target = value; }
        public float CurrentSpeed { get => _projectile.CurrentSpeed; set => _projectile.CurrentSpeed = value; }
        public bool UseTurn { get => _projectile.UseTurn; set => _projectile.UseTurn = value; }
        public float TurnSpeed { get => _projectile.TurnSpeed; set => _projectile.TurnSpeed = value; }
        public bool UseGravity { get => _projectile.UseGravity; set => _projectile.UseGravity = value; }
        public float GravityScale { get => _projectile.GravityScale; set => _projectile.GravityScale = value; }

        public Vector3 Velocity => _projectile.Velocity;
        public Transform Actor { get => _projectile.Actor; set => _projectile.Actor = value; }

        private bool _wasInitialized = false;

        private void Reset()
        {
            EditorSetup();
        }
        private void OnValidate()
        {
            EditorSetup();
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void EditorSetup()
        {
#if UNITY_EDITOR
            if (_projectile is not null && !_projectile.Actor)
            {
                _projectile.Actor = transform;
            }
#endif
        }

        private void Awake()
        {
            Initialization();
        }
        private void OnDestroy()
        {
            if (_useUnityEvent)
            {
                _unityEvents.RemoveUnityEvent(this);
            }
        }

        private void Initialization()
        {
            if (_wasInitialized)
                return;

            _wasInitialized = true;

            if (_useUnityEvent)
            {
                _unityEvents.AddUnityEvent(this);
            }
        }

        [ContextMenu(nameof(OnProjectile))]
        public void OnProjectile()
        {
            Initialization();

            _projectile.OnProjectile();
        }

        [ContextMenu(nameof(EndProjectile))]
        public void EndProjectile()
        {
            _projectile.EndProjectile();
        }

        

        public (Vector3 velocity, Quaternion rotation) UpdateProjectile(float deltaTime)
        {
            return _projectile.UpdateProjectile(deltaTime);
        }
    }
}