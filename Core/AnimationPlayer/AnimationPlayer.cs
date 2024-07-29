using System;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public enum EAnimationState
    {
        None,
        TryPlay,
        Failed,
        Start,
        Playing,
        BlendOut,
        Finish,
        Canceled,
    }

    public class AnimationPlayer : BaseMonoBehaviour
    {
        public class Events
        {
            public event Action OnStarted;
            public event Action OnFailed;
            public event Action OnCanceled;
            public event Action OnFinished;
            public event Action OnFinishedBlendIn;
            public event Action OnStartedBlendOut;

            public event Action<string> OnNotify;
            public event Action<string> OnEnterNotifyState;
            public event Action<string> OnExitNotifyState;


            #region Callback
            public void Invoke_OnStarted()
            {
                OnStarted?.Invoke();
            }
            public void Invoke_OnFailed()
            {
                OnFailed?.Invoke();
            }
            public void Invoke_OnCanceled()
            {
                OnCanceled?.Invoke();
            }
            public void Invoke_OnFinished()
            {
                OnFinished?.Invoke();
            }
            public void Invoke_OnFinishedBlendIn()
            {
                OnFinishedBlendIn?.Invoke();
            }
            public void Invoke_OnStartedBlendOut()
            {
                OnStartedBlendOut?.Invoke();
            }

            public void Invoke_OnNotify(string notify)
            {
                OnNotify?.Invoke(notify);
            }

            public void Invoke_OnEnterNotifyState(string notify)
            {
                OnEnterNotifyState?.Invoke(notify);
            }

            public void Invoke_OnExitNotifyState(string notify)
            {
                OnExitNotifyState?.Invoke(notify);
            }

            #endregion
        }

        [Header(" [ Animation Player ] ")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _defaultState = "Empty";

        public Events AnimationEvents { get; set; }

        private int _defaultStateHash;
        
        private EAnimationState _state;

        private int _layer = 0;
        private int _currentStateHash;

        private bool _useFixedTransition;
        private float _fadeIn;
        private float _offset;
        private float _normalizedTime;

        private bool _isPlaying = false;
        private bool _isFinished = false;

        private readonly List<string> _notifyStates = new();

        public Animator Animator => _animator;
        public bool IsPlaying => _isPlaying;
        public bool IsFinished => _isFinished;
        public float NormalizedTime => _normalizedTime;
        public EAnimationState State => _state;

        public event Action OnStarted;
        public event Action OnFailed;
        public event Action OnCanceled;
        public event Action OnFinished;
        public event Action OnFinishedBlendIn;
        public event Action OnStartedBlendOut;

        public event Action<string> OnNotify;
        public event Action<string> OnEnterNotifyState;
        public event Action<string> OnExitNotifyState;


        private void Reset()
        {
#if UNITY_EDITOR
            gameObject.TryGetComponentInParentOrChildren(out _animator);
#endif
        }
        private void Awake()
        {
            _defaultStateHash = Animator.StringToHash(_defaultState);
        }
        private void OnDisable()
        {
            CancelAnimation();
        }

        private void LateUpdate()
        {
            if (!_isPlaying)
                return;

            var currentState = _animator.GetCurrentAnimatorStateInfo(_layer);
            var nextState = _animator.GetNextAnimatorStateInfo(_layer);

            _normalizedTime = _currentStateHash == nextState.shortNameHash ? nextState.normalizedTime : currentState.normalizedTime;

            switch (_state)
            {
                case EAnimationState.TryPlay:
                    if (currentState.shortNameHash == _currentStateHash || nextState.shortNameHash == _currentStateHash)
                    {
                        SetAnimationState(EAnimationState.Start);
                    }
                    else
                    {
                        SetAnimationState(EAnimationState.Failed);

                    }
                    break;
                case EAnimationState.Start:
                    if (!_animator.IsInTransition(_layer) && _currentStateHash == currentState.shortNameHash)
                    {
                        SetAnimationState(EAnimationState.Playing);
                    }
                    break;
                case EAnimationState.Playing:
                    if (_animator.IsInTransition(_layer))
                    {
                        SetAnimationState(EAnimationState.BlendOut);
                    }
                    break;
                case EAnimationState.BlendOut:
                    if (!_animator.IsInTransition(_layer))
                    {
                        SetAnimationState(EAnimationState.Finish);
                    }
                    break;
                default:
                    break;
            }
        }

        public void Play(string stateName, float fadeIn = 0.2f, float offset = 0f, int layer = 0, bool fixedTransition = false)
        {
            int hash = Animator.StringToHash(stateName);

            PlayAnimation(hash, fadeIn, offset, layer, fixedTransition);
        }

        public void Play(int stateHash, float fadeIn = 0.2f, float offset = 0f, int layer = 0, bool fixedTransition = false)
        {
            if (!_animator)
            {
                Log("Animator Is Null. Try GetComponent");

                if (!gameObject.TryGetComponentInParentOrChildren(out _animator))
                {
                    LogError("Animator Is NULL!!");
                }
            }

            PlayAnimation(stateHash, fadeIn, offset, layer, fixedTransition);
        }

        private void PlayAnimation(int stateHash, float fadeIn, float offset, int layer, bool fixedTransition)
        {
            CancelAnimation();

            _isPlaying = true;
            _isFinished = false;

            _currentStateHash = stateHash;
            _useFixedTransition = fixedTransition;
            _fadeIn = fadeIn;
            _offset = offset;
            _layer = layer;

            SetAnimationState(EAnimationState.TryPlay);
        }

        private void SetAnimationState(EAnimationState newState)
        {
            if (_state == newState)
                return;

            Log($"Current State - {newState} || Prev State - {_state}");
            _state = newState;

            switch (newState)
            {
                case EAnimationState.TryPlay:
                    OnTryPlay();
                    break;
                case EAnimationState.Failed:
                    OnFail();
                    break;
                case EAnimationState.Start:
                    OnStart();
                    break;
                case EAnimationState.Playing:
                    OnPlaying();
                    break;
                case EAnimationState.BlendOut:
                    OnBlendOut();
                    break;
                case EAnimationState.Finish:
                    OnFinish();
                    break;
                case EAnimationState.Canceled:
                    OnCancel();
                    break;
                default:
                    break;
            }
        }

        public bool IsPlayingHash(int animHash)
        {
            return IsPlaying && _currentStateHash == animHash;
        }
        private void OnPlaying()
        {
            Invoke_OnFinishedBlendIn();
        }

        public void TryStopAnimation(int hash, float fadeTime = 0.2f)
        {
            if (_currentStateHash != hash)
                return;

            var prevState = _state;

            if (prevState == EAnimationState.BlendOut)
            {
                CancelAnimation();
            }
            else
            {
                ForceStopAnimation(fadeTime);
            }
        }
        public void ForceStopAnimation(float fadeTime = 0.2f)
        {
            if (!IsPlaying)
                return;

            CancelAnimation();

            _animator.CrossFade(_defaultStateHash, fadeTime, _layer);
        }
        private void CancelAnimation()
        {
            if (!IsPlaying)
                return;

            SetAnimationState(EAnimationState.Canceled);
        }

        private void ClearEvents()
        {
            OnStarted = null;
            OnFailed = null;
            OnCanceled = null;
            OnFinished = null;
            OnFinishedBlendIn = null;
            OnStartedBlendOut = null;

            OnNotify = null;
            OnEnterNotifyState = null;
            OnExitNotifyState = null;

            AnimationEvents = null;
        }
        private void ExitAllNotify()
        {
            if (_notifyStates.Count <= 0)
                return;

            foreach (var notify in _notifyStates)
            {
                Invoke_OnExitNotifyState(notify);
            }

            _notifyStates.Clear();
        }

        private void OnTryPlay()
        {
            if (_useFixedTransition)
                _animator.CrossFadeInFixedTime(_currentStateHash, _fadeIn, _layer, _offset);
            else
                _animator.CrossFade(_currentStateHash, _fadeIn, _layer, _offset);

            _isPlaying = true;
            _normalizedTime = 0f;

            ClearEvents();
        }
        private void OnCancel()
        {
            ExitAllNotify();

            Invoke_OnCanceled();

            _currentStateHash = default;

            ClearEvents();
        }
        private void OnStart()
        {
            Invoke_OnStarted();
        }
        private void OnFail()
        {
            _normalizedTime = 0f;
            _isPlaying = false;
            _currentStateHash = default;

            Invoke_OnFailed();

            ClearEvents();
        }

        private void OnFinish()
        {
            ExitAllNotify();

            _normalizedTime = 1f;
            _isPlaying = false;
            _isFinished = true;
            _currentStateHash = default;

            Invoke_OnFinished();

            ClearEvents();
        }

        private void OnBlendOut()
        {
            Invoke_OnStartedBlendOut();
        }


        public void AnimNotify(string notify)
        {
            if (!IsPlaying)
                return;

            Invoke_OnNotify(notify);
        }
        public void AnimNotifyStateEnter(string notify)
        {
            if (!IsPlaying)
                return;

            if (_notifyStates.Contains(notify))
                return;

            _notifyStates.Add(notify);

            Invoke_OnEnterNotifyState(notify);
        }
        public void AnimNotifyStateExit(string notify)
        {
            if (!IsPlaying)
                return;

            if (!_notifyStates.Contains(notify))
                return;

            _notifyStates.Remove(notify);

            Invoke_OnExitNotifyState(notify);
        }

        #region Invoke
        protected void Invoke_OnStarted()
        {
            Log($"{nameof(OnStarted)}");

            OnStarted?.Invoke();
            AnimationEvents?.Invoke_OnStarted();

            OnFailed = null;
        }
        protected void Invoke_OnFailed()
        {
            Log($"{nameof(OnFailed)}");

            OnFailed?.Invoke();
            AnimationEvents?.Invoke_OnFailed();

            OnFailed = null;
        }
        protected void Invoke_OnCanceled()
        {
            Log($"{nameof(OnCanceled)}");

            OnCanceled?.Invoke();
            AnimationEvents?.Invoke_OnCanceled();

            OnCanceled = null;
            OnFinished = null;
            OnStartedBlendOut = null;
        }
        protected void Invoke_OnFinished()
        {
            Log($"{nameof(OnFinished)}");

            OnFinished?.Invoke();
            AnimationEvents?.Invoke_OnFinished();

            OnFinished = null;
            OnCanceled = null;
        }
        protected void Invoke_OnFinishedBlendIn()
        {
            Log($"{nameof(OnFinishedBlendIn)}");

            OnFinishedBlendIn?.Invoke();
            AnimationEvents?.Invoke_OnFinishedBlendIn();

            OnFinishedBlendIn = null;
        }
        protected void Invoke_OnStartedBlendOut()
        {
            Log($"{nameof(OnStartedBlendOut)}");

            OnStartedBlendOut?.Invoke();
            AnimationEvents?.Invoke_OnStartedBlendOut();

            OnStartedBlendOut = null;
        }

        protected void Invoke_OnNotify(string notify)
        {
            Log($"{nameof(OnNotify)} - {notify}");

            OnNotify?.Invoke(notify);
            AnimationEvents?.Invoke_OnNotify(notify);
        }

        protected void Invoke_OnEnterNotifyState(string notify)
        {
            Log($"{nameof(OnEnterNotifyState)} - {notify}");

            OnEnterNotifyState?.Invoke(notify);
            AnimationEvents?.Invoke_OnEnterNotifyState(notify);
        }

        protected void Invoke_OnExitNotifyState(string notify)
        {
            Log($"{nameof(OnExitNotifyState)} - {notify}");

            OnExitNotifyState?.Invoke(notify);
            AnimationEvents?.Invoke_OnExitNotifyState(notify);
        }

        #endregion
    }
}