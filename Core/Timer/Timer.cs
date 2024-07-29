using UnityEngine;
using static StudioScor.Utilities.ITimer;

namespace StudioScor.Utilities
{
	public interface ITimer
	{
		public delegate void TimerStateHandler(ITimer timer);
		public void OnTimer(float newDuration = -1f);

		public void UpdateTimer(float deltaTime);
		public void JumpTime(float newTime);
		public void PauseTimer();
		public void ResumeTimer();
		public void FinishTimer();
		public void CancelTimer();

		public bool IsPlaying { get; }
		public bool IsStopped { get; }
		public bool IsPaused { get; }
		public bool IsFinished { get; }
		public float Duration { get; }
		public float RemainTime { get; }
		public float ElapsedTime { get; }
		public float NormalizedTime { get; }

        public event TimerStateHandler OnStartedTimer;
        public event TimerStateHandler OnFinishedTimer;
        public event TimerStateHandler OnCanceledTimer;
		public event TimerStateHandler OnEndedTimer;
		public event TimerStateHandler OnPausedTimer;
		public event TimerStateHandler OnResumedTimer;
    }

    [System.Serializable]
    public class Timer : BaseClass, ITimer
    {
		[SerializeField] private float _duration;
		
		private float _remainTime;
		private float _elapsedTime;
		private float _normalizedTime;

		private bool _isPlaying;
		private bool _isPaused;
		private bool _isFinished;

		public event TimerStateHandler OnStartedTimer;
		public event TimerStateHandler OnFinishedTimer;
		public event TimerStateHandler OnCanceledTimer;
        public event TimerStateHandler OnEndedTimer;
        public event TimerStateHandler OnPausedTimer;
        public event TimerStateHandler OnResumedTimer;

        public float Duration => _duration;
		public float RemainTime => IsPlaying ? _remainTime : 0f;
		public float ElapsedTime => _isPlaying ?  _elapsedTime : 0f;
		public float NormalizedTime => _normalizedTime;
		public bool IsPlaying => _isPlaying;
		public bool IsPaused => _isPlaying && _isPaused;
		public bool IsStopped => !_isPlaying;
		public bool IsFinished => _isFinished;

		public Timer()
        {
			_duration = 0.2f;
			_remainTime = _duration;
			_elapsedTime = 0;
			_isPlaying = false;
			_isFinished = false;
		}
		public Timer(float time, bool isPlaying = false)
        {
			_duration = time;
			_remainTime = _duration;
			_elapsedTime = 0;
			this._isPlaying = isPlaying;
			_isFinished = false;
		}

		public void OnResetTimer()
        {
			_remainTime = _duration;
			_elapsedTime = 0;
			_isFinished = false;
			_isPaused = false;
		}

		public void OnTimer(float duration = -1f)
        {
            if (IsPlaying)
                return;

			if (duration > 0)
				_duration = duration;

            _isPlaying = true;

            OnResetTimer();

            Invoke_OnStartedTimer();
        }

		public void SetDuration(float duration)
        {
			this._duration = duration;
        }
		public void JumpTime(float time)
        {
			if (time < 0)
				time = 0;

            _remainTime = _duration - time;
			_elapsedTime = time;
        }

		public void FinishTimer()
		{
			if (!_isPlaying)
				return;

			_isPlaying = false;
			_remainTime = 0;
			_elapsedTime = _duration;
			_isFinished = true;
			_normalizedTime = 1;

			Invoke_OnFinishedTimer();

			Invoke_OnEndedTimer();
		}
		public void CancelTimer()
        {
            if (!_isPlaying)
                return;

            _isPlaying = false;
			_isFinished = false;

			Invoke_OnCanceledTimer();

			Invoke_OnEndedTimer();
		}
		public void PauseTimer()
        {
			if (!_isPlaying || _isPaused)
				return;

            _isPaused = true;

			Invoke_OnPausedTimer();
        }
		public void ResumeTimer()
        {
			if (!_isPlaying || !_isPaused)
				return;

            _isPaused = false;

			Invoke_OnResumedTimer();
		}

		public void EndTimer()
        {
			if (!_isPlaying)
				return;

            if (_remainTime <= 0f)
            {
				FinishTimer();
			}
            else
            {
				CancelTimer();
			}
		}
		
		public void UpdateTimer(float deltaTime)
        {
			if (_isFinished || !_isPlaying || _isPaused)
				return;

			_remainTime -= deltaTime;
			_elapsedTime = _duration - _remainTime;

            if (_remainTime <= 0)
            {
				FinishTimer();

				return;
			}

			_normalizedTime = _elapsedTime / _duration;
        }


        private void Invoke_OnStartedTimer()
        {
            Log($"{nameof(OnStartedTimer)} :: Duration - {Duration:f0}");

            OnStartedTimer?.Invoke(this);
        }
        private void Invoke_OnFinishedTimer()
        {
            Log($"{nameof(OnFinishedTimer)}");

            OnFinishedTimer?.Invoke(this);
        }
        private void Invoke_OnEndedTimer()
        {
            Log($"{nameof(OnEndedTimer)} :: {(IsFinished ? "FInished" : "Canceled" )}");

            OnEndedTimer?.Invoke(this);
        }
        private void Invoke_OnCanceledTimer()
        {
            Log($"{nameof(OnCanceledTimer)} :: Remain Time - {RemainTime:f0}");

            OnCanceledTimer?.Invoke(this);
        }
        private void Invoke_OnPausedTimer()
        {
            Log($"{nameof(OnPausedTimer)} :: Remain Time - {RemainTime:f0}");

            OnPausedTimer?.Invoke(this);
        }
        private void Invoke_OnResumedTimer()
        {
            Log($"{nameof(OnResumedTimer)} :: Remain Time - {RemainTime:f0}");

            OnResumedTimer?.Invoke(this);
        }

    }

}
