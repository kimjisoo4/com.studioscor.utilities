using System;
using System.Collections;
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

            public void Clear_OnStarted() => OnStarted = null;
            public void Clear_OnFailed() => OnFailed = null;
            public void Clear_OnCanceled() => OnCanceled = null;
            public void Clear_OnFinished() => OnFinished = null;

            public void Clear_OnFinishedBlendIn() => OnFinishedBlendIn = null;
            public void Clear_OnStartedBlendOut() => OnStartedBlendOut = null;

            public void Clear_OnNotify() => OnNotify = null;
            public void Clear_OnEnterNotifyState() => OnEnterNotifyState = null;
            public void Clear_OnExitNotifyState() => OnExitNotifyState = null;


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

        private Events _currentAnimationEvents;
        private Events _nextAnimationEvents;

        public Events AnimationEvents
        {
            get
            {
                return _currentAnimationEvents;
            }
            set
            {
                if(_inEnded)
                    _nextAnimationEvents = value;
                else
                    _currentAnimationEvents = value;
            }
        }

        private Events _currentEvents = new();
        private Events _nextEvents = new();

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

        private bool _inEnded = false;
        private bool _hasNextPlay;
        private int _nextStateHash;
        private bool _nextUseFixedTransition;
        private float _nextFadeIn;
        private float _nextOffset;
        private int _nextLayer;

        private bool _wasFauiled;
        private readonly List<string> _notifyStates = new();
        
        public Animator Animator => _animator;
        public bool IsPlaying => _isPlaying;
        public bool IsFinished => _isFinished;
        public float NormalizedTime => _normalizedTime;
        public EAnimationState State => _state;


        public event Action OnStarted
        {
            add
            {
                if(_inEnded)
                    _nextEvents.OnStarted += value;
                else
                    _currentEvents.OnStarted += value;
            }
            remove
            {
                if(_inEnded)
                    _nextEvents.OnStarted -= value;
                else
                    _currentEvents.OnStarted -= value;
            }
        }
        public event Action OnFailed
        {
            add
            {
                if (_inEnded)
                    _nextEvents.OnFailed += value;
                else
                    _currentEvents.OnFailed += value;
            }
            remove
            {
                if (_inEnded)
                    _nextEvents.OnFailed -= value;
                else
                    _currentEvents.OnFailed -= value;
            }
        }
        public event Action OnCanceled
        {
            add
            {
                if (_inEnded)
                    _nextEvents.OnCanceled += value;
                else
                    _currentEvents.OnCanceled += value;
            }
            remove
            {
                if (_inEnded)
                    _nextEvents.OnCanceled -= value;
                else
                    _currentEvents.OnCanceled -= value;
            }
        }
        public event Action OnFinished
        {
            add
            {
                if (_inEnded)
                    _nextEvents.OnFinished += value;
                else
                    _currentEvents.OnFinished += value;
            }
            remove
            {
                if (_inEnded)
                    _nextEvents.OnFinished -= value;
                else
                    _currentEvents.OnFinished -= value;
            }
        }
        public event Action OnFinishedBlendIn
        {
            add
            {
                if (_inEnded)
                    _nextEvents.OnFinishedBlendIn += value;
                else
                    _currentEvents.OnFinishedBlendIn += value;
            }
            remove
            {
                if (_inEnded)
                    _nextEvents.OnFinishedBlendIn -= value;
                else
                    _currentEvents.OnFinishedBlendIn -= value;
            }
        }
        public event Action OnStartedBlendOut
        {
            add
            {
                if (_inEnded)
                    _nextEvents.OnStartedBlendOut += value;
                else
                    _currentEvents.OnStartedBlendOut += value;
            }
            remove
            {
                if (_inEnded)
                    _nextEvents.OnStartedBlendOut -= value;
                else
                    _currentEvents.OnStartedBlendOut -= value;
            }
        }

        public event Action<string> OnNotify
        {
            add
            {
                if (_inEnded)
                    _nextEvents.OnNotify += value;
                else
                    _currentEvents.OnNotify += value;
            }
            remove
            {
                if (_inEnded)
                    _nextEvents.OnNotify -= value;
                else
                    _currentEvents.OnNotify -= value;
            }
        }
        public event Action<string> OnEnterNotifyState
        {
            add
            {
                if (_inEnded)
                    _nextEvents.OnEnterNotifyState += value;
                else
                    _currentEvents.OnEnterNotifyState += value;
            }
            remove
            {
                if (_inEnded)
                    _nextEvents.OnEnterNotifyState -= value;
                else
                    _currentEvents.OnEnterNotifyState -= value;
            }
        }
        public event Action<string> OnExitNotifyState
        {
            add
            {
                if (_inEnded)
                    _nextEvents.OnExitNotifyState += value;
                else
                    _currentEvents.OnExitNotifyState += value;
            }
            remove
            {
                if (_inEnded)
                    _nextEvents.OnExitNotifyState -= value;
                else
                    _currentEvents.OnExitNotifyState -= value;
            }
        }


        private void Reset()
        {
#if UNITY_EDITOR
            if (!_animator)
                _animator = gameObject.GetComponentInParentOrChildren<Animator>();
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
                    Log($"{_currentStateHash} || {currentState.shortNameHash} || {nextState.shortNameHash}");

                    if (currentState.shortNameHash == _currentStateHash || nextState.shortNameHash == _currentStateHash)
                    {
                        SetAnimationState(EAnimationState.Start);
                    }
                    else
                    {
                        if(nextState.shortNameHash == 0 && !_wasFauiled)
                        {
                            _wasFauiled = true;
                            return;
                        }

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
                    else if(_normalizedTime >= 1f)
                    {
                        SetAnimationState(EAnimationState.Finish);
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
            if(_inEnded)
            {
                if(_hasNextPlay)
                {
                    ClearNextEvents();
                }

                _hasNextPlay = true;
                _nextStateHash = stateHash;
                _nextUseFixedTransition = fixedTransition;
                _nextFadeIn = fadeIn;
                _nextOffset = offset;
                _nextLayer = layer;
            }
            else
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

            if (gameObject.activeInHierarchy)
                _animator.CrossFade(_defaultStateHash, fadeTime, _layer);
        }
        private void CancelAnimation()
        {
            if (!IsPlaying)
                return;

            SetAnimationState(EAnimationState.Canceled);
        }

        private void ClearCurrentEvents()
        {
            _currentEvents.Clear_OnStarted();
            _currentEvents.Clear_OnFailed();
            _currentEvents.Clear_OnCanceled();
            _currentEvents.Clear_OnFinished();
            _currentEvents.Clear_OnFinishedBlendIn();

            _currentEvents.Clear_OnNotify();
            _currentEvents.Clear_OnEnterNotifyState();
            _currentEvents.Clear_OnExitNotifyState();

            _currentAnimationEvents = null;
        }
        private void ClearNextEvents()
        {
            _nextEvents.Clear_OnStarted();
            _nextEvents.Clear_OnFailed();
            _nextEvents.Clear_OnCanceled();
            _nextEvents.Clear_OnFinished();
            _nextEvents.Clear_OnFinishedBlendIn();
            
            _nextEvents.Clear_OnNotify();
            _nextEvents.Clear_OnEnterNotifyState();
            _nextEvents.Clear_OnExitNotifyState();

            _nextAnimationEvents = null;
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
            Log($"{nameof(OnTryPlay)} :: {_currentStateHash}");

            if (_useFixedTransition)
                _animator.CrossFadeInFixedTime(_currentStateHash, _fadeIn, _layer, _offset);
            else
                _animator.CrossFade(_currentStateHash, _fadeIn, _layer, _offset);

            _isPlaying = true;
            _normalizedTime = 0f;
            _wasFauiled = false;
        }
        private void OnCancel()
        {
            ExitAllNotify();

            _isPlaying = false;
            _currentStateHash = default;

            Invoke_OnCanceled();

            ClearCurrentEvents();

            if (_hasNextPlay)
            {
                _hasNextPlay = false;

                Play(_nextStateHash, _nextFadeIn, _nextOffset, _nextLayer, _nextUseFixedTransition);
            }
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

            ClearCurrentEvents();

            if (_hasNextPlay)
            {
                _hasNextPlay = false;

                Play(_nextStateHash, _nextFadeIn, _nextOffset, _nextLayer, _nextUseFixedTransition);
            }
        }

        private void OnFinish()
        {
            _inEnded = true;

            ExitAllNotify();

            _normalizedTime = 1f;
            _isPlaying = false;
            _isFinished = true;
            _currentStateHash = default;

            Invoke_OnFinished();

            _inEnded = false;

            ClearCurrentEvents();

            if(_hasNextPlay)
            {
                _hasNextPlay = false;

                Play(_nextStateHash, _nextFadeIn, _nextOffset, _nextLayer, _nextUseFixedTransition);

                _currentAnimationEvents = _nextAnimationEvents;
                _nextAnimationEvents = null;

                (_nextEvents, _currentEvents) = (_currentEvents, _nextEvents);
            }
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

            _currentEvents.Invoke_OnStarted();
            AnimationEvents?.Invoke_OnStarted();

            _currentEvents.Clear_OnStarted();
            _currentEvents.Clear_OnFailed();
        }
        protected void Invoke_OnFailed()
        {
            Log($"{nameof(OnFailed)}");

            _currentEvents.Invoke_OnFailed();
            AnimationEvents?.Invoke_OnFailed();

            _currentEvents.Clear_OnStarted();
            _currentEvents.Clear_OnFailed();
        }
        protected void Invoke_OnCanceled()
        {
            Log($"{nameof(OnCanceled)}");

            _currentEvents.Invoke_OnCanceled();
            AnimationEvents?.Invoke_OnCanceled();

            _currentEvents.Clear_OnCanceled();
            _currentEvents.Clear_OnFinished();
            _currentEvents.Clear_OnStartedBlendOut();
        }
        protected void Invoke_OnFinished()
        {
            Log($"{nameof(OnFinished)}");

            _currentEvents.Invoke_OnFinished();
            AnimationEvents?.Invoke_OnFinished();

            _currentEvents.Clear_OnCanceled();
            _currentEvents.Clear_OnFinished();
        }
        protected void Invoke_OnFinishedBlendIn()
        {
            Log($"{nameof(OnFinishedBlendIn)}");

            _currentEvents.Invoke_OnFinishedBlendIn();
            AnimationEvents?.Invoke_OnFinishedBlendIn();

            _currentEvents.Clear_OnFinishedBlendIn();
        }
        protected void Invoke_OnStartedBlendOut()
        {
            Log($"{nameof(OnStartedBlendOut)}");

            _currentEvents?.Invoke_OnStartedBlendOut();
            AnimationEvents?.Invoke_OnStartedBlendOut();

            _currentEvents.Clear_OnStartedBlendOut();
        }

        protected void Invoke_OnNotify(string notify)
        {
            Log($"{nameof(OnNotify)} - {notify}");

            _currentEvents?.Invoke_OnNotify(notify);
            AnimationEvents?.Invoke_OnNotify(notify);
        }

        protected void Invoke_OnEnterNotifyState(string notify)
        {
            Log($"{nameof(OnEnterNotifyState)} - {notify}");

            _currentEvents?.Invoke_OnEnterNotifyState(notify);
            AnimationEvents?.Invoke_OnEnterNotifyState(notify);
        }

        protected void Invoke_OnExitNotifyState(string notify)
        {
            Log($"{nameof(OnExitNotifyState)} - {notify}");

            _currentEvents?.Invoke_OnExitNotifyState(notify);
            AnimationEvents?.Invoke_OnExitNotifyState(notify);
        }

        #endregion
    }
}