using UnityEngine;
namespace StudioScor.Utilities
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : BaseMonoBehaviour
    {
        [Header(" [ Projectile ] ")]
        [SerializeField] private Rigidbody _RigidBody;
        [Header(" [ Direction ] ")]
        [SerializeField] private Transform _Target;
        [SerializeField] private Vector3 _Direction = Vector3.forward;
        [SerializeField] private float _TurnSpeed = 180f;
        [Header(" [ Speed ] ")]
        [SerializeField] private float _StartedSpeed = 10f;
        [SerializeField] private float _TargetSpeed = 5f;
        [Space(5f)]
        [SerializeField] private float _Acceleration = 20f;
        [SerializeField] private float _Deceleration = 10f;
        [Header(" [ Gravity ] ")]
        [SerializeField] private bool _UseGravity = true;
        [SerializeField][SCondition(nameof(_UseGravity))] private float _Gravity = 9.81f;
        [Header(" [ Play Speed ] ")]
        [SerializeField] private float _PlaySpeed = 1f;
        [Header(" [ Auto Playing ] ")]
        [SerializeField] private bool _AutoPlaying = true;

        private bool _IsPlaying;
        private float _CurrentSpeed;
        private float _CurrentGravity;


        public Vector3 Direction => _Direction;
        public bool IsPlaying => _IsPlaying;
        public bool UseGravity => _UseGravity;
        public Transform Target => _Target;
        public bool HasTarget => _Target;

        public float CurrentSpeed => _CurrentSpeed;
        public float CurrentGravity => _CurrentGravity;

        private void Reset()
        {
#if UNITY_EDITOR
            if(TryGetComponent(out _RigidBody))
            {
                _RigidBody.useGravity = false;
            }
#endif
        }

        private void OnEnable()
        {
            if (_AutoPlaying)
                OnProjectile();
        }
        private void OnDisable()
        {
            EndProjectile();
        }
        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime * _PlaySpeed;

            UpdateProjectile(deltaTime);
        }

        public void SetPlaySpeed(float newSpeed)
        {
            _PlaySpeed = newSpeed;
        }

        public void SetTarget(Component component)
        {
            SetTarget(component.transform);
        }
        public void SetTarget(GameObject gameObject)
        {
            SetTarget(gameObject.transform);
        }
        public void SetTarget(Transform target)
        {
            _Target = target;
        }

        public void SetDirection(Vector3 direction)
        {
            _Direction = direction;
        }

        public void OnProjectile()
        {
            if (_IsPlaying)
                return;

            _IsPlaying = true;

            _CurrentSpeed = _StartedSpeed;
            _CurrentGravity = 0f;
        }
        public void EndProjectile()
        {
            if (!_IsPlaying)
                return;

            _IsPlaying = false;

            _CurrentSpeed = 0f;
            _CurrentGravity = 0f;

            _RigidBody.velocity = Vector3.zero;

            _Target = null;
        }
        public void UpdateProjectile(float deltaTime)
        {
            if (!_IsPlaying)
                return;

            LookAtTarget(deltaTime);

            Movement(deltaTime);
        }

        private void LookAtTarget(float deltaTime)
        {
            if (_Target)
            {
                Vector3 direction = transform.Direction(_Target);

                Vector3 rotation = transform.eulerAngles;
                Vector3 newRotation = Quaternion.LookRotation(direction).eulerAngles;

                float angle = Mathf.MoveTowardsAngle(rotation.y, newRotation.y, deltaTime * _TurnSpeed);

                rotation.y = angle;

                transform.eulerAngles = rotation;
            }
        }

        private void Movement(float deltaTime)
        {
            Vector3 direction = _Direction;

            if (_CurrentSpeed < _TargetSpeed)
            {
                _CurrentSpeed = Mathf.MoveTowards(_CurrentSpeed, _TargetSpeed, _Acceleration * deltaTime);
            }
            else
            {
                _CurrentSpeed = Mathf.MoveTowards(_CurrentSpeed, _TargetSpeed, _Deceleration * deltaTime);
            }

            if (_UseGravity)
                _CurrentGravity -= _Gravity * deltaTime;

            Vector3 velocity = _PlaySpeed * _CurrentSpeed * direction;
            Vector3 gravity = _PlaySpeed * _CurrentGravity * Vector3.up;

            OnMovement(velocity + gravity);
        }

        protected void OnMovement(Vector3 velocity)
        {
            _RigidBody.velocity = velocity;
        }
    }
}