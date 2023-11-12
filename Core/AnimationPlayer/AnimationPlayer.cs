using UnityEngine;
using System;
using System.Collections.Generic;

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
        [Header(" [ Animation Player ] ")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _defaultState = "Empty";

        private int _defaultStateHash;
        
        private EAnimationState _state;

        private int _layer = 0;
        private int _currentStateHash;

        private float _fadeIn;
        private float _offset;
        private float _normalizedTime;
        private bool _isPlaying = false;


        private readonly List<string> _notifyStates = new();

        public Animator Animator => _animator;
        public bool IsPlaying => _isPlaying;
        public float NormalizedTime => _normalizedTime;

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



        public void Play(string stateName, float fadeIn = 0.2f, float offset = 0f, int layer = 0)
        {
            int hash = Animator.StringToHash(stateName);

            PlayAnimation(hash, fadeIn, offset, layer);
        }

        public void Play(int stateHash, float fadeIn = 0.2f, float offset = 0f, int layer = 0)
        {
            if (!_animator)
            {
                Log("Animator Is Null. Try GetComponent");

                if (!gameObject.TryGetComponentInParentOrChildren(out _animator))
                {
                    LogError("Animator Is NULL!!");
                }
            }

            PlayAnimation(stateHash, fadeIn, offset, layer);
        }
        private void PlayAnimation(int stateHash, float fadeIn, float offset, int layer = 0)
        {
            CancelAnimation();

            _isPlaying = true;
            _currentStateHash = stateHash;
            this._fadeIn = fadeIn;
            this._offset = offset;
            this._layer = layer;

            SetAnimationState(EAnimationState.TryPlay);
        }


        private void SetAnimationState(EAnimationState newState)
        {
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

        private void OnPlaying()
        {
            Invoke_OnFinishedBlendIn();
        }

        public void TryStopAnimation(int hash, float fadeTime = 0.2f)
        {
            if (_currentStateHash != hash)
                return;

            ForceStopAnimation(fadeTime);
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

        #region Callback
        protected void Invoke_OnStarted()
        {
            Log("On Enter Animation");

            OnStarted?.Invoke();

            OnFailed = null;
        }
        protected void Invoke_OnFailed()
        {
            Log("On Failed Animation");

            OnFailed?.Invoke();

            OnFailed = null;
        }
        protected void Invoke_OnCanceled()
        {
            Log("On Canceled Animation");

            OnCanceled?.Invoke();

            OnCanceled = null;
            OnFinished = null;
            OnStartedBlendOut = null;
        }
        protected void Invoke_OnFinished()
        {
            Log("On Finished Animation");

            OnFinished?.Invoke();

            OnFinished = null;
            OnCanceled = null;
        }
        protected void Invoke_OnFinishedBlendIn()
        {
            Log("On Finished Blend In Animation");

            OnFinishedBlendIn?.Invoke();

            OnFinishedBlendIn = null;
        }
        protected void Invoke_OnStartedBlendOut()
        {
            Log("On Started Blend Out Animation");

            OnStartedBlendOut?.Invoke();

            OnStartedBlendOut = null;
        }

        protected void Invoke_OnNotify(string notify)
        {
            Log("On Notify - " + notify);

            OnNotify?.Invoke(notify);
        }

        protected void Invoke_OnEnterNotifyState(string notify)
        {
            Log("On Enter Notify State - " + notify);

            OnEnterNotifyState?.Invoke(notify);
        }

        protected void Invoke_OnExitNotifyState(string notify)
        {
            Log("On Exit Notify State - " + notify);

            OnExitNotifyState?.Invoke(notify);
        }

        #endregion
    }
}