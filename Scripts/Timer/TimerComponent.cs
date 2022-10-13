using UnityEngine;
using UnityEngine.Events;

namespace KimScor.Utilities
{
    public class TimerComponent : MonoBehaviour
    {
		[SerializeField] private float _Duration;
        [SerializeField] private bool _AutoPlaying = false;
        [SerializeField] private bool _AutoDestory = false;

		public UnityEvent OnStartedTimer;
		public UnityEvent OnFinishedTimer;
		public UnityEvent OnCanceledTimer;

		private Timer _Timer;

        private void Awake()
        {
			_Timer = new(_Duration);
        }
        private void OnEnable()
        {
            if (_AutoPlaying)
            {
				_Timer.OnTimer();
            }
            if (_AutoDestory)
            {
                _Timer.OnFinishedTimer += Timer_OnFinishedTimer;
            }
        }

        private void Timer_OnFinishedTimer(Timer timer)
        {
            _Timer.OnFinishedTimer -= Timer_OnFinishedTimer;
            Destroy(gameObject, Time.deltaTime);
        }

        private void Update()
        {
			if(_Timer.IsPlaying)
				_Timer.UpdateTimer(Time.deltaTime);
        }
    }

}
