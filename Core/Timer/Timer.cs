using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class Timer : BaseClass
    {
		public delegate void TimerHandler(Timer timer);

		[SerializeField] private float duration;
		
		private float remainTime;
		private float elapsedTime;
		private float normalizedTime;

		private bool isPlaying;
		private bool isFinished;

		public event TimerHandler OnStartedTimer;
		public event TimerHandler OnFinishedTimer;
		public event TimerHandler OnCanceledTimer;

		public float Duration => duration;
		public float RemainTime => IsPlaying ? remainTime : 0f;
		public float ElapsedTime => isPlaying ?  elapsedTime : 0f;
		public float NormalizedTime => normalizedTime;
		public bool IsPlaying => isPlaying;
		public bool IsStopped => !isPlaying;
		public bool IsFinished => isFinished;

		public Timer()
        {
			duration = 0.2f;
			remainTime = duration;
			elapsedTime = 0;
			isPlaying = false;
			isFinished = false;
		}

		public Timer(float time, bool isPlaying = false)
        {
			duration = time;
			remainTime = duration;
			elapsedTime = 0;
			this.isPlaying = isPlaying;
			isFinished = false;
		}
		public void OnResetTimer()
        {
			remainTime = duration;
			elapsedTime = 0;
			isFinished = false;
		}
		public void OnTimer(float duration)
        {
			this.duration = duration;

			OnTimer();
        }
		public void OnTimer()
        {
			if (IsPlaying)
				return;

			isPlaying = true;
			
			OnResetTimer();

			Invoke_OnStartedTimer();
		}

		public void SetDuration(float duration)
        {
			this.duration = duration;
        }
		public void JumpTimer(float time)
        {
			remainTime -= time;
			elapsedTime = time;
        }

		public void OnFinisheTimer()
		{
			isPlaying = false;
			remainTime = 0;
			elapsedTime = duration;
			isFinished = true;
			normalizedTime = 1;

			Invoke_OnFinishedTimer();
		}
		private void OnCancelTimer()
        {
			isPlaying = false;
			isFinished = false;

			Invoke_OnCanceledTimer();
		}
		public void OnPauseTimer()
        {
			if (!isPlaying)
				return;

			isPlaying = false;
        }
		public void OnResumeTimer()
        {
			if (isPlaying)
				return;

			isPlaying = true;
		}

		public void EndTimer()
        {
			if (!isPlaying)
				return;

            if (remainTime <= 0f)
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
			if (isFinished || !isPlaying)
				return;

			remainTime -= deltaTime;
			elapsedTime = duration - remainTime;

            if (remainTime <= 0)
            {
				OnFinisheTimer();

				return;
			}

			normalizedTime = elapsedTime / duration;
        }


        private void Invoke_OnStartedTimer()
        {
            Log("On Started Timer");

            OnStartedTimer?.Invoke(this);
        }
        private void Invoke_OnFinishedTimer()
        {
            Log("On Finished Timer");

            OnFinishedTimer?.Invoke(this);
        }
        private void Invoke_OnCanceledTimer()
        {
            Log("On Canceled Timer");

            OnCanceledTimer?.Invoke(this);
        }
    }

}
