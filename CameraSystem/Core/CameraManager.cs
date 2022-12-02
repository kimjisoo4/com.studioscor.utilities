using UnityEngine;
using Cinemachine;

namespace StudioScor.CameraSystem
{
    public class CameraManager : MonoBehaviour
    {
        private static CameraManager _Instance = null;

        [SerializeField] private CameraBase _DefaultCamera;
        [SerializeField] private CameraBase _CurrentCamera;
        [SerializeField] private CinemachineImpulseSource _CinemachineImpulseSource;

        public CameraBase CurrentCamera => _CurrentCamera;

#if UNITY_EDITOR
        private void Reset()
        {
            transform.TryGetComponent(out _CinemachineImpulseSource);
        }
#endif
        public static CameraManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<CameraManager>();
                }

                return _Instance;
            }
        }
        void Awake()
        {
            if (_Instance == null)
            {
                _Instance = this;
            }
        }
        private void Start()
        {
            if (_CurrentCamera == null)
            {
                TransitionCameraState(_DefaultCamera);
            }
        }

        public void TransitionCameraState(CameraBase cameraBase)
        {
            if (cameraBase == null || cameraBase == _CurrentCamera)
                return;

            if (_CurrentCamera != null)
            {
                _CurrentCamera.OnDeactivateCamera();
            }

            _CurrentCamera = cameraBase;

            _CurrentCamera.OnActiveCamera();
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
    }

}
