using UnityEngine;
using Cinemachine;

namespace StudioScor.CameraSystem
{
    public abstract class ThirdPersonCamera : CameraBase
    {
        [Header(" [ Follow Target ] ")]
		[SerializeField] protected Transform _FollowCameraRoot;

		[Header(" [ Zoom Setting ] ")]
		[SerializeField] private float _MinDistance = 6;
		[SerializeField] private float _MaxDistance = 10;

		[Header(" [ Follow Setting ] ")]
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;
		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;
		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;
		[Tooltip("For locking Speed Multiplier")]
		public float TurnSpeed = 1f;
		public float LookTurnSpeed = 1f;
		public bool ReverseVertical = false;

		private float _cinemachineTargetYaw;
		private float _cinemachineTargetPitch;
		private const float _threshold = 0.01f;
		protected abstract bool IsCurrentDeviceMouse { get; }
		protected abstract Vector2 LookDirection { get; }

		protected Vector3 _LookAtDirection = Vector3.zero;
		protected Transform _LookAtTarget = null;

		private Cinemachine3rdPersonFollow _Cinemachine3rdPersonFollow;

		protected Cinemachine3rdPersonFollow Cinemachine3rdPersonFollow
        {
            get
            {
                if (_Cinemachine3rdPersonFollow == null)
                {
					_Cinemachine3rdPersonFollow = CinemachineCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
				}

				return _Cinemachine3rdPersonFollow;
            }
        }

		private void LateUpdate()
        {
			if (!CinemachineCore.Instance.IsLive(CinemachineCamera))
				return;

			float deltaTime = Time.deltaTime;

			CameraRotation(deltaTime);
        }
        public override void OnActiveCamera()
        {
            base.OnActiveCamera();
			
/*			시작시 방향을 돌리는 옵션. 고려해볼 요소임
			Vector3 eulerAngles = _FollowCameraRoot.eulerAngles;

			_cinemachineTargetYaw = eulerAngles.y;
			_cinemachineTargetPitch = eulerAngles.x;

			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
*/
		}
        public override void UpdateDistance()
        {
			float distnace = Mathf.Lerp(_MinDistance, _MaxDistance, DistanceRange);

			Cinemachine3rdPersonFollow.CameraDistance = distnace;
		}
        public void LookAtDirection(Vector3 direction)
		{
			_LookAtDirection = direction;
		}
		public void LookAtTarget(Transform target)
        {
			_LookAtTarget = target;
		}
		public void CameraRotation(float deltaTime)
		{
			if (_FollowCameraRoot == null)
				return;

			float cinemachineTargetYaw = _cinemachineTargetYaw;
			float cinemachineTargetPitch = _cinemachineTargetPitch;

			if (!LockCameraPosition && LookDirection.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime;
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : deltaTime;

				deltaTimeMultiplier *= TurnSpeed;

				cinemachineTargetYaw += LookDirection.x * deltaTimeMultiplier;
				cinemachineTargetPitch += LookDirection.y * deltaTimeMultiplier;
			}

			if (_LookAtTarget != null)
			{
				Vector3 direction = _LookAtTarget.position - _FollowCameraRoot.position;
				Vector3 eulerAngle = Quaternion.LookRotation(direction).eulerAngles;

				cinemachineTargetYaw = Mathf.MoveTowardsAngle(_cinemachineTargetYaw, eulerAngle.y, deltaTime * LookTurnSpeed);
			}
			else if(_LookAtDirection != Vector3.zero)
			{
				Vector3 eulerAngle = Quaternion.LookRotation(_LookAtDirection).eulerAngles;

				cinemachineTargetYaw = Mathf.MoveTowardsAngle(_cinemachineTargetYaw, eulerAngle.y, deltaTime * LookTurnSpeed);
			}

			// clamp our rotations so our values are limited 360 degrees
			_cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

			// Cinemachine will follow this target
			_FollowCameraRoot.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f)
				lfAngle += 360f;

			if (lfAngle > 360f)
				lfAngle -= 360f;

			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

    }
}
