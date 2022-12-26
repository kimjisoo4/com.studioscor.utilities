using UnityEngine;
using System.Diagnostics;
using StudioScor.Utilities;

namespace StudioScor.PlayerSystem
{
    public class ControllerSystem : BaseStateMono
    {
        #region Events
        public delegate void OnChangedPawnHandler(ControllerSystem controller, PawnSystem pawn);

        public delegate void InputHandler(ControllerSystem controller);

        public delegate void StartMovementInputHandler(ControllerSystem controller, Vector3 direction, float strength);
        public delegate void FinishMovementInputHandler(ControllerSystem controller, Vector3 prevDirection, float prevStrength);

        public delegate void StartRotateInputHander(ControllerSystem controller, Vector3 direction);
        public delegate void FinishRotateInputHander(ControllerSystem controller, Vector3 direction);

        public delegate void LookTargetHandler(ControllerSystem controllerSystem, Transform currentLookTarget, Transform prevLookTarget);
        #endregion

        [Header(" [ Controller System ] ")]
        [Header(" [ Controlled Pawn ] ")]
        [SerializeField] protected PawnSystem _Pawn;
        public PawnSystem Pawn => _Pawn;

        [Header(" [ Player Controller] ")]
        [SerializeField] protected bool _IsPlayerController = false;
        public bool IsPlayerController => _IsPlayerController;

        [Header(" [ Team ] ")]
        [SerializeField] protected EAffiliation _Affiliation = EAffiliation.Hostile;
        public EAffiliation Affiliation => _Affiliation;

        [Header(" [ Use Movement Input ] ")]
        [SerializeField] protected bool _UseMovementInput = true;
        public bool UseMovementInput => _UseMovementInput;

        [Header(" [ Use Turn Input ]")]
        [SerializeField] protected bool _UseTurnInput = true;
        public bool UseTurnInput => _UseTurnInput;

        [Header(" [ Use Look Input ] ")]
        [SerializeField] protected bool _UseLookInput = true;
        public bool UseLookInput => _UseLookInput;

        [Header(" [ Look Target ] ")]
        [SerializeField] protected Transform _LookTarget;
        public Transform LookTarget => _LookTarget;

        private Vector3 _MoveDirection = Vector3.zero;
        public Vector3 MoveDirection => _MoveDirection;

        private float _MoveStrength = 0f;
        public float MoveStrength => _MoveStrength;

        private Vector3 _TurnDirection = Vector3.zero;
        public Vector3 TurnDirection => _TurnDirection;

        private Vector3 _LookDirection = Vector3.zero;
        public Vector3 LookDirection => _LookDirection;

        public bool IsPossess => Pawn;

        public event OnChangedPawnHandler OnPossessedPawn;
        public event OnChangedPawnHandler UnPossessedPawn;

        public event InputHandler OnUsedMovementInput;
        public event InputHandler UnUsedMovementInput;

        public event InputHandler OnUsedTurnInput;
        public event InputHandler UnUsedTurnInput;

        public event InputHandler OnUsedLookInput;
        public event InputHandler UnUsedLookInput;

        public event StartMovementInputHandler OnStartedMovementInput;
        public event FinishMovementInputHandler OnFinishedMovementInput;

        public event StartRotateInputHander OnStartedTurntInput;
        public event FinishRotateInputHander OnFinishedTurnInput;

        public event StartRotateInputHander OnStartedLookInput;
        public event FinishRotateInputHander OnFinishedLookInput;

        public event LookTargetHandler OnChangedLookTarget;


        public void OnPossess(PawnSystem pawn)
        {
            if (Pawn == pawn)
                return;

            UnPossess(Pawn);

            _Pawn = pawn;

            if (!Pawn)
                return;

            Pawn.OnPossess(this);

            OnPossessPawn();
        }
        public void UnPossess(PawnSystem pawn)
        {
            if (!Pawn)
                return;

            if (_Pawn != pawn)
                return;

            _Pawn = null;

            pawn.UnPossess(this);

            UnPossessPawn(pawn);
        }
        
