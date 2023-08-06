using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/Timer/Timer Component", order: 0)]
    public class TimerComponent : BaseMonoBehaviour
    {
        [Header("[ Timer Component ]")]
        [SerializeField] private Timer timer;
        [SerializeField] private EExitAction exitAction = EExitAction.Destroy;

        [Header(" [ Play Speed ] ")]
        [SerializeField] private float playSpeed = 1f;

        [Header(" [ Auto Playing ] ")]
        [SerializeField] private bool isAutoPlaying = true;

        [Header(" [ Events ] ")]
		[SerializeField] private UnityEvent onStartedTimer;
		[SerializeField] private UnityEvent onFinishedTimer;
		[SerializeField] private UnityEvent onCanceledTimer;

        public Timer Timer => timer;


        private void Awake()
        {
            timer.OnStartedTimer += Timer_OnStartedTimer;
            timer.OnFinishedTimer += Timer_OnFinishedTimer;
            timer.OnCanceledTimer += Timer_OnCanceledTimer;
        }

        private void OnEnable()
        {
            if (isAutoPlaying)
                OnTimer();
        }

        private void OnDisable()
        {
            EndTimer();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime * playSpeed;

            UpdateTimer(deltaTime);
        }

        public void SetPlaySpeed(float newSpeed)
        {
            playSpeed = newSpeed;
        }

        public void SetTimer(float duration)
        {
            timer.SetDuration(duration);
        }

        public void OnTimer()
        {
            Log($"On Timer - [ Duration : {timer.Duration:N2} ] ");
            
            timer.OnTimer(timer.Duration);
        }
        public void EndTimer()
        {
            Log($"End Timer - [ {(timer.IsFinished ? "Finished" : "Canceled")} ] ");

            timer.EndTimer();
        }
        public void OnPauseTimer()
        {
            Log($"On Pause Timer - [ RemainTime : {timer.RemainTime:N2} ] ");

            timer.OnPauseTimer();
        }
        public void OnResumeTimer()
        {
            Log($"On Resume Timer - [ RemainTime : {timer.RemainTime:N2} ] ");

            timer.OnResumeTimer();
        }

        public void UpdateTimer(float deltaTime)
        {
            timer.UpdateTimer(deltaTime);
        }

        private void Timer_OnStartedTimer(Timer timer)
        {
            Log(" On Started Timer ");

            onStartedTimer?.Invoke();
        }

        private void Timer_OnCanceledTimer(Timer timer)
        {
            Log(" On Canceled Timer ");

            onCanceledTimer?.Invoke();
        }
        

        private void Timer_OnFinishedTimer(Timer timer)
        {
            Log($" On Finished Timer - [ ExieAction : {exitAction} ]");

            onFinishedTimer?.Invoke();

            switch (exitAction)
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
