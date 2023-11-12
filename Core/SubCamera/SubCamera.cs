using System;
using UnityEngine;
#if SCOR_ENABLE_UNIVERSAL_RENDER
using UnityEngine.Rendering.Universal;
#endif

namespace StudioScor.Utilities
{
    public class SubCamera : BaseMonoBehaviour
    {
        [Header(" [ Sub Camera ] ")]
        [SerializeField] private Camera targetCamera;
        [SerializeField] private bool isChildTransform = true;
        [SerializeField][SCondition(nameof(isChildTransform))] private bool isStayPosition = false;

        [Header(" [ Auto Playing ] ")]
        [SerializeField] private bool isAutoPlaying = true;

        private void Reset()
        {
#if UNITY_EDITOR
            gameObject.TryGetComponentInParentOrChildren(out targetCamera);
#endif
        }
        private void OnEnable()
        {
            if (isAutoPlaying)
                AttachCamera();
        }
        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded)
                return;
         
            DetachCamera();
        }

        protected void AttachCamera()
        {
            var mainCamera = Camera.main;

#if SCOR_ENABLE_UNIVERSAL_RENDER
            var cameraData = mainCamera.GetUniversalAdditionalCameraData();

            if(!cameraData.cameraStack.Contains(targetCamera))
                cameraData.cameraStack.Add(targetCamera);
#endif

            if(isChildTransform)
            {
                targetCamera.transform.SetParent(mainCamera.transform, isStayPosition);
            }
        }

        protected void DetachCamera()
        {
#if SCOR_ENABLE_UNIVERSAL_RENDER
            var mainCamera = Camera.main;

            var cameraData = mainCamera.GetUniversalAdditionalCameraData();

            cameraData.cameraStack.Remove(targetCamera);
#endif

            if (isChildTransform)
            {
                targetCamera.transform.parent = null;
            }
        }
    }
}