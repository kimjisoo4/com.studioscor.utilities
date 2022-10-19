using UnityEngine;

namespace KimScor.Utilities
{
    public interface IStiffen
    {
        public delegate void StiffenHandler(IStiffen stiffen);

        public event StiffenHandler OnFinishedStiffen;
        public event StiffenHandler OnStartedStiffen;
        public bool TryTakeStiffen(float duration);
        public bool CanStiffen(float duration);
        public void TakeStiffen(float duration);
    }

    public class StiffenComponent : MonoBehaviour, IStiffen
    {
        private Timer _Timer = new();

        public event IStiffen.StiffenHandler OnFinishedStiffen;
        public event IStiffen.StiffenHandler OnStartedStiffen;

        void Awake()
        {
            _Timer = new();

            _Timer.OnFinishedTimer += Timer_OnFinishedTimer;
        }

        private void Timer_OnFinishedTimer(Timer timer)
        {
            OnFinishedStiffen?.Invoke(this);
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
            return !_Timer.IsPlaying || _Timer.RemainTime < duration;
        }

        public void TakeStiffen(float duration)
        {
            _Timer.OnTimer(duration);

            OnStartedStiffen?.Invoke(this);
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;

            _Timer.UpdateTimer(deltaTime);
        }
    }
}