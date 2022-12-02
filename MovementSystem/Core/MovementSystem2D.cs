using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

using StudioScor.MovementSystem;


namespace StudioScor.MovementSystem
{
    public abstract class MovementSystem2D : MonoBehaviour
    {
        #region EventHandler
        public delegate void ChangedMovementHandler(MovementSystem2D movementSystem);
        #endregion

        [Header("[Debug Mode]")]
        [SerializeField] protected bool DebugMode = false;

        protected float _MoveStrength = 0f;
        protected float _CurrentSpeed = 0f;
        protected float _CurrentVerticalSpeed = 0f;
        protected Vector2 _MoveDirection = Vector2.zero;
        protected Vector2 _LastMoveDirection = Vector2.zero;
        protected Vector2 _Velocity = Vector2.zero;
        protected Vector2 _DeltaVelocity = Vector2.zero;
        protected Vector2 _LastVelocity = Vector2.zero;
        protected bool _IsGrounded = true;
        protected bool _IsMoving = false;

        public virtual bool IsGrounded => _IsGrounded;
        public virtual bool IsMoving => _IsMoving;
        public virtual float MoveStrength => _MoveStrength;
        public virtual float CurrentSpeed => _CurrentSpeed;
        public virtual float CurrentVerticalSpeed => _CurrentVerticalSpeed;
        public virtual Vector2 MoveDirection => _MoveDirection;
        public virtual Vector2 LastMoveDirection => _LastMoveDirection;
        public virtual Vector2 Velocity => _Velocity;
        public virtual Vector2 DeltaVelocity => _DeltaVelocity;
        public virtual Vector2 LastVelocity => _LastVelocity;

        #region events
        public event ChangedMovementHandler OnLanded;
        public event ChangedMovementHandler OnJumped;
        public event ChangedMovementHandler OnStartedMovement;
        public event ChangedMovementHandler OnFinishedMovement;
        #endregion


        public void SetMoveDirection(Vector2 direction, float strength = -1f)
        {
            if (direction == Vector2.zero)
            {
                _MoveDirection = default;
                _MoveStrength = 0;
            }
            else
            {
                _MoveDirection = direction;
                _LastMoveDirection = _MoveDirection;

                if (strength < 0)
                {
                    _MoveStrength = 1;
                }
                else
                {
                    _MoveStrength = Mathf.Clamp01(strength);
                }
            }
        }

        public void OnMovement(float deltaTime)
        {
            SetGrounded(CheckOnGrounded());

            MovementUpdate(deltaTime);

            PropertyUpdate();
        }

        protected abstract bool CheckOnGrounded();
        protected abstract void MovementUpdate(float deltaTime);
        public abstract void SetMovePosition(Vector2 position);

        protected virtual void PropertyUpdate()
        {
            _LastVelocity = Velocity + DeltaVelocity;

            Vector3 currentVelocity = LastVelocity;

            float currentVerticalSpeed = currentVelocity.y;

            currentVelocity.y = 0f;

            float currentSpeed = currentVelocity.magnitude;


            if (IsMoving && currentSpeed == 0)
            {
                _IsMoving = false;

                OnFinishMovement();
            }
            else if (!IsMoving && currentSpeed > 0)
            {
                _IsMoving = true;

                OnStartMovement();
            }

            _CurrentSpeed = currentSpeed;
            _CurrentVerticalSpeed = currentVerticalSpeed;

            ResetVelocity();
        }

        public void AddVelocity(Vector2 velocity)
        {
            _Velocity += velocity;
        }

        public void AddDeltaVelocity(Vector2 velocity)
        {
            _DeltaVelocity += velocity;
        }

        public void ResetVelocity()
        {
            _Velocity = Vector2.zero;
            _DeltaVelocity = Vector2.zero;
        }

        public void SetGrounded(bool isGrounded)
        {
            if (_IsGrounded != isGrounded)
            {
                _IsGrounded = isGrounded;

                if (isGrounded)
                {
                    OnLand();
                }
                else
                {
                    OnJump();
                }
            }
        }

        #region EDITOR
        [Conditional("UNITY_EDITOR")]
        protected void Log(string log)
        {
            if (DebugMode)
                UnityEngine.Debug.Log("MovementSystem2D [" + gameObject.name + "] : " + log);
        }
        #endregion

        #region EventCallBack

        protected virtual void OnLand()
        {
            if (DebugMode)
                Log("Landed");

            OnLanded?.Invoke(this);
        }
        protected virtual void OnJump()
        {
            if (DebugMode)
                Log("Jumped");

            OnJumped?.Invoke(this);
        }

        protected virtual void OnStartMovement()
        {
            if (DebugMode)
                Log("Started Movement");

            OnStartedMovement?.Invoke(this);
        }
        protected virtual void OnFinishMovement()
        {
            if (DebugMode)
                Log("Finished Movement");

            OnFinishedMovement?.Invoke(this);
        }
        #endregion
    }

}
