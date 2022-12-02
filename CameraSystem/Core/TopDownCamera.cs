using UnityEngine;
using Cinemachine;

namespace StudioScor.CameraSystem
{
    public class TopDownCamera : CameraBase
    {
		[Header(" [ Follow Setting ] ")]
		[SerializeField] private bool _UseFollow = true;
		[SerializeField] private bool _UseLookAt = false;

		[Header(" [ Zoom Setting ] ")]
		[SerializeField] private Vector3 _MinDisntace;
		[SerializeField] private Vector3 _MaxDisntace;
			

		private CinemachineTransposer _CinemachineTransposer;

		public CinemachineTransposer CinemachineTransposer
        {
            get
            {
                if (_CinemachineTransposer == null)
                {
					_CinemachineTransposer = CinemachineCamera.GetCinemachineComponent<CinemachineTransposer>();
				}

				return _CinemachineTransposer;
            }
        }
        public override void UpdateDistance()
        {
			Vector3 position = Vector3.Lerp(_MinDisntace, _MaxDisntace, DistanceRange);

			CinemachineTransposer.m_FollowOffset = position;
		}

        public override void UpdateTarget(Transform followTarget = null)
		{
			if (_UseFollow)
				CinemachineCamera.Follow = followTarget;

			if (_UseLookAt)
				CinemachineCamera.LookAt = followTarget;
		}
	}
}
