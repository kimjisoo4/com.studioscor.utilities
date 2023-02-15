using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class TimerComponent : BaseMonoBehaviour
    {
        [Header("[ Timer Component ]")]
		[SerializeField] private float _Duration = 1f;
        [SerializeField] private float _Multiply = 1f;
        [SerializeField] private bool _AutoPlaying = false;
        [SerializeField] private bool _UseManul = false;
        [SerializeField] private EExitAction _ExitAction = EExitAction.Destroy;

		public UnityEvent OnStartedTimer;
		public UnityEvent OnFinishedTimer;
		public UnityEvent OnCanceledTimer;

		private Timer _Timer;

        public float Duration => _Duration;
        public float NormalizedTime => _Timer.NormalizedTime;
        public float RemainTime => _Timer.RemainTime;
        public float ElaspedTime => _Timer.ElapsedTime;

        private void Awake()
        {
			_Timer = new();

            _Timer.OnStartedTimer += Timer_OnStartedTimer;
            _Timer.OnFinishedTimer += Timer_OnFinishedTimer;
            _Timer.OnCanceledTimer += Timer_OnCanceledTimer;
        }

        private void Timer_OnCanceledTimer(Timer timer)
        {
            OnCanceledTimer?.Invoke();
        }

        private void Timer_OnStartedTimer(Timer timer)
        {
            OnStartedTimer?.Invoke();
        }

        private void Start()
        {
            if (_AutoPlaying)
                OnTimer();
        }

        private void Timer_OnFinishedTimer(Timer timer)
        {
            OnFinishedTimer?.Invoke();

            switch (_ExitAction)
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

        public void SetTimer(float duration)
        {
            _Duration = duration;
        }
        public void OnTimer()
        {
            _Timer.OnTimer(_Duration);
        }
        public void StopTimer()
        {
            _Timer.OnStopTimer();
        }
        public void OnPauseTimer()
        {
            _Timer.OnPauseTimer();
        }
        public void OnResumeTimer()
        {
            _Timer.OnResumeTimer();
        }

        private void Update()
        {
            _Timer.UpdateTimer(Time.deltaTime * _Multiply);
        }
    }

}
