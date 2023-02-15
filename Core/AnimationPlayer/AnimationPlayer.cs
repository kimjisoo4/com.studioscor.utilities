using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

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
        [SerializeField] private Animator _Animator;
        [SerializeField] private int _Layer = 1;
        [SerializeField] private string _DefaultStateName = "Empty";

        private int _DefaultStateHash;
        private int _CurrentStateHash;

        private float _FadeIn;
        private float _FadeOut;
        private float _Offset;

        private List<string> _NotifyStates;

        private EAnimationState _State;
        private bool _IsPlaying = false;
        private float _NormalizedTime;

        public bool IsPlaying => _IsPlaying;
        public float NormalizedTime => _NormalizedTime;


        public Action OnStarted;
        public Action OnFailed;
        public Action OnCanceled;
        public Action OnFinished;
        public Action OnStartedBlendOut;

        public Action<string> OnNotify;
        public Action<string> OnEnterNotifyState;
        public Action<string> OnExitNotifyState;


        private void Reset()
        {
            gameObject.TryGetComponentInParentOrChildren(out _Animator);
        }
        private void OnValidate()
        {
            _DefaultStateHash = Animator.StringToHash(_DefaultStateName);
        }

        private void Awake()
        {
            _DefaultStateHash = Animator.StringToHash(_DefaultStateName);
            _NotifyStates = new();
        }

        private void LateUpdate()
        {
            if (!_IsPlaying)
                return;

            var currentState = _Animator.GetCurrentAnimatorStateInfo(_Layer);
            var nextState = _Animator.GetNextAnimatorStateInfo(_Layer);

            _NormalizedTime = _CurrentStateHash == nextState.shortNameHash ? nextState.normalizedTime : currentState.normalizedTime;

            switch (_State)
            {
                case EAnimationState.TryPlay:
                    if (currentState.shortNameHash == _CurrentStateHash || nextState.shortNameHash == _CurrentStateHash)
                    {
                        SetAnimationState(EAnimationState.Start);
                    }
                    else
                    {
                        SetAnimationState(EAnimationState.Failed);

                    }
                    break;
                case EAnimationState.Start:
                    if (!_Animator.IsInTransition(_Layer))
                    {
                        SetAnimationState(EAnimationState.Playing);
                    }
                    break;
                case EAnimationState.Playing:
                    if (_NormalizedTime > _FadeOut)
                    {
                        SetAnimationState(EAnimationState.BlendOut);
                    }
                    break;
                case EAnimationState.BlendOut:
                    if (!_Animator.IsInTransition(_Layer))
                    {
                        SetAnimationState(EAnimationState.Finish);
                    }
                    break;
                default:
                    break;
            }
        }



        public void Play(string stateName, float fadeIn = 0.2f, float fadeOut = 0.8f, float offset = 0f)
        {
            int hash = Animator.StringToHash(stateName);

            PlayAnimation(hash, fadeIn, fadeOut, offset);
        }

        public void Play(int stateHash, float fadeIn = 0.2f, float fadeOut = 0.8f, float offset = 0f)
        {
            if (!_Animator)
            {
                Log("Animator Is Null. Try GetComponent");

                if (!gameObject.TryGetComponentInParentOrChildren(out _Animator))
                {
                    Log("Animator Is NULL!!", true);
                }
            }

            PlayAnimation(stateHash, fadeIn, fadeOut, offset);
        }
        private void PlayAnimation(int stateHash, float fadeIn, float fadeOut, float offset)
        {
            CancelAnimation();

            _IsPlaying = true;
            _CurrentStateHash = stateHash;
            _FadeIn = fadeIn;
            _FadeOut = fadeOut;
            _Offset = offset;

            SetAnimationState(EAnimationState.TryPlay);
        }


        private void SetAnimationState(EAnimationState newState)
        {
            _State = newState;

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

        


        public void TryStopAnimation(int hash, float fadeTime = 0.2f)
        {
            if (_CurrentStateHash != hash)
                return;

            ForceStopAnimation(fadeTime);
        }
        public void ForceStopAnimation(float fadeTime = 0.2f)
        {
            if (!IsPlaying)
                return;

            CancelAnimation();

            _Animator.CrossFade(_DefaultStateHash, fadeTime, _Layer);
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
            OnStartedBlendOut = null;

            OnNotify = null;
            OnEnterNotifyState = null;
            OnExitNotifyState = null;
        }
        private void ExitAllNotify()
        {
            if (_NotifyStates.Count <= 0)
                return;

            foreach (var notify in _NotifyStates)
            {
                Callback_OnExitNotifyState(notify);
            }

            _NotifyStates.Clear();
        }





        private void OnTryPlay()
        {
            _Animator.CrossFade(_CurrentStateHash, _FadeIn, _Layer, _Offset);

            _IsPlaying = true;
            _NormalizedTime = 0f;

            ClearEvents();
        }
        private void OnCancel()
        {
            ExitAllNotify();

            Callback_OnCanceled();

            _CurrentStateHash = default;

            ClearEvents();
        }
        private void OnStart()
        {
            Callback_OnStarted();
        }
        private void OnFail()
        {
            _NormalizedTime = 0f;
            _IsPlaying = false;
            _CurrentStateHash = default;

            Callback_OnFailed();

            ClearEvents();
        }

        private void OnFinish()
        {
            _NormalizedTime = 1f;
            _IsPlaying = false;
            _CurrentStateHash = default;

            Callback_OnFinished();

            ClearEvents();
        }

        private void OnBlendOut()
        {
            Callback_OnStartedBlendOut();

            if (_FadeOut < 1f)
            {
                _Animator.CrossFade(_DefaultStateHash, 1f - _FadeOut, _Layer);
            }
            else
            {
                SetAnimationState(EAnimationState.Finish);
            }
        }


        public void AnimNotify(string notify)
        {
            if (!IsPlaying)
                return;

            Callback_OnNotify(notify);
        }
        public void AnimNotifyStateEnter(string notify)
        {
            if (!IsPlaying)
                return;

            if (_NotifyStates.Contains(notify))
                return;

            _NotifyStates.Add(notify);

            Callback_OnEnterNotifyState(notify);
        }
        public void AnimNotifyStateExit(string notify)
        {
            if (!IsPlaying)
                return;

            if (!_NotifyStates.Contains(notify))
                return;

            _NotifyStates.Remove(notify);

            Callback_OnExitNotifyState(notify);
        }

        #region Callback
        protected void Callback_OnStarted()
        {
            Log("On Enter Animation");

            OnStarted?.Invoke();

            OnFailed = null;
        }
        protected void Callback_OnFailed()
        {
            Log("On Failed Animation");

            OnFailed?.Invoke();

            OnFailed = null;
        }
        protected void Callback_OnCanceled()
        {
            Log("On Canceled Animation");

            OnCanceled?.Invoke();

            OnCanceled = null;
            OnFinished = null;
            OnStartedBlendOut = null;
        }
        protected void Callback_OnFinished()
        {
            Log("On Finished Animation");

            OnFinished?.Invoke();

            OnFinished = null;
            OnCanceled = null;
        }

        protected void Callback_OnStartedBlendOut()
        {
            Log("On Blend In Animation");

            OnStartedBlendOut?.Invoke();

            OnStartedBlendOut = null;
        }

        protected void Callback_OnNotify(string notify)
        {
            Log("On Notify - " + notify);

            OnNotify?.Invoke(notify);
        }

        protected void Callback_OnEnterNotifyState(string notify)
        {
            Log("On Enter Notify State - " + notify);

            OnEnterNotifyState?.Invoke(notify);
        }

        protected void Callback_OnExitNotifyState(string notify)
        {
            Log("On Exit Notify State - " + notify);

            OnExitNotifyState?.Invoke(notify);
        }

        #endregion
    }
}