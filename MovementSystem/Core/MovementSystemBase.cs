using UnityEngine;
using System.Diagnostics;

namespace StudioScor.MovementSystem
{
    public abstract class MovementSystemBase : MonoBehaviour
    {
        #region EventHandler
        public delegate void ChangedMovementHandler(MovementSystemBase movementSystem);
        #endregion

        [Header(" [ Ignore Input ] ")]
        [SerializeField] private bool _IgnoreInput = false;
        [Header(" [ Debug Mode ] ")]
        [SerializeField] protected bool _UseDebug = false;

        protected float _MoveStrength = 0f;
        protected float _CurrentSpeed = 0f;
        protected float _CurrentVerticalSpeed = 0f;
        protected Vector3 _MoveDirection = Vector3.zero;
        protected Vector3 _LastMoveDirection = Vector3.zero;
        protected Vector3 _Velocity = Vector3.zero;
        protected Vector3 _DeltaVelocity = Vector3.zero;
        protected Vector3 _LastVelocity = Vector3.zero;
        protected bool _IsGrounded = true;
        protected bool _IsMoving = false;

        private int _IgnoreInputStack = 0;
        private bool _WasSetup = false;

        public virtual bool IsGrounded => _IsGrounded;
        public virtual bool IsMoving => _IsMoving;
        public virtual float MoveStrength => _MoveStrength;
        public virtual float CurrentSpeed => _CurrentSpeed;
        public virtual float CurrentVerticalSpeed => _CurrentVerticalSpeed;
        public virtual Vector3 MoveDirection => _MoveDirection;
        public virtual Vector3 LastMoveDirection => _LastMoveDirection;
        public virtual Vector3 Velocity => _Velocity;
        public virtual Vector3 DeltaVelocity => _DeltaVelocity;
        public virtual Vector3 LastVelocity => _LastVelocity;
        public bool IgnoreInput => _IgnoreInput;

        #region events
        public event ChangedMovementHandler OnLanded;
        public event ChangedMovementHandler OnJumped;
        public event ChangedMovementHandler OnStartedMovement;
        public event ChangedMovementHandler OnFinishedMovement;
        #endregion


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!_UseDebug)
                return;

            DrawGizmos();
        }

        [Conditional("UNITY_EDITOR")]
        protected virtual void DrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, MoveDirection * 1f);
            
            Gizmos.color = Color.gray;
            Gizmos.DrawRay(transform.position, LastMoveDirection * 1f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, LastVelocity);
        }
#endif

        private void Awake()
        {
            OnSetup();
        }

        private void OnSetup() 
        {
            if (_WasSetup)
                return;

            _WasSetup = true;

            Setup();
        }
        protected virtual void Setup() { }

        public void SetIgnoreInput(bool ignoreInput)
        {
            if (_IgnoreInput == ignoreInput)
                return;

            _IgnoreInput = ignoreInput;
        }

        public void AddIgnoreInput()
        {
            _IgnoreInputStack++;

            SetIgnoreInput(_IgnoreInputStack != 0);
        }
        public void RemoveIgnoreInput()
        {
            _IgnoreInputStack--;

            SetIgnoreInput(_IgnoreInputStack != 0);
        }
        public void ClearIngnoreInput()
        {
            _IgnoreInputStack = 0;

            SetIgnoreInput(false);
        }

        public void SetMovementInput(Vector3 direction, float strength = -1f)
        {
            if(IgnoreInput)
            {
                _MoveDirection = default;
                _MoveStrength = 0;

                return;
            }

            if (direction == Vector3.zero)
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
        public abstract void AddPosition(Vector3 velocity);
        public abstract void SetPosition(Vector3 position);
        public virtual void ResetMovement()
        {
            OnSetup();

            ClearIngnoreInput();
            ResetVelocity();

            _MoveStrength = 0f;
            _CurrentSpeed = 0f;
            _CurrentVerticalSpeed = 0f;
            _MoveDirection = Vector3.zero;
            _LastMoveDirection = Vector3.zero;

            PropertyUpdate();
        }

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

        public void AddVelocity(Vector3 velocity)
        {
            _Velocity += velocity;
        }

        public void AddDeltaVelocity(Vector3 velocity)
        {
            _DeltaVelocity += velocity;
        }

        protected void ResetVelocity()
        {
            _Velocity = Vector3.zero;
            _DeltaVelocity = Vector3.zero;
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
            if (_UseDebug)
                UnityEngine.Debug.Log("MovementSystem [" + gameObject.name + "] : " + log);
        }
        #endregion


        #region EventCallBack

        protected virtual void OnLand()
        {
            if (_UseDebug)
                Log("Landed");

            OnLanded?.Invoke(this);
        }
        protected virtual void OnJump()
        {
            if (_UseDebug)
                Log("Jumped");

            OnJumped?.Invoke(this);
        }

        protected virtual void OnStartMovement()
        {
            if (_UseDebug)
                Log("Started Movement");

            OnStartedMovement?.Invoke(this);
        }
        protected virtual void OnFinishMovement()
        {
            if (_UseDebug)
                Log("Finished Movement");

            OnFinishedMovement?.Invoke(this);
        }
#endregion

        
    }
}