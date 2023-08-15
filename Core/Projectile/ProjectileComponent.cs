using UnityEngine;

namespace StudioScor.Utilities
{
    public class ProjectileComponent : BaseMonoBehaviour
    {
        [Header(" [ Projectile ] ")]
        [SerializeField] private Rigidbody rigidBody;

        [Header(" Move")]
        [SerializeField] private AccelerateMove accelerateMove;

        [Header(" Gravity ")]
        [SerializeField] private bool useGravity = false;
        [SerializeField] private GravityMove gravityMove;

        [Header(" Direction ")]
        [SerializeField] private LookAtDirection lookAtDirection;

        public AccelerateMove AccelerateMove => accelerateMove;
        public GravityMove GravityMove => gravityMove;  
        public LookAtDirection LookAtDirection => lookAtDirection;

        private void OnEnable()
        {
            OnProjectile();
        }
        private void OnDisable()
        {
            EndProjectile();
        }

        public void OnProjectile()
        {
            accelerateMove.OnAccelerate();
            
            if(useGravity)
                gravityMove.OnGravity();

            lookAtDirection.OnLookAtDirection();
        }
        public void EndProjectile()
        {
            accelerateMove.EndAccelerate();

            if (useGravity)
                gravityMove.EndGravity();

            lookAtDirection.EndLookAtDirection();
        }

        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            accelerateMove.UpdateAccelerate(deltaTime);
            gravityMove.UpdateGravity(deltaTime);
            lookAtDirection.UpdateRotation(deltaTime);

            rigidBody.rotation = lookAtDirection.EularAngles;
            rigidBody.velocity = transform.forward * accelerateMove.Speed + gravityMove.Velocity;
        }
    }
}