using UnityEngine;

namespace StudioScor.Utilities
{

    public interface IStiffen
    {
        public delegate void StiffenHandler(IStiffen stiffen);

        public bool IsStiffen { get; }
        public float RemainTime { get; }

        public event StiffenHandler OnFinishedStiffen;
        public event StiffenHandler OnStartedStiffen;
        public bool TryTakeStiffen(float duration);
        public bool CanStiffen(float duration);
        public void TakeStiffen(float duration);
    }
    

    public class StiffenComponent : BaseMonoBehaviour, IStiffen
    {
        private readonly Timer timer = new();

        public bool IsStiffen => timer.IsPlaying;
        public float RemainTime => timer.RemainTime;

        public event IStiffen.StiffenHandler OnFinishedStiffen;
        public event IStiffen.StiffenHandler OnStartedStiffen;

        void Awake()
        {
            timer.OnFinishedTimer += Timer_OnFinishedTimer;
            timer.OnCanceledTimer += Timer_OnFinishedTimer;
        }

        private void Timer_OnFinishedTimer(ITimer timer)
        {
            Invoke_OnFinishedStiffen();
        }

        public bool TryTakeStiffen(float duration)
        {
            if (!CanStiffen(duration))
                return false;

            TakeStiffen(duration);

            return true;
        }

        public bool CanStiffen(float duration)
        {
            return !timer.IsPlaying || timer.RemainTime < duration;
        }

        public void TakeStiffen(float duration)
        {
            timer.OnTimer(duration);

            Invoke_OnStartedStiffen();
        }

        public void EndStiffen()
        {
            timer.EndTimer();
        }

        void Update()
        {
            if (!timer.IsPlaying)
                return;

            float deltaTime = Time.deltaTime;

            timer.UpdateTimer(deltaTime);
        }

        #region Callback
        private void Invoke_OnStartedStiffen()
        {
            Log("On Started Stiffen ");

            OnStartedStiffen?.Invoke(this);
        }
        private void Invoke_OnFinishedStiffen()
        {
            Log("On Finished Stiffen ");

            OnFinishedStiffen?.Invoke(this);
        }
        #endregion
    }
}