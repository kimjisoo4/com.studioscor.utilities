using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/Timer/Timer Component", order: 0)]
    public class TimerComponent : BaseMonoBehaviour, ITimer
    {
        [System.Serializable]
        public class UnityEvents
        {
            [SerializeField] private UnityEvent _onStartedTimer;
            [SerializeField] private UnityEvent _onFinishedTimer;
            [SerializeField] private UnityEvent _onCanceledTimer;
            [SerializeField] private UnityEvent _onEndedTimer;
            [SerializeField] private UnityEvent _onPausedTimer;
            [SerializeField] private UnityEvent _onResumedTimer;
            public void AddUnityEvent(ITimer timer)
            {
                timer.OnStarted += Timer_OnStartedTimer;
                timer.OnCanceled += Timer_OnCanceledTimer;
                timer.OnFinished += Timer_OnFinishedTimer;
                timer.OnEnded += Timer_OnEndedTimer;
                timer.OnPaused += Timer_OnPausedTimer;
                timer.OnResumed += Timer_OnResumedTimer;
            }
            public void RemoveUnityEvent(ITimer timer)
            {
                timer.OnStarted -= Timer_OnStartedTimer;
                timer.OnCanceled -= Timer_OnCanceledTimer;
                timer.OnFinished -= Timer_OnFinishedTimer;
                timer.OnEnded -= Timer_OnEndedTimer;
                timer.OnPaused -= Timer_OnPausedTimer;
                timer.OnResumed -= Timer_OnResumedTimer;
            }

            private void Timer_OnResumedTimer(ITimer timer)
            {
                _onResumedTimer?.Invoke();
            }

            private void Timer_OnPausedTimer(ITimer timer)
            {
                _onPausedTimer?.Invoke();
            }

            private void Timer_OnEndedTimer(ITimer timer)
            {
                _onEndedTimer?.Invoke();
            }

            private void Timer_OnFinishedTimer(ITimer timer)
            {
                _onFinishedTimer?.Invoke();
            }

            private void Timer_OnCanceledTimer(ITimer timer)
            {
                _onCanceledTimer?.Invoke();
            }

            private void Timer_OnStartedTimer(ITimer timer)
            {
                _onStartedTimer?.Invoke();
            }

           
        }

        [Header("[ Timer Component ]")]
        [SerializeField] private Timer _timer;
        [SerializeField] private EExitAction _exitAction = EExitAction.Destroy;
        [SerializeField] private bool _playExitActionInCancel = false;

        [Header(" [ Auto Playing ] ")]
        [SerializeField] private bool isAutoPlaying = true;

        [Header(" Unity Events ")]
        [SerializeField] private bool _useUnityEvent = true;
        [SerializeField] private UnityEvents _unityEvents;

        public bool IsPlaying => _timer.IsPlaying;
        public bool IsStopped => _timer.IsStopped;
        public bool IsPaused => _timer.IsPaused;
        public bool IsFinished => _timer.IsFinished;
        public float Duration => _timer.Duration;
        public float RemainTime => _timer.RemainTime;
        public float ElapsedTime => _timer.ElapsedTime;
        public float NormalizedTime => _timer.NormalizedTime;

        
        private bool _wasInitialized = false;


        public event ITimer.TimerStateHandler OnStarted { add => _timer.OnStarted += value; remove => _timer.OnStarted -= value; }
        public event ITimer.TimerStateHandler OnFinished { add => _timer.OnFinished += value; remove => _timer.OnFinished -= value; }
        public event ITimer.TimerStateHandler OnCanceled { add => _timer.OnCanceled += value; remove => _timer.OnCanceled -= value; }
        public event ITimer.TimerStateHandler OnEnded { add => _timer.OnEnded += value; remove => _timer.OnEnded -= value; }
        public event ITimer.TimerStateHandler OnPaused { add => _timer.OnPaused += value; remove => _timer.OnPaused -= value; }
        public event ITimer.TimerStateHandler OnResumed { add => _timer.OnResumed += value; remove => _timer.OnResumed -= value; }

        private void Awake()
        {
            Initialization();
        }
        private void OnDestroy()
        {
            if (!_wasInitialized)
                return;

            _timer.OnEnded -= _timer_OnEndedTimer;

            if (_useUnityEvent)
            {
                _unityEvents.RemoveUnityEvent(this);
            }
        }
        private void OnEnable()
        {
            if (isAutoPlaying)
                Play();
        }

        private void Initialization()
        {
            if (_wasInitialized)
                return;

            _wasInitialized = true;
            _timer.OnEnded += _timer_OnEndedTimer;

            if (_useUnityEvent)
            {
                _unityEvents.AddUnityEvent(this);
            }
        }
        
        public void Play(float newDuration = -1) => _timer.Play(newDuration);
        public void EndTimer() => _timer.EndTimer();
        public void UpdateTimer(float deltaTime) => _timer.UpdateTimer(deltaTime);
        public void JumpTime(float newTime) => _timer.JumpTime(newTime);
        public void PauseTimer() => _timer.PauseTimer();

        public void ResumeTimer() => _timer.ResumeTimer();

        public void End() => _timer.End();

        public void CancelTimer() => _timer.CancelTimer();

        private void _timer_OnEndedTimer(ITimer timer)
        {
            if (_exitAction != EExitAction.None)
                return;

            if (!_playExitActionInCancel && !timer.IsFinished)
                return;

            switch (_exitAction)
            {
                case EExitAction.None:
                    break;
                case EExitAction.Disable:
                    gameObject.SetActive(false);
                    break;
                case EExitAction.Destroy:
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
