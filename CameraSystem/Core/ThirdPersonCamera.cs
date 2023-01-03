#if ENABLE_CINEMACHINE
using UnityEngine;
using Cinemachine;

namespace StudioScor.CameraSystem
{
    public class ThirdPersonCamera : CameraBase
    {
        [Header(" [ Third Person Camera ] ")]
        private Cinemachine3rdPersonFollow _ThirdPersonFollow;

        [SerializeField]private float _MinDistance = 0f;
        [SerializeField]private float _MaxDistance = 2f;

        protected override void Setup()
        {
            base.Setup();

            if (_ThirdPersonFollow is null)
            {
                _ThirdPersonFollow = CinemachineCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            }
        }
        public override void UpdateDistance()
        {
            _ThirdPersonFollow.CameraDistance = Mathf.Lerp(_MinDistance, _MaxDistance, DistanceRange);
        }
    }
}
#endif