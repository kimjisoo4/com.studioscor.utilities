using UnityEngine;

namespace StudioScor.MovementSystem
{
    public abstract class DirectionMovement : MovementModifier
    {
        public abstract float MoveSpeed { get; }
        public abstract float Strength { get; }
        public abstract float AccelateSpeed { get; }
        public abstract float DecelerateSpeed { get; }
        public abstract Vector3 MoveDirection { get; }
        public abstract Vector3 LastMoveDirection { get; }

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

            Vector3 direction = MoveDirection;

            float accelateSpeed;


            if (direction == Vector3.zero)
            {
                direction = LastMoveDirection;

                accelateSpeed = DecelerateSpeed;
            }
            else
            {
                if (currentSpeed > MoveSpeed)
                {
                    accelateSpeed = DecelerateSpeed;
                }
                else
                {
                    accelateSpeed = AccelateSpeed;
                }
            }


            _CurrentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelateSpeed * deltaTime);


            Vector3 MoveVelocity = direction * _CurrentSpeed;


            return MoveVelocity;
        }

    }
}