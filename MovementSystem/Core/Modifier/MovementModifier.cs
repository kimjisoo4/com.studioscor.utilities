using UnityEngine;


namespace StudioScor.MovementSystem
{
    public abstract class MovementModifier
    {
        public abstract Vector3 OnMovement(float deltaTime);
        public abstract void ResetVelocity();
    }

}