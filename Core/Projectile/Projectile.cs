using UnityEngine;
using static StudioScor.Utilities.IProjectile;

namespace StudioScor.Utilities
{
    public interface IProjectile
    {
        public delegate void ProjectileStateHandler(IProjectile projectile);

        public Transform Actor { get; set; }
        public bool IsPlaying { get; }

        public float StartSpeed { get; set; }
        public float TargetSpeed { get; set; }
        public float Acceleration { get; set; }
        public float Deceleration { get; set; }
        public Vector3 Direction { get; set; }
        public Transform Target { get; set; }
        public float CurrentSpeed { get; set; }
        public bool UseTurn { get; set; }
        public float TurnSpeed { get; set; }

        public bool UseGravity { get; set; }
        public float GravityScale { get; set; }

        public Vector3 Velocity { get; }
        public void OnProjectile();
        public void EndProjectile();

        public (Vector3 velocity, Quaternion rotation) UpdateProjectile(float deltaTime);

        public event ProjectileStateHandler OnStartedProjectile;
        public event ProjectileStateHandler OnEndedProjectile;
    }

    [System.Serializable]
    public class Projectile : BaseClass, IProjectile
    {
        [Header(" [ Projectile ] ")]
        [SerializeField] private Transform _actor;

        [Header(" Direction ")]
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _direction = Vector3.forward;

        [Header(" Movement ")]
        [SerializeField] private float _startSpeed = 10f;
        [SerializeField] private float _targetSpeed = 5f;
        [Space(5f)]
        [SerializeField] private float _acceleration = 20f;
        [SerializeField] private float _deceleration = 10f;

        [Header(" Gravity ")]
        [SerializeField] private bool _useGravity = false;
        [SerializeField] private float _gravityScale = 1f;

        [Header(" Rotation ")]
        [SerializeField] private bool _useTurn = true;
        [SerializeField] private float _turnSpeed = 180f;
        
        private bool _isPlaying;
        private float _currentSpeed;
        private Vector3 _velocity;
        private float _gravity = 0f;

        public bool IsPlaying => _isPlaying;

        public Transform Actor { get => _actor; set => _actor = value; }
        public override Object Context { get => Actor; }
        public float StartSpeed { get => _startSpeed; set => _startSpeed = value; }
        public float TargetSpeed { get =>_targetSpeed; set => _targetSpeed = value; }
        public float CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }
        public float Acceleration { get =>_acceleration; set => _acceleration = value; }
        public float Deceleration { get => _deceleration; set => _deceleration = value; }
        public bool UseTurn { get => _useTurn; set => _useTurn = value; }
        public float TurnSpeed { get => _turnSpeed; set => _turnSpeed = value; }
        public Vector3 Direction { get => _direction; set => _direction = value; }
        public Transform Target { get => _target; set => _target = value; }
        public bool UseGravity { get =>_useGravity; set => _useGravity = value; }
        public float GravityScale { get => _gravityScale; set => _gravityScale = value; }
        public Vector3 Velocity => _velocity;


        public event ProjectileStateHandler OnStartedProjectile;
        public event ProjectileStateHandler OnEndedProjectile;

        public void SetSpeed(float newSpeed)
        {
            _currentSpeed = newSpeed;
        }

        public void OnProjectile()
        {
            if (_isPlaying)
                return;

            _isPlaying = true;

            _currentSpeed = _startSpeed;
            _gravity = 0f;

            Invoke_OnStartedProjectile();
        }

        public void EndProjectile()
        {
            if (!_isPlaying)
                return;

            _isPlaying = false;

            _currentSpeed = 0f;
            _gravity = 0f;

            _target = null;

            Invoke_OnEndedProjectile();
        }
        public (Vector3 velocity, Quaternion rotation) UpdateProjectile(float deltaTime)
        {
            if (!_isPlaying)
                return (default, default);

            Vector3 velocity = UpdateMovement(deltaTime);
            Quaternion rotation = UpdateRotation(deltaTime);

            return (velocity, rotation);
        }

        private Quaternion UpdateRotation(float deltaTime)
        {
            if (!_useTurn)
                return _actor.rotation;

            Vector3 direction = _velocity.normalized;

            if (direction.SafeEquals(Vector3.zero))
                return _actor.rotation;

            Quaternion currentRotation = _actor.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            Quaternion newRotation = Quaternion.RotateTowards(currentRotation, targetRotation, deltaTime * _turnSpeed);

            return newRotation;
        }
        

        private Vector3 UpdateMovement(float deltaTime)
        {
            Vector3 direction = _target ? _actor.Direction(_target) : _actor.TransformDirection(_direction);

            if (_currentSpeed < _targetSpeed)
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, _targetSpeed, _acceleration * deltaTime);
            }
            else
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, _targetSpeed, _deceleration * deltaTime);
            }

            Vector3 velocity = direction * (_currentSpeed * deltaTime);

            if(_useGravity)
            {
                var gravityStrength = Physics.gravity.y * _gravityScale * deltaTime;
                _gravity += gravityStrength * deltaTime;

                velocity += Vector3.up * _gravity;
            }

            _velocity = velocity;

            return velocity;
        }

        private void Invoke_OnStartedProjectile()
        {
            Log($"{nameof(OnStartedProjectile)}");

            OnStartedProjectile?.Invoke(this);
        }
        private void Invoke_OnEndedProjectile()
        {
            Log($"{nameof(OnEndedProjectile)}");

            OnEndedProjectile?.Invoke(this);
        }
    }
}