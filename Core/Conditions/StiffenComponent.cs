using UnityEngine;

namespace StudioScor.Utilities
{

    public interface IStiffenSystem
    {
        public delegate void StiffenHandler(IStiffenSystem stiffen);

        public bool IsPlaying { get; }
        public bool IsStiffen { get; }
        public float RemainTime { get; }

        public bool TryTakeStiffen(float duration);
        public bool CanStiffen();
        public void TakeStiffen(float duration);
        public void CancelStiffen();

        public void Tick(float deltaTime);

        public void OnStiffen();
        public void EndStiffen();


        public event StiffenHandler OnStartedStiffen;
        public event StiffenHandler OnTriggeredStiffen;
        public event StiffenHandler OnFinishedStiffen;
    }
    

    public class StiffenComponent : BaseMonoBehaviour, IStiffenSystem
    {
        [Header(" [ Stiffen Component ] ")]
        [SerializeField] private bool _autoPlaying = true;

        private float _remainTime;
        private bool _isStiffen;
        private bool _isPlaying;

        public bool IsPlaying => _isPlaying;
        public bool IsStiffen => _isStiffen;
        public float RemainTime => _remainTime;

        public event IStiffenSystem.StiffenHandler OnFinishedStiffen;
        public event IStiffenSystem.StiffenHandler OnStartedStiffen;
        public event IStiffenSystem.StiffenHandler OnTriggeredStiffen;


        private void OnEnable()
        {
            if (_autoPlaying)
                OnStiffen();
        }
        public void OnStiffen()
        {
            if (IsPlaying)
                return;

            _isPlaying = true;
        }
        public void EndStiffen()
        {
            if (!IsPlaying)
                return;

            _isPlaying = false;

            CancelStiffen();
        }
        public bool TryTakeStiffen(float duration)
        {
            if (!CanStiffen())
                return false;

            TakeStiffen(duration);

            return true;
        }

        public virtual bool CanStiffen()
        {
            return _isPlaying;
        }

        public void TakeStiffen(float duration)
        {
            if (!_isStiffen)
            {
                _isStiffen = true;
                _remainTime = duration;

                Invoke_OnStartedStiffen();
            }
            else if(_remainTime < duration)
            {
                _remainTime = duration;
            }

            Invoke_OnTriggeredStiffen();
        }

        public void CancelStiffen()
        {
            if (!_isStiffen)
                return;

            _remainTime = 0f;
            _isStiffen = false;

            Invoke_OnFinishedStiffen();
        }

        public override void Tick(float deltaTime)
        {
            if (!IsPlaying || !_isStiffen)
                return;

            _remainTime -= deltaTime;

            if(_remainTime <= 0f)
            {
                CancelStiffen();
            }
        }

        #region Callback
        private void Invoke_OnStartedStiffen()
        {
            Log($"{nameof(OnStartedStiffen)}");

            OnStartedStiffen?.Invoke(this);
        }
        private void Invoke_OnTriggeredStiffen()
        {
            Log($"{nameof(OnTriggeredStiffen)}");

            OnTriggeredStiffen?.Invoke(this);
        }
        private void Invoke_OnFinishedStiffen()
        {
            Log($"{nameof(OnFinishedStiffen)}");

            OnFinishedStiffen?.Invoke(this);
        }
        #endregion
    }
}