        public virtual EAffiliation CheckAffiliation(ControllerSystem targetController)
        {
            if (Affiliation == EAffiliation.Neutral || targetController.Affiliation == EAffiliation.Neutral)
                return EAffiliation.Neutral;

            if (Affiliation == targetController.Affiliation)
            {
                return EAffiliation.Friendly;
            }
            else
            {
                return EAffiliation.Hostile;
            }
        }

        public virtual bool CheckHostile(ControllerSystem targetController)
        {
            switch (Affiliation)
            {
                case EAffiliation.Neutral:
                    return false;
                case EAffiliation.Friendly:
                    return targetController.GetHostile();
                case EAffiliation.Hostile:
                    return targetController.GetFriendly();
                default:
                    return false;
            }
        }

        public virtual bool GetHostile() => _Affiliation.Equals(EAffiliation.Hostile);
        public virtual bool GetNeutral() => _Affiliation.Equals(EAffiliation.Neutral);
        public virtual bool GetFriendly() =>_Affiliation.Equals(EAffiliation.Friendly);


        
        public void SetUseMovementInput(bool useMovementInput)
        {
            if (_UseMovementInput == useMovementInput)
            {
                return;
            }

            _UseMovementInput = useMovementInput;

            if (UseMovementInput)
                OnUseMovementInput();
            else
                UnUseMovementInput();
        }

        public void SetMovementInput(Vector3 direction, float strength)
        {
            if (!UseMovementInput)
                return;

            Vector3 prevDirection = MoveDirection;
            float prevMoveStrength = MoveStrength;

            _MoveDirection = direction;
            _MoveStrength = Mathf.Clamp01(strength);

            if (prevDirection == Vector3.zero && direction != Vector3.zero)
            {
                OnStartMovementInput();
            }
            else if (prevDirection != Vector3.zero && direction == Vector3.zero)
            {
                OnFinishMovementInput(prevDirection, prevMoveStrength);
            }
        }

        public void SetUseTurnInput(bool useTurnInput)
        {
            if (_UseTurnInput == useTurnInput)
            {
                return;
            }

            _UseTurnInput = useTurnInput;

            if (UseTurnInput)
                OnUseTurnInput();
            else
                UnUseTurnInput();
        }

        public void SetTurnInput(Vector3 direction)
        {
            if (!UseTurnInput)
                return;

            Vector3 prevDirection = direction;

            _TurnDirection = direction;

            if (prevDirection == Vector3.zero && _TurnDirection != Vector3.zero)
            {
                OnStartTurnInput();
            }
            else if (prevDirection != Vector3.zero && _TurnDirection == Vector3.zero)
            {
                OnFinishTurnInput(prevDirection);
            }
        }


        #region Look

        public Vector3 GetLookDirection()
        {
            if (Pawn == null)
                return Vector3.zero;

            if (LookTarget != null)
            {
                return (LookTarget.position - Pawn.transform.position).normalized;
            }
            else
            {
                return LookDirection;
            }
        }

        public void SetUseLookInput(bool useLookInput)
        {
            if (_UseLookInput == useLookInput)
            {
                return;
            }

            _UseLookInput = useLookInput;

            if (UseTurnInput)
                OnUseLookInput();
            else
                UnUseLookInput();
        }
        public void SetLookInput(Vector3 direction)
        {
            if (!UseLookInput)
                return;

            Vector3 prevDirection = direction;

            _LookDirection = direction;

            if (prevDirection == Vector3.zero && _LookDirection != Vector3.zero)
            {
                OnStartLookInput();
            }
            else if (prevDirection != Vector3.zero && _LookDirection == Vector3.zero)
            {
                OnFinishLookInput(prevDirection);
            }
        }
        public void SetLookInput(Transform target)
        {
            if (!UseLookInput)
                return;

            if (transform == null)
                return;

            if (Pawn == null)
                return;

            Vector3 direction = target.transform.position - Pawn.transform.position;

            direction.Normalize();

            SetLookInput(direction);
        }

        public void SetLookInputToTarget()
        {
            if (LookTarget == null)
            {
                SetLookInput(Vector3.zero);

                return;
            }

            if (!Pawn)
                return;

            Vector3 direction = LookTarget.transform.position - Pawn.transform.position;

            SetLookInput(direction.normalized);
        }

