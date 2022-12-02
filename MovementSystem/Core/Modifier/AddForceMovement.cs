using UnityEngine;

namespace StudioScor.MovementSystem
{
    public abstract class AddForceMovement : MovementModifier
    {
        public abstract float Mass { get; }
        public abstract float Drag { get; }

        public abstract bool IsGrounded { get; }

        private Vector3 _Force;

        private bool _IsRemainForce = false;
        public bool IsRemainForce => _IsRemainForce;

        public Vector3 Force => _Force;

        private bool _WasGrounded = false;

        public void OverrideForce(Vector3 newForce)
        {
            _Force = newForce / Mass;

            _IsRemainForce = true;

            if (_Force.y > 0)
            {
                _WasGrounded = false;
            }
        }
        public void AddForce(Vector3 addforce)
        {
            _Force += addforce / Mass;

            _IsRemainForce = true;

            if (_Force.y > 0)
            {
                _WasGrounded = false;
            }
        }
        public override void ResetVelocity()
        {
            _Force = Vector3.zero;

            _IsRemainForce = false;
        }

        public override Vector3 OnMovement(float deltaTime)
        {
            if (!_IsRemainForce)
            {
                return Vector3.zero;
            }

            if (_WasGrounded && IsGrounded  && _Force.y > 0)
            {
                _Force.y = 0;
            }

            _WasGrounded = IsGrounded;

            float power = _Force.magnitude;

            if (power < 0.2f)
            {
                ResetVelocity();

                return Vector3.zero;
            }

            Vector3 velocity = _Force;

            _Force = Vector3.MoveTowards(_Force, Vector3.zero, deltaTime * Drag);

            return velocity;
        }
    }
}