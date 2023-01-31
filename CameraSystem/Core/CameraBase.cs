#if SCOR_ENABLE_CINEMACHINE
using UnityEngine;
using Cinemachine;

using StudioScor.Utilities;
namespace StudioScor.CameraSystem
{
    public abstract class CameraBase : BaseMonoBehaviour
    {
		#region events
		public delegate void CameraBaseEventHandler(CameraBase cameraBase);
		#endregion

		[Header(" [ Camera Base ] ")]
		[SerializeField] private CinemachineVirtualCamera _CinemachineCamera;
        [Space(5f)]
		[SerializeField] private bool _UseFollow = true;
		[SerializeField] private bool _UseLookAt = false;
		[Space(5f)]
		[SerializeField, Range(0f, 1f)] private float _DistanceRange;


		protected bool _WasSetup = false;
		public CinemachineVirtualCamera CinemachineCamera
		{
			get
			{
				if (!_WasSetup)
				{
					Setup();
				}

				return _CinemachineCamera;
			}
		}
		public float DistanceRange => _DistanceRange;

		public event CameraBaseEventHandler OnActivatedCamera;
		public event CameraBaseEventHandler OnInActivatedCamera;
		public event CameraBaseEventHandler OnReachedMinDistance;
		public event CameraBaseEventHandler OnReachedMaxDistance;

#if UNITY_EDITOR
		private void Reset()
		{
			_CinemachineCamera = GetComponent<CinemachineVirtualCamera>();
		}
#endif
        private void Awake()
        {
            if (!_WasSetup)
            {
				Setup();
            }
		}

		protected virtual void Setup()
        {
			_WasSetup = true;

			if (_CinemachineCamera == null)
			{
				if (!TryGetComponent(out _CinemachineCamera))
				{
					Log("Cinimachine Virtual Camera Is Null", true);
				}
			}

		}
		public void OnSelfActive()
        {
			CameraManager.Instance.TransitionCameraState(this);
		}

		public virtual void OnActivateCamera()
        {
			CinemachineCamera.Priority = 11;

			OnActiveCamera();
		}
		public virtual void OnInActivateCamera()
        {
			CinemachineCamera.Priority = 10;

			OnInActiveCamera();
		}
		public virtual void UpdateTarget(Transform followTarget = null)
		{
			if (_UseFollow)
				CinemachineCamera.Follow = followTarget;

			if (_UseLookAt)
				CinemachineCamera.LookAt = followTarget;
		}
		public void AddDistance(float addDistance)
        {
			SetDistance(_DistanceRange + addDistance);
        }
		public void SubDistance(float subDistance)
        {
			SetDistance(_DistanceRange - subDistance);
		}

		public void SetDistance(float distnace)
        {
			_DistanceRange = Mathf.Clamp01(distnace);

			UpdateDistance();

			if (_DistanceRange <= 0)
			{
				OnReachMinDistance();
			}
			else if (_DistanceRange >= 1)
			{
				OnReachMaxDistance();
			}
		}

		public abstract void UpdateDistance();


		#region CallBack
		protected virtual void OnReachMinDistance()
		{
			Log("On Reached Min Distance");

			OnReachedMinDistance?.Invoke(this);
		}
		protected virtual void OnReachMaxDistance()
		{
			Log("On Reached Max Distance");

			OnReachedMaxDistance?.Invoke(this);
		}
		protected virtual void OnActiveCamera()
		{
			Log("On Activated Camera ");

			OnActivatedCamera?.Invoke(this);
		}
		protected virtual void OnInActiveCamera()
		{
			Log("On InActivated Camera");

			OnInActivatedCamera?.Invoke(this);
		}
		#endregion
	}
}
#endif