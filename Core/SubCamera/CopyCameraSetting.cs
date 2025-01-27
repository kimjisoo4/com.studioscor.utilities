using UnityEngine;
#if SCOR_ENABLE_CINEMACHINE
using Unity.Cinemachine;
#endif

namespace StudioScor.Utilities
{
    public class CopyCameraSetting : BaseMonoBehaviour
    {
        [Header(" [ Sub Camera ] ")]
        [SerializeField] private Camera targetCamera;

#if !SCOR_ENABLE_CINEMACHINE
        private Camera mainCamera;
#endif
        private void Reset()
        {
#if UNITY_EDITOR
            gameObject.TryGetComponentInParentOrChildren(out targetCamera);
#endif
        }
        private void OnEnable()
        {
            CopySetting(Camera.main);

#if SCOR_ENABLE_CINEMACHINE
            CinemachineCore.CameraUpdatedEvent.AddListener(CameraUpdate);
#else
            mainCamera = Camera.main;
#endif
        }

        private void OnDisable()
        {
#if SCOR_ENABLE_CINEMACHINE
            if (!gameObject.scene.isLoaded)
                return;

            CinemachineCore.CameraUpdatedEvent.RemoveListener(CameraUpdate);
#endif
        }
        
        protected virtual void CopySetting(Camera camera)
        {
            targetCamera.orthographic = camera.orthographic;
            targetCamera.fieldOfView = camera.fieldOfView;
            targetCamera.farClipPlane = camera.farClipPlane;
            targetCamera.nearClipPlane = camera.nearClipPlane;
        }

#if SCOR_ENABLE_CINEMACHINE
        private void CameraUpdate(CinemachineBrain brain)
        {
            if (!brain.IsBlending)
                return;

            var camera = brain.OutputCamera;

            CopySetting(camera);
        }
#else
        private void LateUpdate()
        {
            CopySetting(mainCamera);
        }
#endif

    }
}