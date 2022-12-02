using UnityEngine;

namespace StudioScor.Utilities
{


    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [Header(" [ Reference ] ")]
        [SerializeField] private Rigidbody _RigidBody;

        [Header(" [ Direction ] ")]
        [SerializeField] private Transform _Target;
        [SerializeField] private Vector3 _Direction = Vector3.forward;

        [Header(" [ Speed ] ")]
        [SerializeField] private float _StartedSpeed = 10f;
        [SerializeField] private float _TargetSpeed = 5f;
        
        [Header(" [ Accel & Decel ] ")]
        [SerializeField] private float _Acceleration = 20f;
        [SerializeField] private float _Deceleration = 10f;

        [Header(" [ Use Gravity ] ")]
        [SerializeField] private bool _UseGravitu = true;
        [SerializeField] private float _Gravity = 9.81f;

        [SerializeField] private float _TurnSpeed = 180f;

        [Header(" [ Use Debug ] ")]
        [SerializeField] protected bool _UseDebug;

        private float _CurrentHorizontalSpeed;
        private float _CurrentVerticalSpeed;



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
            OnProjectile();
        }
        private void OnDisable()
        {
            _Target = null;
        }

        private void OnProjectile()
        {
            _CurrentHorizontalSpeed = _StartedSpeed;
            _CurrentVerticalSpeed = 0f;
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

            if (_UseGravitu)
                _CurrentVerticalSpeed -= _Gravity * deltaTime;

            OnMovement(direction * _CurrentHorizontalSpeed + Vector3.up * _CurrentVerticalSpeed);
        }

        private void FixedUpdate()
        {
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