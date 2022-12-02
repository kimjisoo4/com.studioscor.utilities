using UnityEngine;

namespace StudioScor.MovementSystem
{
    public abstract class SampleMovementSystem : MovementSystemBase
    {
        [Header("[Property]")]
        [SerializeField] protected float _MoveSpeed = 5f;
        [SerializeField] protected float _AcceateSpeed = 10f;
        [SerializeField] protected float _DecelerateSpeed = 10f;
        [SerializeField] protected float _Gravity = 9.81f;
        [SerializeField] protected float _Mass = 5f;
        [SerializeField] protected float _Drag = 1f;
        public virtual float MoveSpeed => _MoveSpeed;
        public virtual float AccelateSpeed => _AcceateSpeed;
        public virtual float DecelerateSpeed => _DecelerateSpeed;
        public virtual float Gravity => _Gravity;
        public virtual float Mass => _Mass;
        public virtual float Drag => _Drag;

        [Header("[Activate Movement]")]
        [SerializeField] protected bool _UseMovement = true;
        [SerializeField] protected bool _UseSideStep = true;
        [SerializeField] protected bool _UseGravity = true;
        [SerializeField] protected bool _UseAddForce = true;
        [SerializeField] protected bool _UseTargetMovement = true;

        public bool UseMovement => _UseMovement;
        public bool UseSideStep => _UseSideStep;
        public bool UseGravity => _UseGravity;
        public bool UseAddForce => _UseAddForce;
        public bool UseTargetMovement => _UseTargetMovement;
        public bool RemainAddForce => _AddForceMovement.IsRemainForce;

        public Vector3 CurrentAddForce => _AddForceMovement.Force;

        private DirectionalModifier _DirectionMovement;
        private ForwardModifier _ForwardMovement;
        private GravityModifier _GravityMovement;
        private TargetMovement _TargetMovement;
        private AddForceModifier _AddForceMovement;
        public bool GetUsingTargetMovement => _TargetMovement.IsActivate;
        private bool _GrantedAddUpForce = false;

        public event TargetMovement.TargetMovementHandler OnFinishedTargetMovement;

        protected override void Setup()
        {
            _DirectionMovement = new DirectionalModifier(this);
            _ForwardMovement = new ForwardModifier(this);
            _GravityMovement = new GravityModifier(this);
            _TargetMovement = new TargetMovement();
            _AddForceMovement = new(this);

            _TargetMovement.OnFinishedMovement += OnFinishedTargetMovement;
        }

        #region Setter
        public void SetMoveSpeed(float newMoveSpeed)
        {
            _MoveSpeed = newMoveSpeed;
        }
        public void SetAccelateSpeed(float newAccelateSpeed)
        {
            _AcceateSpeed = newAccelateSpeed;
        }
        public void SetDecelerateSpeed(float newDecelerateSpeed)
        {
            _DecelerateSpeed = newDecelerateSpeed;
        }
        public void SetGravity(float newGravity)
        {
            _Gravity = newGravity;
        }
        public void SetUseMovement(bool useDirection,bool resetVelocity = false)
        {
            if (_UseMovement == useDirection)
                return;

            _UseMovement = useDirection;

            if (resetVelocity)
                _DirectionMovement.ResetVelocity();
                _ForwardMovement.ResetVelocity();
        }
        public void SetUseSideStep(bool useSideStep)
        {
            if (_UseSideStep == useSideStep)
                return;

            _UseSideStep = useSideStep;

            if (!_UseSideStep)
            {
                _ForwardMovement.SetCurrentSpeed(_DirectionMovement.CurrentSpeed);
            }
            else
            {
                _DirectionMovement.SetCurrentSpeed(_ForwardMovement.CurrentSpeed);
            }
        }
        public void SetUseGravity(bool useGraity)
        {
            _UseGravity = useGraity;

            if(!UseGravity)
                _GravityMovement.ResetVelocity();
        }
        public void SetUseTargetMovement(bool useTargetMovement)
        {
            _UseTargetMovement = useTargetMovement;
        }
        public void SetTargetMovement(float duration, FTargetMovement targetMovement, float angle = 0f)
        {
            _TargetMovement.SetTargetMovement(duration, targetMovement, angle);
        }
        public void SetTargetMovement(float duration, FSingleTargetMovement singleTargetMovement, float angle = 0f)
        {
            _TargetMovement.SetTargetMovement(duration, singleTargetMovement, angle);
        }   
        public void SetTargetMovementAngle(float angle)
        {
            _TargetMovement.SetAngle(angle);
        }
        public void ResetTargetMovement()
        {
            _TargetMovement.StopTargetMovement();
        }

