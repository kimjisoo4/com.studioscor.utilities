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
                timer.OnStartedTimer += Timer_OnStartedTimer;
                timer.OnCanceledTimer += Timer_OnCanceledTimer;
                timer.OnFinishedTimer += Timer_OnFinishedTimer;
                timer.OnEndedTimer += Timer_OnEndedTimer;
                timer.OnPausedTimer += Timer_OnPausedTimer;
                timer.OnResumedTimer += Timer_OnResumedTimer;
            }
            public void RemoveUnityEvent(ITimer timer)
            {
                timer.OnStartedTimer -= Timer_OnStartedTimer;
                timer.OnCanceledTimer -= Timer_OnCanceledTimer;
                timer.OnFinishedTimer -= Timer_OnFinishedTimer;
                timer.OnEndedTimer -= Timer_OnEndedTimer;
                timer.OnPausedTimer -= Timer_OnPausedTimer;
                timer.OnResumedTimer -= Timer_OnResumedTimer;
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
        [SerializeField][SEnumCondition(nameof(_exitAction), (int)EExitAction.None)] private bool _playExitActionInCancel = false;

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


        public event ITimer.TimerStateHandler OnStartedTimer { add => _timer.OnStartedTimer += value; remove => _timer.OnStartedTimer -= value; }
        public event ITimer.TimerStateHandler OnFinishedTimer { add => _timer.OnFinishedTimer += value; remove => _timer.OnFinishedTimer -= value; }
        public event ITimer.TimerStateHandler OnCanceledTimer { add => _timer.OnCanceledTimer += value; remove => _timer.OnCanceledTimer -= value; }
        public event ITimer.TimerStateHandler OnEndedTimer { add => _timer.OnEndedTimer += value; remove => _timer.OnEndedTimer -= value; }
        public event ITimer.TimerStateHandler OnPausedTimer { add => _timer.OnPausedTimer += value; remove => _timer.OnPausedTimer -= value; }
        public event ITimer.TimerStateHandler OnResumedTimer { add => _timer.OnResumedTimer += value; remove => _timer.OnResumedTimer -= value; }

        private void Awake()
        {
            Initialization();
        }
        private void OnDestroy()
        {
            if (!_wasInitialized)
                return;

            _timer.OnEndedTimer -= _timer_OnEndedTimer;

            if (_useUnityEvent)
            {
                _unityEvents.RemoveUnityEvent(this);
            }
        }
        private void OnEnable()
        {
            if (isAutoPlaying)
                OnTimer();
        }

        private void Initialization()
        {
            if (_wasInitialized)
                return;

            _wasInitialized = true;
            _timer.OnEndedTimer += _timer_OnEndedTimer;

            if (_useUnityEvent)
            {
                _unityEvents.AddUnityEvent(this);
            }
        }
        
        public void OnTimer(float newDuration = -1) => _timer.OnTimer(newDuration);
        public void EndTimer() => _timer.EndTimer();
        public void UpdateTimer(float deltaTime) => _timer.UpdateTimer(deltaTime);
        public void JumpTime(float newTime) => _timer.JumpTime(newTime);
        public void PauseTimer() => _timer.PauseTimer();

        public void ResumeTimer() => _timer.ResumeTimer();

        public void FinishTimer() => _timer.FinishTimer();

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
