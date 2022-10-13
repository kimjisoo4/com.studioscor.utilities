﻿namespace KimScor.Utilities
{
    public class Timer
    {
		public delegate void TimerHandler(Timer timer);

		private float _Duration;
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
			OnResetTimer();

			_IsPlaying = true;

			OnStartedTimer?.Invoke(this);
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
		public void OnStopTimer()
        {
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
