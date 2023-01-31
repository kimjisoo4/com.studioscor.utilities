#if SCOR_ENABLE_CINEMACHINE
using UnityEngine;
using Cinemachine;

namespace StudioScor.CameraSystem
{

    public class TopDownCamera : CameraBase
    {
		[Header(" [ Zoom Setting ] ")]
		[SerializeField] private Vector3 _MinDisntace;
		[SerializeField] private Vector3 _MaxDisntace;
			

		private CinemachineTransposer _CinemachineTransposer;

		public CinemachineTransposer CinemachineTransposer
        {
            get
            {
                if (!_WasSetup)
                {
                    Setup();
                }

				return _CinemachineTransposer;
            }
        }
        protected override void Setup()
        {
            base.Setup();

            if (_CinemachineTransposer == null)
            {
                _CinemachineTransposer = CinemachineCamera.GetCinemachineComponent<CinemachineTransposer>();
            }
        }
        public override void UpdateDistance()
        {
			Vector3 position = Vector3.Lerp(_MinDisntace, _MaxDisntace, DistanceRange);

			CinemachineTransposer.m_FollowOffset = position;
		}
	}
}
#endif