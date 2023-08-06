using UnityEngine;
namespace StudioScor.Utilities
{
    [System.Serializable]
    public class Projectile
    {
        [Header(" [ Projectile ] ")]
        [SerializeField]private Transform projectileTarget;

        [Header(" [ Direction ] ")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 direction = Vector3.forward;
        [SerializeField] private float turnSpeed = 180f;

        [Header(" [ Speed ] ")]
        [SerializeField] private float startedSpeed = 10f;
        [SerializeField] private float targetSpeed = 5f;
        [Space(5f)]
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float deceleration = 10f;

        [Header(" [ Gravity ] ")]
        [SerializeField] private bool useGravity = true;
        [SerializeField][SCondition(nameof(useGravity))] private float gravity = 9.81f;

        
        private bool isPlaying;
        private float currentSpeed;
        private float currentGravity;

        private Vector3 moveVelocity;
        private Vector3 turnRotation;


        public Transform ProjectileTarget => projectileTarget;
        public Vector3 Direction => direction;
        public bool IsPlaying => isPlaying;
        public bool UseGravity => useGravity;
        public Transform Target => target;
        public bool HasTarget => target;

        public float CurrentSpeed => currentSpeed;
        public float CurrentGravity => currentGravity;

        public Vector3 MoveVelocity => moveVelocity;
        public Vector3 TurnRotation => turnRotation;

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
            this.target = target;
        }

        public void SetDirection(Vector3 direction)
        {
            this.direction = direction;
        }

        public void OnProjectile()
        {
            if (isPlaying)
                return;

            isPlaying = true;

            currentSpeed = startedSpeed;
            currentGravity = 0f;
        }
        public void OnProjectile(Transform target)
        {
            if (isPlaying)
                return;

            SetTarget(target);

            OnProjectile();
        }
        public void OnProjectile(Vector3 direction)
        {
            if (isPlaying)
                return;

            SetDirection(direction);

            OnProjectile();
        }

        public void EndProjectile()
        {
            if (!isPlaying)
                return;

            isPlaying = false;

            currentSpeed = 0f;
            currentGravity = 0f;

            target = null;
        }
        public void UpdateProjectile(float deltaTime)
        {
            if (!isPlaying)
                return;

            UpdateRotation(deltaTime);
            UpdateMovement(deltaTime);
        }

        private void UpdateRotation(float deltaTime)
        {
            if (!target)
                return;
            
            Vector3 direction = projectileTarget.Direction(target);

            Vector3 rotation = projectileTarget.eulerAngles;
            Vector3 newRotation = Quaternion.LookRotation(direction).eulerAngles;

            float angle = Mathf.MoveTowardsAngle(rotation.y, newRotation.y, deltaTime * turnSpeed);

            rotation.y = angle;

            turnRotation = rotation;
        }
        

        private void UpdateMovement(float deltaTime)
        {
            Vector3 direction = this.direction;

            if (currentSpeed < targetSpeed)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * deltaTime);
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, deceleration * deltaTime);
            }

            Vector3 velocity = currentSpeed * direction;

            if (useGravity)
            {
                currentGravity -= this.gravity * deltaTime;

                velocity += currentGravity * Vector3.up;
            }

            moveVelocity = velocity;
        }
    }
}