        public void SetLookTarget(Transform newLookTarget)
        {
            if (_LookTarget == newLookTarget)
            {
                return;
            }

            var prevTarget = _LookTarget;

            _LookTarget = newLookTarget;


            if (_LookTarget == null) 
            {
                SetLookInput(prevTarget);
            }
            else
            {
                SetLookInput(_LookTarget);
            }
            

            OnChangeLookTarget(prevTarget);
        }

        #endregion

        #region EDITOR

        private void OnDrawGizmosSelected()
        {
            if (!UseDebug)
                return;

            if (_Pawn == null)
                return;

            Vector3 start = _Pawn.transform.position + Vector3.up;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(start, MoveDirection * 3f);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(start, TurnDirection * 3f);

            Gizmos.color = Color.yellow;
            if (LookTarget == null)
            {
                Gizmos.DrawRay(start, LookDirection * 3f);
            }
            else
            {
                Gizmos.DrawRay(start, GetLookDirection() * 3f);
                Gizmos.DrawWireSphere(LookTarget.position + Vector3.up, 1f);
            }
        }

        #endregion

        #region Callback
        protected void OnPossessPawn()
        {
            Log("On Possessed Pawn - " + Pawn.name);

            OnPossessedPawn?.Invoke(this, Pawn);
        }
        protected void UnPossessPawn(PawnSystem prevPawn)
        {
            Log("Un Possessed Pawn - " + prevPawn.name);

            UnPossessedPawn?.Invoke(this, prevPawn);
        }


        protected void OnUseMovementInput()
        {
            Log("On Use Movement Input");

            OnUsedMovementInput?.Invoke(this);
        }
        protected void UnUseMovementInput()
        {
            Log("Un Use Movement Input");

            UnUsedMovementInput?.Invoke(this);
        }


        protected void OnStartMovementInput()
        {
            Log("On Start Movement Input -" + MoveDirection + " * " + MoveStrength);

            OnStartedMovementInput?.Invoke(this, MoveDirection, MoveStrength);
        }
        protected void OnFinishMovementInput(Vector3 prevDirection, float prevMoveStrength)
        {
            Log("On Finish Movement Input -" + prevDirection + " * " + prevMoveStrength);

            OnFinishedMovementInput?.Invoke(this, prevDirection, prevMoveStrength);
        }


        protected void OnUseTurnInput()
        {
            Log("On Use Turn Input");

            OnUsedTurnInput?.Invoke(this);
        }
        protected void UnUseTurnInput()
        {
            Log("Un Use Turn Input");

            UnUsedTurnInput?.Invoke(this);
        }
        protected void OnStartTurnInput()
        {
            Log("On Start Turn Input - " + TurnDirection);

            OnStartedTurntInput?.Invoke(this, TurnDirection);
        }
        protected void OnFinishTurnInput(Vector3 prevDirection)
        {
            Log("On Finish Turn Input - " + prevDirection);

            OnFinishedTurnInput?.Invoke(this, prevDirection);
        }

        protected void OnUseLookInput()
        {
            Log("On Use Look Input");

            OnUsedLookInput?.Invoke(this);
        }
        protected void UnUseLookInput()
        {
            Log("Un Use Look Input");

            UnUsedLookInput?.Invoke(this);
        }
        protected void OnStartLookInput()
        {
            Log("On Start Look Input - " + TurnDirection);

            OnStartedLookInput?.Invoke(this, TurnDirection);
        }
        protected void OnFinishLookInput(Vector3 prevDirection)
        {
            Log("On Finish Look Input - " + prevDirection);

            OnFinishedLookInput?.Invoke(this, prevDirection);
        }

        protected void OnChangeLookTarget(Transform prevLookTarget = null)
        {
            Log("On Change Look Target - New Look Target : " + _LookTarget + " Prev Look Target : " + prevLookTarget);

            OnChangedLookTarget?.Invoke(this, _LookTarget, prevLookTarget);
        }
        #endregion
    }

}

