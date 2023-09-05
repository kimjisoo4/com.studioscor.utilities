using UnityEngine;

namespace StudioScor.Utilities
{
    public class ProjectileComponent : BaseMonoBehaviour
    {
        [field: Header(" [ Projectile ] ")]
        [field: SerializeField] public Rigidbody RigidBody { get; protected set; }

        [field: Header(" Move")]
        [field: SerializeField] public AccelerateMove AccelerateMove { get; protected set; }

        [field: Header(" Gravity ")]
        [field: SerializeField] public bool UseGravity { get; protected set; } = false;
        [field: SerializeField] public GravityMove GravityMove { get; protected set; }

        [field: Header(" Direction ")]
        [field: SerializeField] public LookAtDirection LookAtDirection { get; protected set; }


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
            AccelerateMove.OnAccelerate();
            
            if(UseGravity)
                GravityMove.OnGravity();

            LookAtDirection.OnLookAtDirection();
        }
        public void EndProjectile()
        {
            AccelerateMove.EndAccelerate();

            if (UseGravity)
                GravityMove.EndGravity();

            LookAtDirection.EndLookAtDirection();
        }

        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            AccelerateMove.UpdateAccelerate(deltaTime);
            GravityMove.UpdateGravity(deltaTime);
            LookAtDirection.UpdateRotation(deltaTime);

            RigidBody.rotation = LookAtDirection.Rotation;
            RigidBody.velocity = transform.forward * AccelerateMove.Speed + GravityMove.Velocity;
        }
    }
}