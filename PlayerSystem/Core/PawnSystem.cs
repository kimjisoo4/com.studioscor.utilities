using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace StudioScor.PlayerSystem
{
    public class PawnSystem : MonoBehaviour
    {
        #region Events
        public delegate void ChangedControllerHandler(PawnSystem pawn, ControllerSystem controller);
        public delegate void IgnoreInput(PawnSystem pawn, bool ignore);
        #endregion

        [Header(" [ Use Player Controller ] ")]
        [SerializeField] private bool _IsPlayer = false;

        [Header(" [ Default Ai Controller ] ")]
        [SerializeField] private ControllerSystem _DefaultController;

        [SerializeField] private ControllerSystem _Controller;
        public ControllerSystem DefaultController => _DefaultController;
        public ControllerSystem Controller => _Controller;

        [SerializeField] private bool _UseAutoPossesed = true;

        [Header(" [ Ignore Movement Input ] ")]
        [SerializeField] private bool _IgnoreMovementInput = false;
        public bool IgnoreMovementInput => _IgnoreMovementInput;

        [Header(" [ Ignore Rotate Input ]")]
        [SerializeField] private bool _IgnoreRotateInput = false;
        public bool IgnoreRotateInput => _IgnoreRotateInput;

        [Header(" [ Use DebugMode] ")]
        [SerializeField] private bool _UseDebugMode = false;
        public bool IsPlayer => _IsPlayer;
        public bool IsPossessed => Controller;
        
        public event ChangedControllerHandler OnPossessedController;
        public event ChangedControllerHandler UnPossessedController;

        public event IgnoreInput OnChangedIgnoreMovementInput;
        public event IgnoreInput OnChangedIgnoreRotateInput;

        private void Awake()
        {
            PlayerManager.Instance.AddPawn(this);
        }
        private void OnDestroy()
        {
#if UNITY_EDITOR
            if (!this.gameObject.scene.isLoaded) return;
#endif
            PlayerManager.Instance.RemovePawn(this);
        }
        
        protected void OnEnable()
        {
            OnInitialization();
        }

        private void OnInitialization()
        {
            if (_Controller != null)
            {
                _Controller.OnPossess(this);

                return;
            }

            if (_IsPlayer && PlayerManager.Instance.PlayerCharacter == null)
            {
                PlayerManager.Instance.PlayerController.OnPossess(this);
            }
            else
            {
                if (_UseAutoPossesed)
                {
                    SpawnAndPossessAiController();
                }
            }
        }

        public void OnPossess(ControllerSystem controller)
        {
            if (Controller == controller)
            {
                return;
            }

            UnPossess(Controller);

            _Controller = controller;

            if (!Controller)
                return;

            if (Controller.IsPlayerController)
            {
                _IsPlayer = true;
            }

            Controller.OnPossess(this);

            OnPossessController();
        }

        public void UnPossess(ControllerSystem controller)
        {
            if (!controller)
                return;

            if (Controller != controller)
                return;

            _Controller = null;

            controller.UnPossess(this);

            if (controller.IsPlayerController)
            {
                _IsPlayer = false;

                if (_UseAutoPossesed)
                {
                    SpawnAndPossessAiController();
                }
            }

            UnPossessController(controller);
        }

        private void SpawnAndPossessAiController()
        {
            if (_DefaultController != null)
            {
                _Controller = Instantiate(_DefaultController);

                _Controller.OnPossess(this);
            }
        }

        #region Setter
        public void SetIgnoreMovementInput(bool useMovementInput)
        {
            if (_IgnoreMovementInput == useMovementInput)
            {
                return;
            }

            _IgnoreMovementInput = useMovementInput;

            OnChangeIgnoreMovementInput();
        }
        public void SetIgnoreRotateInput(bool useRotateInput)
        {
            if (_IgnoreRotateInput == useRotateInput)
            {
                return;
            }

            _IgnoreRotateInput = useRotateInput;

            OnChangeIgnoreRotateInput();
        }
        #endregion

        #region Getter
        public Vector3 GetMoveDirection()
        {
            if (IgnoreMovementInput || !Controller)
                return Vector3.zero;

            return Controller.MoveDirection;
        }
        public float GetMoveStrength()
        {
            if (IgnoreMovementInput || !Controller)
                return 0;

            return Controller.MoveStrength;
        }
        public Vector3 GetRotateDirection()
        {
            if (IgnoreRotateInput || !Controller)
                return Vector3.zero;

            return Controller.TurnDirection;
        }
        #endregion

        #region EDITOR
        [Conditional("UNITY_EDITOR")]
        private void Log(string log)
        {
            if (_UseDebugMode)
                UnityEngine.Debug.Log("PawnSystem [" + gameObject.name  + "] : " + log, this);
        }
        #endregion

        #region Callback
        protected void OnPossessController()
        {
            Log("On Possessed Controller [" + gameObject.name + "] " + Controller);

            OnPossessedController?.Invoke(this, Controller);
        }
        protected void UnPossessController(ControllerSystem prevController)
        {
            Log("Un Possessed Controller [" + gameObject.name + "] " + prevController);

            UnPossessedController?.Invoke(this, prevController);
        }

        protected void OnChangeIgnoreMovementInput()
        {
            Log("On Change Ignore Movement Input : " + IgnoreMovementInput);

            OnChangedIgnoreMovementInput?.Invoke(this, IgnoreMovementInput);
        }

        protected void OnChangeIgnoreRotateInput()
        {
            Log("On Change Ignore Rotate Input : " + IgnoreRotateInput);

            OnChangedIgnoreRotateInput?.Invoke(this, IgnoreRotateInput);
        }

        #endregion
    }

}

