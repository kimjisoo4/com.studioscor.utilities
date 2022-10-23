using UnityEngine;
using UnityEngine.Events;

namespace KimScor.Utilities
{
    public class TimerComponent : MonoBehaviour
    {
        public enum EFinishedActionState
        {
            None,
            Destroy,
            SetActive,
        }
		[SerializeField] private float _Duration;
        [SerializeField] private bool _AutoPlaying = false;
        [SerializeField] private EFinishedActionState FinishedAction = EFinishedActionState.Destroy;

		public UnityEvent OnStartedTimer;
		public UnityEvent OnFinishedTimer;
		public UnityEvent OnCanceledTimer;

		private Timer _Timer;

        private void Awake()
        {
			_Timer = new(_Duration);

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

        private void OnEnable()
        {
            if (_AutoPlaying)
            {
				_Timer.OnTimer();
            }
        }

        private void Timer_OnFinishedTimer(Timer timer)
        {
            OnFinishedTimer?.Invoke();

            switch (FinishedAction)
            {
                case EFinishedActionState.None:
                    break;
                case EFinishedActionState.Destroy:
                    _Timer.OnFinishedTimer -= Timer_OnFinishedTimer;
                    Destroy(gameObject, Time.deltaTime);
                    break;
                case EFinishedActionState.SetActive:
                    gameObject.SetActive(false);
                    break;  
                default:
                    break;
            }
        }

        private void Update()
        {
			if(_Timer.IsPlaying)
				_Timer.UpdateTimer(Time.deltaTime);
        }
    }

}
