using UnityEngine;


namespace StudioScor.Utilities.FadeSystem
{
    public interface IFadeSystem
    {
        public delegate void FadeSystemEventHandler(IFadeSystem fadeSystem);
        public delegate void FadeSystemUpdateAmountEventHandler(IFadeSystem fadeSystem, float amount);

        /// <summary>
        /// Fade를 할 때까지 걸리는 시간
        /// </summary>
        public float Duration { get; }

        /// <summary>
        /// Fade 의 정도 ( 0 ~ 1 )
        /// </summary>
        public float Amount { get; }

        /// <summary>
        /// Fade 가 진행중인가의 여부
        /// </summary>
        public bool IsFading { get; }

        /// <summary>
        /// 현재 Fade 의 상태
        /// </summary>
        public EFadeState State { get; }

        /// <summary>
        /// 서서히 화면을 보이게 함
        /// </summary>
        /// <param name="duration"> 화면이 보이게 되기까지 걸리는 시간</param>
        /// <param name="playFromStart"> True 일 경우, 화면을 가리고 시작</param>
        public void StartFadeIn(float duration, bool playFromStart = false);

        /// <summary>
        /// 서서히 화면을 가리게 함.
        /// </summary>
        /// <param name="duration"> 화면이 안보이게 되기까지 걸리는 시간</param>
        /// <param name="playFromStart"> True 일 경우, 화면을 보이고 시작</param>
        public void StartFadeOut(float duration, bool playFromStart = false);

        /// <summary>
        /// 상태를 업데이트.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void UpdateFading(float deltaTime);

        /// <summary>
        /// FadeIn 이 진행중일 경우, 즉시 화면을 보이게함.
        /// </summary>
        public void EndFadeIn();

        /// <summary>
        /// FadeOut 이 진행중일 경우, 즉시 화면을 안보이게함.
        /// </summary>
        public void EndFadeOut();


        public event FadeSystemEventHandler OnFadeInStarted;
        public event FadeSystemEventHandler OnFadeOutStarted;
        public event FadeSystemEventHandler OnFadeInFinished;
        public event FadeSystemEventHandler OnFadeOutFinished;
        public event FadeSystemEventHandler OnFadingStarted;
        public event FadeSystemEventHandler OnFadingFinished;

        public event FadeSystemUpdateAmountEventHandler OnAmountUpdated;
    }

    public class FadeSystemComponent : BaseMonoBehaviour, IFadeSystem
    {
        [Header(" [ Fade System Component ] ")]
        [SerializeField] private EFadeState _state = EFadeState.FadeOut;
        [SerializeField] private float _duration = 1f;

        [Header(" Events ")]
        [SerializeField] private ToggleableUnityEvent _onFadeInStarted;
        [SerializeField] private ToggleableUnityEvent _onFadeInFinished;
        [SerializeField] private ToggleableUnityEvent _onFadeOutStarted;
        [SerializeField] private ToggleableUnityEvent _onFadeOutFinished;
        [SerializeField] private ToggleableUnityEvent _onFadingStarted;
        [SerializeField] private ToggleableUnityEvent _onFadingFinished;

        private bool _isFading;
        private float _amount;
        private float _elapsedTime;

        public float Duration => _duration;
        public float Amount
        {
            get
            {
                return _amount;
            }
            private set
            {
                value = Mathf.Clamp01(value);

                if (_amount.SafeEquals(value))
                    return;

                _amount = value;

                RaiseOnAmountUpdated();
            }
        }
        public bool IsFading => _isFading;
        public EFadeState State => _state;

        public event IFadeSystem.FadeSystemEventHandler OnFadeInStarted;
        public event IFadeSystem.FadeSystemEventHandler OnFadeOutStarted;
        public event IFadeSystem.FadeSystemEventHandler OnFadeInFinished;
        public event IFadeSystem.FadeSystemEventHandler OnFadeOutFinished;
        public event IFadeSystem.FadeSystemEventHandler OnFadingStarted;
        public event IFadeSystem.FadeSystemEventHandler OnFadingFinished;
        public event IFadeSystem.FadeSystemUpdateAmountEventHandler OnAmountUpdated;

        private void Awake()
        {
            switch (_state)
            {
                case EFadeState.None:
                    enabled = false;
                    break;
                case EFadeState.FadeIn:
                    StartFadeIn(_duration, true);
                    break;
                case EFadeState.FadeOut:
                    StartFadeOut(_duration, true);
                    break;
            }
        }

