using UnityEngine;


namespace StudioScor.MovementSystem
{
    public abstract class ForwardMovement : MovementModifier
    {
        public abstract float MoveSpeed { get; }
        public abstract float Strength { get; }
        public abstract float AccelateSpeed { get; }
        public abstract float DecelerateSpeed { get; }
        public abstract Transform Transform { get; }
        public float CurrentSpeed => _CurrentSpeed;

        private float _CurrentSpeed = 0f;

        public void SetCurrentSpeed(float newCurrentSpeed)
        {
            _CurrentSpeed = newCurrentSpeed;
        }

        public override void ResetVelocity()
        {
            _CurrentSpeed = 0f;
        }

        public override Vector3 OnMovement(float deltaTime)
        {
            float targetSpeed = MoveSpeed * Strength;

            float currentSpeed = _CurrentSpeed;

            float accelateSpeed;

            if (Strength > 0)
            {
                accelateSpeed = AccelateSpeed;
            }
            else
            {
                accelateSpeed = DecelerateSpeed;
            }

            _CurrentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelateSpeed * deltaTime);

            Vector3 MoveVelocity = Transform.forward * _CurrentSpeed;

            return MoveVelocity;
        }
    }
}