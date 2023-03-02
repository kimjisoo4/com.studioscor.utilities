using UnityEngine;
using UnityEngine.Events;

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
        [Header(" [ Auto Playing ] ")]
        [SerializeField] private bool _AutoShooting = true;
        
        public UnityEvent OnShotProjectile;


        private bool _WasShot;
        private float _CurrentHorizontalSpeed;
        private float _CurrentVerticalSpeed;

        public bool WasShot => _WasShot;



#if UNITY_EDITOR
        private void Reset()
        {
            if(TryGetComponent(out _RigidBody))
            {
                _RigidBody.useGravity = false;
            }
        }
#endif

        private void OnEnable()
        {
            if(_AutoShooting)
                OnProjectile();
        }
        private void OnDisable()
        {
            ResetProjectile();
        }

        public void OnProjectile()
        {
            if (_WasShot)
                return;

            _WasShot = true;

            _CurrentHorizontalSpeed = _StartedSpeed;
            _CurrentVerticalSpeed = 0f;

            OnShotProjectile?.Invoke();
        }
        public void ResetProjectile()
        {
            _WasShot = false;

            _CurrentHorizontalSpeed = 0f;
            _CurrentVerticalSpeed = 0f;

            _RigidBody.velocity = Vector3.zero;

            _Target = null;
        }

        public void SetTarget(Transform target)
        {
            _Target = target;
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
            Vector3 direction = transform.TransformDirection(_Direction);

            if (_CurrentHorizontalSpeed < _TargetSpeed)
            {
                _CurrentHorizontalSpeed = Mathf.MoveTowards(_CurrentHorizontalSpeed, _TargetSpeed, _Acceleration * deltaTime);
            }
            else
            {
                _CurrentHorizontalSpeed = Mathf.MoveTowards(_CurrentHorizontalSpeed, _TargetSpeed, _Deceleration * deltaTime);
            }

            if (_UseGravity)
                _CurrentVerticalSpeed -= _Gravity * deltaTime;

            OnMovement(direction * _CurrentHorizontalSpeed + Vector3.up * _CurrentVerticalSpeed);
        }

        private void FixedUpdate()
        {
            if (!_WasShot)
                return;

            float deltaTime = Time.fixedDeltaTime;

            LookAtTarget(deltaTime);

            Movement(deltaTime);
        }

        protected void OnMovement(Vector3 velocity)
        {
            _RigidBody.velocity = velocity;
        }
    }
}