        private void OnDestroy()
        {
            OnFadeInStarted = null;
            OnFadeOutStarted = null;
            OnFadeInFinished = null;
            OnFadeOutFinished = null;
            OnFadingStarted = null;
            OnFadingFinished = null;
            OnAmountUpdated = null;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            UpdateFading(deltaTime);
        }


        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [ContextMenu(nameof(StartFadeIn), false, 1000000)]
        internal void EDITOR_StartFadeIn()
        {
            StartFadeIn(_duration);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [ContextMenu(nameof(StartFadeOut), false, 1000000)]
        internal void EDITOR_StartFadeOut()
        {
            StartFadeOut(_duration);
        }

        public void StartFadeIn(float duration, bool playFromStart = false)
        {
            _duration = Mathf.Max(0, duration);
            if(playFromStart)
            {
                Amount = 1f;
            }

            _elapsedTime = _duration * (1 - Amount);
            _state = EFadeState.FadeIn;
            _isFading = true;

            OnStartFadeIn();
            RaiseOnFadeInStarted();

            OnStartFading();
            RaiseOnFadingStarted();

            enabled = true;

        }

        public void StartFadeOut(float duration, bool playFromStart = false)
        {
            _duration = Mathf.Max(0, duration);
            if (playFromStart)
            {
                Amount = 0f;
            }

            _elapsedTime = _duration * Amount;
            _state = EFadeState.FadeOut;
            _isFading = true;
            
            OnStartFadeOut();
            RaiseOnFadeOutStarted();

            OnStartFading();
            RaiseOnFadingStarted();

            enabled = true;
        }

        public void UpdateFading(float deltaTime)
        {
            if(_duration <= 0f)
            {
                switch (_state)
                {
                    case EFadeState.FadeIn:
                        EndFadeIn();
                        break;
                    case EFadeState.FadeOut:
                        EndFadeOut();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                _elapsedTime += deltaTime;

                float normalizedTime = _elapsedTime.SafeDivide(_duration);

                switch (_state)
                {
                    case EFadeState.FadeIn:
                        Amount = 1 - normalizedTime;

                        if (Amount.SafeEquals(0f))
                            EndFadeIn();
                        break;
                    case EFadeState.FadeOut:
                        Amount = normalizedTime;

                        if (Amount.SafeEquals(1f))
                            EndFadeOut();
                        break;
                    default:
                        break;
                }
            }
        }



        public void EndFadeIn()
        {
            if (!_isFading)
                return;

            if (_state != EFadeState.FadeIn)
                return;

            enabled = false;
            _isFading = false;
            Amount = 0f;

            OnEndFading();
            RaiseOnFadingFinished();

            OnEndFadeIn();
            RaiseOnFadeInFinished();
        }
        public void EndFadeOut()
        {
            if (!_isFading)
                return;

            if (_state != EFadeState.FadeOut)
                return;

            enabled = false;
            _isFading = false;
            Amount = 1f;

            OnEndFading();
            RaiseOnFadingFinished();

            OnEndFadeOut();
            RaiseOnFadeOutFinished();
        }

        protected virtual void OnStartFadeIn() { }
        protected virtual void OnStartFadeOut() { }
        protected virtual void OnEndFadeIn() { }
        protected virtual void OnEndFadeOut() { }

        protected virtual void OnStartFading() { }
        protected virtual void OnEndFading() { }

        protected void RaiseOnAmountUpdated()
        {
            Log($"{nameof(OnAmountUpdated)} - Amount : {_amount}");

            OnAmountUpdated?.Invoke(this, _amount);
        }

        protected void RaiseOnFadeInStarted()
        {
            Log(nameof(OnFadeInStarted));

            _onFadeInStarted.Invoke();
            OnFadeInStarted?.Invoke(this);
        }
        protected void RaiseOnFadeOutStarted()
        {
            Log(nameof(OnFadeOutStarted));

            _onFadeOutStarted.Invoke();
            OnFadeOutStarted?.Invoke(this);
        }
        protected void RaiseOnFadeInFinished()
        {
            Log(nameof(OnFadeInFinished));

            _onFadeInFinished.Invoke();
            OnFadeInFinished?.Invoke(this);
        }
        protected void RaiseOnFadeOutFinished()
        {
            Log(nameof(OnFadeOutFinished));

            _onFadeOutFinished.Invoke();
            OnFadeOutFinished?.Invoke(this);
        }
        protected void RaiseOnFadingStarted()
        {
            Log(nameof(OnFadingStarted));

            _onFadingStarted.Invoke();
            OnFadingStarted?.Invoke(this);
        }
        protected void RaiseOnFadingFinished()
        {
            Log(nameof(OnFadingFinished));

            _onFadingFinished.Invoke();
            OnFadingFinished?.Invoke(this);
        }
    }

}
