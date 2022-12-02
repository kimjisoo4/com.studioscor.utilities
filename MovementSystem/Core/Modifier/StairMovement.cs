using UnityEngine;

namespace StudioScor.MovementSystem
{
    public abstract class StairMovement : MovementModifier
    {
        public abstract float Gravity { get; }
        public abstract float StairStrength { get; }
        public abstract bool IsGrounded { get; }

        public override Vector3 OnMovement(float deltaTime)
        {
            if (IsGrounded)
            {
                return Vector3.down * Gravity * StairStrength;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}