using UnityEngine;


namespace StudioScor.Utilities.FadeSystem
{
    public interface IFadeSystem
    {
        public delegate void FadeSystemEventHandler(IFadeSystem fadeSystem);
        public delegate void FadeSystemUpdateAmountEventHandler(IFadeSystem fadeSystem, float amount);

        public float Duration { get; }
        public float Amount { get; }
        public bool IsFading { get; }
        public EFadeState State { get; }

        public event FadeSystemEventHandler OnStartedFadeIn;
        public event FadeSystemEventHandler OnStartedFadeOut;
        public event FadeSystemEventHandler OnFinishedFadeIn;
        public event FadeSystemEventHandler OnFinishedFadeOut;
        public event FadeSystemEventHandler OnStartedFading;
        public event FadeSystemEventHandler OnFinishedFading;

        public event FadeSystemUpdateAmountEventHandler OnUpdatedAmount;

        public void StartFadeIn(float duration, bool playFromStart = false);
        public void StartFadeOut(float duration, bool playFromStart = false);
        public void UpdateFading(float deltaTime);
        public void EndFadeIn();
        public void EndFadeOut();
    }

    public class FadeSystemComponent : BaseMonoBehaviour, IFadeSystem
    {
        [Header(" [ Fade System Component ] ")]
        [SerializeField] private EFadeState _state = EFadeState.FadeOut;
        [SerializeField] private float _duration = 1f;

        [Header(" Events ")]
        [SerializeField] private ToggleableUnityEvent _onStartedFadeIn;
        [SerializeField] private ToggleableUnityEvent _onFinishedFadeIn;
        [SerializeField] private ToggleableUnityEvent _onStartedFadeOut;
        [SerializeField] private ToggleableUnityEvent _onFinishedFadeOut;
        [SerializeField] private ToggleableUnityEvent _onStartedFading;
        [SerializeField] private ToggleableUnityEvent _onFinishedFading;

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

                Invoke_OnUpdatedAmount();
            }
        }
        public bool IsFading => _isFading;
        public EFadeState State => _state;

        public event IFadeSystem.FadeSystemEventHandler OnStartedFadeIn;
        public event IFadeSystem.FadeSystemEventHandler OnStartedFadeOut;
        public event IFadeSystem.FadeSystemEventHandler OnFinishedFadeIn;
        public event IFadeSystem.FadeSystemEventHandler OnFinishedFadeOut;
        public event IFadeSystem.FadeSystemEventHandler OnStartedFading;
        public event IFadeSystem.FadeSystemEventHandler OnFinishedFading;
        public event IFadeSystem.FadeSystemUpdateAmountEventHandler OnUpdatedAmount;

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
            OnStartedFadeIn = null;
            OnStartedFadeOut = null;
            OnFinishedFadeIn = null;
            OnFinishedFadeOut = null;
            OnUpdatedAmount = null;
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

            Invoke_OnStartedFadeIn();
            Invoke_OnStartedFading();

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

            Invoke_OnStartedFadeOut();
            Invoke_OnStartedFading();

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
            enabled = false;
            _isFading = false;
            Amount = 0f;

            Invoke_OnFinishedFading();
            Invoke_OnFinishedFadeIn();
        }
        public void EndFadeOut()
        {
            enabled = false;
            _isFading = false;
            Amount = 1f;

            Invoke_OnFinishedFading();
            Invoke_OnFinishedFadeOut();
        }

        protected void Invoke_OnUpdatedAmount()
        {
            Log($"{nameof(OnUpdatedAmount)} - Amount : {_amount}");

            OnUpdatedAmount?.Invoke(this, _amount);
        }

        protected void Invoke_OnStartedFadeIn()
        {
            Log(nameof(OnStartedFadeIn));

            _onStartedFadeIn.Invoke();
            OnStartedFadeIn?.Invoke(this);
        }
        protected void Invoke_OnStartedFadeOut()
        {
            Log(nameof(OnStartedFadeOut));

            _onStartedFadeOut.Invoke();
            OnStartedFadeOut?.Invoke(this);
        }
        protected void Invoke_OnFinishedFadeIn()
        {
            Log(nameof(OnFinishedFadeIn));

            _onFinishedFadeIn.Invoke();
            OnFinishedFadeIn?.Invoke(this);
        }
        protected void Invoke_OnFinishedFadeOut()
        {
            Log(nameof(OnFinishedFadeOut));

            _onFinishedFadeOut.Invoke();
            OnFinishedFadeOut?.Invoke(this);
        }
        protected void Invoke_OnStartedFading()
        {
            Log(nameof(OnStartedFading));

            _onStartedFading.Invoke();
            OnStartedFading?.Invoke(this);
        }
        protected void Invoke_OnFinishedFading()
        {
            Log(nameof(OnFinishedFading));

            _onFinishedFading.Invoke();
            OnFinishedFading?.Invoke(this);
        }
    }

}
