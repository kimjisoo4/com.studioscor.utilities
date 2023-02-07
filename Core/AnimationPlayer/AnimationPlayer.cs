using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace StudioScor.Utilities
{
    public class AnimationPlayer : BaseMonoBehaviour
    {
        [Header(" [ Animation Player ] ")]
        [SerializeField] private Animator _Animator;
        [SerializeField] private int _Layer = 1;
        [SerializeField] private string _DefaultStateName = "Empty";

        public event Action OnStarted;
        public event Action OnFailed;
        public event Action OnCanceled;
        public event Action OnFinished;
        public event Action OnStartedBlendOut;
        
        public event Action<string> OnNotify;
        public event Action<string> OnEnterNotifyState;
        public event Action<string> OnExitNotifyState;

        private Coroutine _Coroutine;
        private WaitUntil _WaitIsInTransition;
        private WaitUntil _WaitIsOutTranstion;
        private WaitForEndOfFrame _WaitEndFrame;

        private List<string> _NotifyStates;

        private int _DefaultStateHash;
        private int _CurrentStateHash;

        private AnimatorStateInfo CurrentState => _Animator.GetCurrentAnimatorStateInfo(_Layer);
        private AnimatorStateInfo NextState => _Animator.GetNextAnimatorStateInfo(_Layer);
        public bool IsPlaying => _CurrentStateHash != default 
                                 && (CurrentState.shortNameHash == _CurrentStateHash || NextState.shortNameHash == _CurrentStateHash);
        private bool IsNextState => NextState.shortNameHash == _CurrentStateHash;
        private AnimatorStateInfo AnimatorState => IsNextState ? NextState : CurrentState;
        public float Duration => IsPlaying ? AnimatorState.length : -1f;
        public float NormalizedTime => IsPlaying ? AnimatorState.normalizedTime: -1f;
        public float ElapsedTime => IsPlaying ? AnimatorState.length * AnimatorState.normalizedTime : -1f;


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

            _Coroutine = null;
            _WaitEndFrame = new();
            _WaitIsInTransition = new(() => { return !_Animator.IsInTransition(_Layer); });
            _WaitIsOutTranstion = new(() => { return _Animator.IsInTransition(_Layer); });
        }

        public void PlayAnimation(string stateName, float fade = 0.2f, float offset = 0f)
        {
            int hash = Animator.StringToHash(stateName);

            PlayAnimation(hash, fade, offset);
        }

        public void PlayAnimation(int stateHash, float fade = 0.2f, float offset = 0f)
        {
            if (!_Animator)
            {
                Log("Animator Is Null. Try GetComponent");

                if (!gameObject.TryGetComponentInParentOrChildren(out _Animator))
                {
                    Log("Animator Is NULL!!", true);
                }
            }

            CancelAnimation();

            _CurrentStateHash = stateHash;

            _Coroutine = StartCoroutine(AnimationPlay(stateHash, fade, offset));
        }

        public void StopAnimation(float fadeTime = 0.2f)
        {
            CancelAnimation();

            _Animator.CrossFade(_DefaultStateHash, fadeTime, _Layer);
        }

        private void CancelAnimation()
        {
            if (_Coroutine is not null)
            {
                StopCoroutine(_Coroutine);

                _Coroutine = null;

                ExitAllNotify();

                Callback_OnCanceled();

                _CurrentStateHash = default;
            }

            ClearEvents();
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

        protected IEnumerator AnimationPlay(int hash, float fade = 0.2f, float offset = 0f)
        {
            if (!_Animator.HasState(_Layer, hash))
            {
                yield return _WaitEndFrame;

                Log("This Animation State Is NULL!!", true);

                Callback_OnFailed();

                yield break;
            }

            _Animator.CrossFade(hash, fade, _Layer, offset);

            yield return _WaitEndFrame;

            if (IsPlaying)
            {
                Callback_OnStarted();
            }
            else
            {
                Callback_OnFailed();

                _Coroutine = null;

                yield break;
            }

            if (IsNextState)
            {
                yield return _WaitIsInTransition;
            }

            yield return _WaitIsOutTranstion;

            Callback_OnStartedBlendOut();

            yield return _WaitIsInTransition;

            Callback_OnFinished();

            _CurrentStateHash = default;
            _Coroutine = null;
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

            _NotifyStates.Add(notify);

            Callback_OnEnterNotifyState(notify);
        }
        public void AnimNotifyStateExit(string notify)
        {
            if (!IsPlaying)
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