using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class Timer
    {
		public delegate void TimerHandler(Timer timer);

		[SerializeField] private float _Duration;
		
		private float _RemainTime;
		private float _ElapsedTime;
		private float _NormalizedTime;

		private bool _IsPlaying;
		private bool _IsFinished;

		public event TimerHandler OnStartedTimer;
		public event TimerHandler OnFinishedTimer;
		public event TimerHandler OnCanceledTimer;

		public float Duration => _Duration;
		public float RemainTime => _RemainTime;
		public float ElapsedTime => _ElapsedTime;
		public float NormalizedTime => _NormalizedTime;
		public bool IsPlaying => _IsPlaying;
		public bool IsStoped => !_IsPlaying;
		public bool IsFinished => _IsFinished;

		public Timer()
        {
			_Duration = 0.2f;
			_RemainTime = _Duration;
			_ElapsedTime = 0;
			_IsPlaying = false;
			_IsFinished = false;
		}

		public Timer(float time, bool isPlaying = false)
        {
			_Duration = time;
			_RemainTime = _Duration;
			_ElapsedTime = 0;
			_IsPlaying = isPlaying;
			_IsFinished = false;
		}
		public void OnResetTimer()
        {
			_RemainTime = _Duration;
			_ElapsedTime = 0;
			_IsFinished = false;
		}
		public void OnTimer(float duration)
        {
			_Duration = duration;

			OnTimer();
        }
		public void OnTimer()
        {
            if (IsPlaying)
            {
				OnStopTimer();
			}

			OnResetTimer();

			_IsPlaying = true;

			OnStartedTimer?.Invoke(this);
		}

		public void SetDuration(float duration)
        {
			_Duration = duration;
        }
		public void JumpTimer(float time)
        {
			_RemainTime -= time;
			_ElapsedTime = time;
        }

		public void OnFinisheTimer()
		{
			_IsPlaying = false;
			_RemainTime = 0;
			_ElapsedTime = _Duration;
			_IsFinished = true;
			_NormalizedTime = 1;

			OnFinishedTimer?.Invoke(this);
		}
		private void OnCancelTimer()
        {
			_IsPlaying = false;
			_IsFinished = false;

			OnCanceledTimer?.Invoke(this);
		}
		public void OnPauseTimer()
        {
			if (!_IsPlaying)
				return;

			_IsPlaying = false;
        }
		public void OnResumeTimer()
        {
			if (_IsPlaying)
				return;

			_IsPlaying = true;
		}

		public void OnStopTimer()
        {
			if (!_IsPlaying)
				return;

            if (_RemainTime <= 0f)
            {
				OnFinisheTimer();
			}
            else
            {
				OnCancelTimer();
			}
		}
		
		public void UpdateTimer(float deltaTime)
        {
			if (_IsFinished || !_IsPlaying)
				return;

			_RemainTime -= deltaTime;
			_ElapsedTime = _Duration - _RemainTime;

            if (_RemainTime <= 0)
            {
				OnFinisheTimer();

				return;
			}

			_NormalizedTime = _ElapsedTime / _Duration;
        }
    }

}
