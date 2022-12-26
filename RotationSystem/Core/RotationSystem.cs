using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.RotationSystem
{

    public abstract class RotationSystem : MonoBehaviour
    {
        [Header(" [ Setting ] ")]
        [SerializeField] protected ERotationType _RotationType;
        [SerializeField] protected float _TurnSpeed = 360;
        public float TurnSpeed => _TurnSpeed;

        [SerializeField] protected Transform _TargetTransform;
        [SerializeField] protected Camera _MainCamera;
        [SerializeField] protected bool _UseRotation = true;

        public bool UseRotation => _UseRotation;
        public Transform TargetTransform => _TargetTransform;
        public Camera MainCamera => _MainCamera;

        [Header(" [ Ignore Input ] ")]
        [SerializeField] private bool _IgnoreInput = false;

        [Header("[Debug Mode]")]
        [SerializeField] protected bool DebugMode = false;

        protected Vector3 _TurnEulerAngles = Vector3.zero;

        protected Vector3 _InputDirection = Vector3.zero;

        protected Vector3 _RotationDirection = Vector3.zero;
        public Vector3 TurnEulerAngles => _TurnEulerAngles;
        public Vector3 RotationDirection => _RotationDirection;
        public Vector3 InputDirection => _InputDirection;

        public ERotationType RotationType => _RotationType;

        private int _IgnoreInputStack = 0;
        public bool IgnoreInput => _IgnoreInput;

        #region Setter
        public void SetRotationType(ERotationType newRotationType)
        {
            _RotationType = newRotationType;
        }
        public void SetTurnSpeed(float newTurnSpeed)
        {
            _TurnSpeed = newTurnSpeed;
        }
        public void SetUseRotation(bool useRotation)
        {
            _UseRotation = useRotation;
        }

        public void SetInputDirection(Vector3 direction)
        {
            if (IgnoreInput || direction == Vector3.zero)
            {
                _InputDirection = transform.forward;

                return;
            }

            _InputDirection = direction;
        }
        public void SetRotationDirection(Vector3 direction)
        {
            if (direction == Vector3.zero)
            {
                _RotationDirection = transform.forward;
                
                return;
            }

            _RotationDirection = direction;
        }

        public void SetRotationTarget(Transform target)
        {
            _TargetTransform = target;
        }
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

        #endregion

        private void Awake()
        {
            _MainCamera = Camera.main;
        }

        public virtual void OnRotation(float deltaTime)
        {
            if (!_UseRotation)
            {
                _TurnEulerAngles = transform.eulerAngles;

                UpdateRotation(deltaTime);

                return;
            }

            switch (_RotationType)
            {
                case ERotationType.InputDirection:
                    OnRotationToInputDirection();
                    break;
                case ERotationType.Direction:
                    OnRotationToDirection();
                    break;
                case ERotationType.Target:
                    OnRotationToTarget();
                    break;
                case ERotationType.TargetOrDirection:
                    OnRotationToTargetOrDirection();
                    break;
                case ERotationType.Camera:
                    OnRotationToCamera();
                    break;
                default:
                    break;
            }

            UpdateRotation(deltaTime);
        }

        protected abstract void UpdateRotation(float deltaTime);

        public virtual void ResetRotation()
        {
            _TurnEulerAngles = Vector3.zero;
            _InputDirection = Vector3.zero;
            _RotationDirection = Vector3.zero;

            _TargetTransform = null;

            ClearIngnoreInput();

        }
        public virtual void OnRotationToInputDirection()
        {
            if (InputDirection == Vector3.zero)
            {
                _TurnEulerAngles = transform.eulerAngles;

                return;
            }

            Quaternion newRotation = Quaternion.LookRotation(InputDirection);

            _TurnEulerAngles = newRotation.eulerAngles;
        }
        public virtual void OnRotationToDirection()
        {
            if (RotationDirection == Vector3.zero)
            {
                _TurnEulerAngles = transform.eulerAngles;

                return;
            }

            Quaternion newRotation = Quaternion.LookRotation(RotationDirection);

            _TurnEulerAngles = newRotation.eulerAngles;
        }

        public virtual void OnRotationToTarget()
        {
            if (TargetTransform == null)
            {
                _TurnEulerAngles = transform.eulerAngles;

                return;
            }

            Vector3 direction = TargetTransform.position - transform.position;

            direction.Normalize();

            Quaternion newRotation = Quaternion.LookRotation(direction);

            _TurnEulerAngles = newRotation.eulerAngles;
        }

        public virtual void OnRotationToCamera()
        {
            if (MainCamera == null)
            {
                _TurnEulerAngles = transform.eulerAngles;

                return;
            }

            Vector3 direction = MainCamera.transform.forward;

            Quaternion newRotation = Quaternion.LookRotation(direction);

            _TurnEulerAngles = newRotation.eulerAngles;

        }

        public virtual void OnRotationToTargetOrDirection()
        {
            if (TargetTransform != null)
            {
                OnRotationToTarget();
            }
            else
            {
                OnRotationToDirection();
            }
        }

        public virtual void SetRotation(Vector3 direction)
        {
            Quaternion newRotation = Quaternion.LookRotation(direction);

            _TurnEulerAngles = newRotation.eulerAngles;

            transform.rotation = newRotation;
        }
    }
}