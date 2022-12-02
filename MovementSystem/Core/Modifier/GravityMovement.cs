using UnityEngine;


namespace StudioScor.MovementSystem
{
    [System.Serializable]
    public abstract class GravityMovement : MovementModifier
    {
        public abstract bool IsGrounded { get; }
        public abstract float CurrentVeritcalSpeed { get; }
        public abstract float Gravity { get; }

        public override void ResetVelocity()
        {
            
        }
        public override Vector3 OnMovement(float deltaTime)
        {
            if (IsGrounded)
            {
                return Vector3.zero;
            }

            float verticalSpeed = CurrentVeritcalSpeed;

            if (verticalSpeed > 0)
            {
                verticalSpeed = 0;
            }

            verticalSpeed -= Gravity * deltaTime;

            Vector3 GravityVelocity = Vector3.up * verticalSpeed;


            return GravityVelocity;
        }
    }
}