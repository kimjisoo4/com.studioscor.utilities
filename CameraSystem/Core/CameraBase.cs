using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace StudioScor.CameraSystem
{
    public abstract class CameraBase : MonoBehaviour
    {
		#region events
		public delegate void CameraBaseEventHandler(CameraBase cameraBase);
		#endregion

		[Header(" [ Cinemachine ] ")]
		[SerializeField] private CinemachineVirtualCamera _CinemachineCamera;
		
		public CinemachineVirtualCamera CinemachineCamera
		{
			get
			{
				if (_CinemachineCamera == null)
				{
					_CinemachineCamera = GetComponent<CinemachineVirtualCamera>();
				}

				return _CinemachineCamera;
			}
		}
		[SerializeField, Range(0f, 1f)] private float _DistanceRange;
		public float DistanceRange => _DistanceRange;

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
            if(_CinemachineCamera == null)
				_CinemachineCamera = GetComponent<CinemachineVirtualCamera>();
		}

		public void OnSelfActive()
        {
			CameraManager.Instance.TransitionCameraState(this);
		}

		public virtual void OnActiveCamera()
        {
			CinemachineCamera.Priority = 11;
		}
		public virtual void OnDeactivateCamera()
        {
			CinemachineCamera.Priority = 10;
		}


		public abstract void UpdateTarget(Transform followTarget = null);

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
			OnReachedMinDistance?.Invoke(this);
		}
		protected virtual void OnReachMaxDistance()
		{
			OnReachedMaxDistance?.Invoke(this);
		}
		#endregion
	}
}