        public void SetUseAddForce(bool useAddForce)
        {
            _UseAddForce = useAddForce;

            if (!_UseAddForce)
                _AddForceMovement.ResetVelocity();
        }
        public void SetAddForce(Vector3 addforce, bool useOverride = true)
        {
            if (addforce.y > 0)
            {
                _GrantedAddUpForce = true;
            }

            if (useOverride)
            {
                _AddForceMovement.OverrideForce(addforce);
            }
            else
            {
                _AddForceMovement.AddForce(addforce);
            }
        }
        public void ResetAddForceMovement()
        {
            _AddForceMovement.ResetVelocity();
        }

        public override void ResetMovement()
        {
            base.ResetMovement();

            _AddForceMovement.ResetVelocity();
            _DirectionMovement.ResetVelocity();
            _ForwardMovement.ResetVelocity();
            _GravityMovement.ResetVelocity();
            _TargetMovement.ResetVelocity();
        }
        #endregion

        protected override void MovementUpdate(float deltaTime)
        {
            if (_GrantedAddUpForce)
            {
                _GrantedAddUpForce = false;

                SetGrounded(false);
            }

            if (UseSideStep)
            {
                OnDirectionMovement(deltaTime);
            }
            else
            {
                OnForwardMovement(deltaTime);
            }

            OnGravityMovement(deltaTime);
            OnTargetMovement(deltaTime);
            OnAddForceMovement(deltaTime);
        }
        public void OnDirectionMovement(float deltaTime)
        {
            if (!_UseMovement)
            {
                _DirectionMovement.OnMovement(deltaTime);

                return;
            }


            _Velocity += _DirectionMovement.OnMovement(deltaTime);

        }
        public void OnForwardMovement(float deltaTime)
        {
            if (!_UseMovement)
            {
                _ForwardMovement.OnMovement(deltaTime);

                return;
            }
                

            _Velocity += _ForwardMovement.OnMovement(deltaTime);
        }
        public void OnGravityMovement(float deltaTime)
        {
            if (!_UseGravity)
                return;

            _Velocity += _GravityMovement.OnMovement(deltaTime);
        }

        public void OnTargetMovement(float deltaTime)
        {
            if (!_TargetMovement.IsActivate)
                return;

            _DeltaVelocity += _TargetMovement.OnMovement(deltaTime);
        }
        public void OnAddForceMovement(float deltaTime)
        {
            if (!_UseAddForce)
                return;

            _Velocity += _AddForceMovement.OnMovement(deltaTime);
        }

        #region Movement Modifier Class

        protected class AddForceModifier : AddForceMovement
        {
            public SampleMovementSystem MovementSystem;

            public AddForceModifier(SampleMovementSystem movementSystem)
            {
                MovementSystem = movementSystem;
            }
            public override float Mass => MovementSystem.Mass;

            public override float Drag => MovementSystem.Drag;

            public override bool IsGrounded => MovementSystem.IsGrounded;
        }
        protected class GravityModifier : GravityMovement
        {
            public SampleMovementSystem MovementSystem;

            public GravityModifier(SampleMovementSystem movementSystem)
            {
                MovementSystem = movementSystem;
            }

            public void SetMovementSystem(SampleMovementSystem movementSystem)
            {
                MovementSystem = movementSystem;
            }

            public override bool IsGrounded => MovementSystem.IsGrounded;

            public override float CurrentVeritcalSpeed => MovementSystem.CurrentVerticalSpeed;

            public override float Gravity => MovementSystem.Gravity;
        }

        protected class DirectionalModifier : DirectionMovement
        {
            public SampleMovementSystem MovementSystem;

            public DirectionalModifier(SampleMovementSystem movementSystem)
            {
                MovementSystem = movementSystem;
            }

            public void SetMovementSystem(SampleMovementSystem movementSystem)
            {
                MovementSystem = movementSystem;
            }

            public override float MoveSpeed => MovementSystem.MoveSpeed;

            public override float Strength => MovementSystem.MoveStrength;

            public override float AccelateSpeed => MovementSystem.AccelateSpeed;

            public override Vector3 MoveDirection => MovementSystem.MoveDirection;

            public override Vector3 LastMoveDirection => MovementSystem.LastMoveDirection;

            public override float DecelerateSpeed => MovementSystem.DecelerateSpeed;
        }
        protected class ForwardModifier : ForwardMovement
        {
            public SampleMovementSystem MovementSystem;

            public ForwardModifier(SampleMovementSystem movementSystem)
            {
                MovementSystem = movementSystem;
            }

            public void SetMovementSystem(SampleMovementSystem movementSystem)
            {
                MovementSystem = movementSystem;
            }

            public override float MoveSpeed => MovementSystem.MoveSpeed;
            public override float Strength => MovementSystem.MoveStrength;
            public override float AccelateSpeed => MovementSystem.AccelateSpeed;
            public override float DecelerateSpeed => MovementSystem.DecelerateSpeed;
            public override Transform Transform => MovementSystem.transform;
        }

        #endregion
    }
}
