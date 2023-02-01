using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class TimerComponent : MonoBehaviour
    {
		[SerializeField] private float _Duration = 1f;
        [SerializeField] private bool _AutoPlaying = false;
        [SerializeField] private EExitAction _ExitAction = EExitAction.Destroy;

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

        private void Update()
        {
			if(_Timer.IsPlaying)
				_Timer.UpdateTimer(Time.deltaTime);
        }
    }

}
