#if SCOR_ENABLE_CINEMACHINE
using UnityEngine;
using Cinemachine;
using StudioScor.Utilities;

namespace StudioScor.CameraSystem
{
    public class CameraManager : Singleton<CameraManager>
    {
        #region Events
        public delegate void TransitionCamera(CameraManager cameraManager);
        public delegate void ChangeCameraHandler(CameraManager cameraManager, CameraBase currentCamera, CameraBase prevCamera);
        #endregion
        [Header(" [ Camera Manager ] ")]
        [SerializeField] private CameraBase _DefaultCamera;
        [SerializeField] private CameraBase _CurrentCamera;
        [SerializeField] private CinemachineBrain _CinemachineBrain;
        [SerializeField] private CinemachineImpulseSource _CinemachineImpulseSource;

        private bool _IsTransition = false;
        public CameraBase CurrentCamera => _CurrentCamera;

        public event ChangeCameraHandler OnChangedCamera;

        public event TransitionCamera OnStartedTransitionCamera;
        public event TransitionCamera OnFinishedTransitionCamera;

#if UNITY_EDITOR
        private void Reset()
        {
            if (!_CinemachineImpulseSource)
                _CinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

            if(!_CinemachineBrain)
                _CinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        }
#endif


        protected override void Setup()
        {
            base.Setup();

            if (!_CinemachineImpulseSource)
                _CinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

            if (!_CinemachineBrain)
                _CinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        }

        protected virtual void Start()
        {
            if (!_CurrentCamera && _DefaultCamera)
            {
                TransitionCameraState(_DefaultCamera);
            }
        }

        public void TransitionCameraState(CameraBase cameraBase)
        {
            if (cameraBase == null || cameraBase == _CurrentCamera)
                return;

            var prevCaemra = CurrentCamera;
            
            bool isTransition = false;

            if (_CurrentCamera != null)
            {
                _CurrentCamera.OnInActivateCamera();

                isTransition = true;
            }

            _CurrentCamera = cameraBase;

            _CurrentCamera.OnActivateCamera();

            OnChangeCamera(prevCaemra);

            if (isTransition)
            {
                if (_IsTransition)
                {
                    OnFinishTransitionCamera();
                }

                _IsTransition = true;

                OnStartTransitionCamera();
            }
        }

        public void DefaultCameraState()
        {
            TransitionCameraState(_DefaultCamera);
        }

        public void OnCameraShake(Vector3 direction = default, float force = 0.1f)
        {
            if (!_CinemachineImpulseSource)
                return;

            if(direction == default)
            {
                direction = Random.insideUnitCircle * force;
            }

            _CinemachineImpulseSource.GenerateImpulse(direction * force);
        }

        private void LateUpdate()
        {
            if (_IsTransition)
            {
                if (!_CinemachineBrain.IsBlending)
                {
                    _IsTransition = false;

                    OnFinishTransitionCamera();
                }
            }
        }
        #region Callback
        protected void OnChangeCamera(CameraBase prevCamera)
        {
            Log(" On Changed Camera - Curret : " + CurrentCamera + " Prev : " + prevCamera);

            OnChangedCamera?.Invoke(this, CurrentCamera, prevCamera);
        }
        protected void OnStartTransitionCamera()
        {
            Log(" On Started Transition Camera");

            OnStartedTransitionCamera?.Invoke(this);
        }
        protected void OnFinishTransitionCamera()
        {
            Log(" On Finishied Transition Camera");

            OnFinishedTransitionCamera?.Invoke(this);
        }
        #endregion
    }

}
#endif