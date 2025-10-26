using UnityEngine;
using static StudioScor.Utilities.ITimer;

namespace StudioScor.Utilities
{
	public interface ITimer
	{
		public delegate void TimerStateHandler(ITimer timer);
		public void Play(float newDuration = -1f);
		public void EndTimer();
		public void UpdateTimer(float deltaTime);
		public void JumpTime(float newTime);
		public void PauseTimer();
		public void ResumeTimer();
		public void End();
		public void CancelTimer();

		public bool IsPlaying { get; }
		public bool IsStopped { get; }
		public bool IsPaused { get; }
		public bool IsFinished { get; }
		public float Duration { get; }
		public float RemainTime { get; }
		public float ElapsedTime { get; }
		public float NormalizedTime { get; }

        public event TimerStateHandler OnStarted;
        public event TimerStateHandler OnFinished;
        public event TimerStateHandler OnCanceled;
		public event TimerStateHandler OnEnded;
		public event TimerStateHandler OnPaused;
		public event TimerStateHandler OnResumed;
    }

    [System.Serializable]
    public class Timer : BaseClass, ITimer
    {
		[field: SerializeField]public float Duration { get; set; }
		
		private float _remainTime;
		private float _elapsedTime;
		private float _normalizedTime;

		private bool _isPaused;
		private bool _isFinished;

		public bool IsPlaying { get; private set; }
		public float RemainTime => IsPlaying ? _remainTime : 0f;
		public float ElapsedTime => IsPlaying ?  _elapsedTime : 0f;
		public float NormalizedTime => _normalizedTime;
		public bool IsPaused => IsPlaying && _isPaused;
		public bool IsStopped => !IsPlaying;
		public bool IsFinished => _isFinished;

		public event TimerStateHandler OnStarted;
		public event TimerStateHandler OnFinished;
		public event TimerStateHandler OnCanceled;
        public event TimerStateHandler OnEnded;
        public event TimerStateHandler OnPaused;
        public event TimerStateHandler OnResumed;

		public Timer()
        {
			Duration = 0.2f;
			_remainTime = Duration;
			_elapsedTime = 0;
			IsPlaying = false;
			_isFinished = false;
		}
		public Timer(float time, bool isPlaying = false)
        {
            Duration = time;
			_remainTime = Duration;
			_elapsedTime = 0;
			this.IsPlaying = isPlaying;
			_isFinished = false;
		}

		public void OnResetTimer()
        {
			_remainTime = Duration;
			_elapsedTime = 0;
			_isFinished = false;
			_isPaused = false;
		}

		public void Play(float duration = -1f)
        {
            if (IsPlaying)
                return;

			if (duration > 0)
                Duration = duration;

            IsPlaying = true;

            OnResetTimer();

            RaiseOnStarted();
        }

		public void JumpTime(float time)
        {
			if (time < 0)
				time = 0;

            _remainTime = Duration - time;
			_elapsedTime = time;
        }

        public void End()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;

            if (NormalizedTime < 1)
            {
                RaiseOnCanceled();
            }
            else
            {
                RaiseOnFinished();
            }

            RaiseOnEnded();
        }

		public void CancelTimer()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;
			_isFinished = false;

			RaiseOnCanceled();

			RaiseOnEnded();
		}
		public void PauseTimer()
        {
			if (!IsPlaying || _isPaused)
				return;

            _isPaused = true;

			RaiseOnPaused();
        }
		public void ResumeTimer()
        {
			if (!IsPlaying || !_isPaused)
				return;

            _isPaused = false;

			RaiseOnResumed();
		}

		public void EndTimer()
        {
			if (!IsPlaying)
				return;

            if (_remainTime <= 0f)
            {
				End();
			}
            else
            {
				CancelTimer();
			}
		}
		
		public void UpdateTimer(float deltaTime)
        {
			if (_isFinished || !IsPlaying || _isPaused)
				return;

			_remainTime = Mathf.Clamp(_remainTime - deltaTime, 0f, Duration);
			_elapsedTime = Duration - _remainTime;
			_normalizedTime = _elapsedTime.SafeDivide(Duration);

            if (_remainTime <= 0)
            {
				End();
			}
        }


        private void RaiseOnStarted()
        {
            Log($"{nameof(OnStarted)} :: Duration - {Duration:f0}");

            OnStarted?.Invoke(this);
        }
        private void RaiseOnFinished()
        {
            Log($"{nameof(OnFinished)}");

            OnFinished?.Invoke(this);
        }
        private void RaiseOnEnded()
        {
            Log($"{nameof(OnEnded)} :: {(IsFinished ? "FInished" : "Canceled" )}");

            OnEnded?.Invoke(this);
        }
        private void RaiseOnCanceled()
        {
            Log($"{nameof(OnCanceled)} :: Remain Time - {RemainTime:f0}");

            OnCanceled?.Invoke(this);
        }
        private void RaiseOnPaused()
        {
            Log($"{nameof(OnPaused)} :: Remain Time - {RemainTime:f0}");

            OnPaused?.Invoke(this);
        }
        private void RaiseOnResumed()
        {
            Log($"{nameof(OnResumed)} :: Remain Time - {RemainTime:f0}");

            OnResumed?.Invoke(this);
        }

    }

}
