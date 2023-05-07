using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/Timer/Timer Component", order: 0)]
    public class TimerComponent : BaseMonoBehaviour
    {
        [Header("[ Timer Component ]")]
		[SerializeField] private float _Duration = 1f;
        [SerializeField] private EExitAction _ExitAction = EExitAction.Destroy;

        [Header(" [ Play Speed ] ")]
        [SerializeField] private float _PlaySpeed = 1f;

        [Header(" [ Auto Playing ] ")]
        [SerializeField] private bool _AutoPlaying = true;

		[SerializeField] private UnityEvent _OnStartedTimer;
		[SerializeField] private UnityEvent _OnFinishedTimer;
		[SerializeField] private UnityEvent _OnCanceledTimer;

        public event UnityAction OnStartedTimer;
        public event UnityAction OnFinishedTimer;
        public event UnityAction OnCanceledTimer;

        private readonly Timer _Timer = new();

        public float Duration => _Duration;
        public float NormalizedTime => _Timer.NormalizedTime;
        public float RemainTime => _Timer.RemainTime;
        public float ElaspedTime => _Timer.ElapsedTime;
        public bool IsFinished => _Timer.IsFinished;
        public bool IsPlaying => _Timer.IsPlaying;
        public bool IsStopped => _Timer.IsStopped;


        private void Awake()
        {
            _Timer.OnStartedTimer += Timer_OnStartedTimer;
            _Timer.OnFinishedTimer += Timer_OnFinishedTimer;
            _Timer.OnCanceledTimer += Timer_OnCanceledTimer;
        }

        private void OnEnable()
        {
            if (_AutoPlaying)
                OnTimer();
        }
        private void OnDisable()
        {
            EndTimer();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime * _PlaySpeed;

            UpdateTimer(deltaTime);
        }

        public void SetPlaySpeed(float newSpeed)
        {
            _PlaySpeed = newSpeed;
        }

        public void SetTimer(float duration)
        {
            _Duration = duration;
        }

        public void OnTimer()
        {
            Log($"On Timer - [ Duration : {_Duration:N2} ] ");
            
            _Timer.OnTimer(_Duration);
        }
        public void EndTimer()
        {
            Log($"End Timer - [ {(_Timer.IsFinished ? "Finished" : "Canceled")} ] ");

            _Timer.EndTimer();
        }
        public void OnPauseTimer()
        {
            Log($"On Pause Timer - [ RemainTime : {_Timer.RemainTime:N2} ] ");

            _Timer.OnPauseTimer();
        }
        public void OnResumeTimer()
        {
            Log($"On Resume Timer - [ RemainTime : {_Timer.RemainTime:N2} ] ");

            _Timer.OnResumeTimer();
        }

        public void UpdateTimer(float deltaTime)
        {
            _Timer.UpdateTimer(deltaTime);
        }
        private void Timer_OnCanceledTimer(Timer timer)
        {
            Log(" On Canceled Timer ");

            _OnCanceledTimer?.Invoke();
            OnCanceledTimer?.Invoke();
        }
        private void Timer_OnStartedTimer(Timer timer)
        {
            Log(" On Started Timer ");

            _OnStartedTimer?.Invoke();
            OnStartedTimer?.Invoke();
        }

        private void Timer_OnFinishedTimer(Timer timer)
        {
            Log($" On Finished Timer - [ ExieAction : {_ExitAction} ]");

            _OnFinishedTimer?.Invoke();
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
    }
}
