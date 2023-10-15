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
        [SerializeField] private Animator animator;
        [SerializeField] private int layer = 1;
        [SerializeField] private string defaultStateName = "Empty";

        private int defaultStateHash;
        private int currentStateHash;

        private float fadeIn;
        private float fadeOut;
        private float offset;

        private List<string> notifyStates;

        private EAnimationState state;
        private bool isPlaying = false;
        private float normalizedTime;

        public Animator Animator => animator;
        public bool IsPlaying => isPlaying;
        public float NormalizedTime => normalizedTime;


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
            gameObject.TryGetComponentInParentOrChildren(out animator);
        }

        private void OnValidate()
        {
            defaultStateHash = Animator.StringToHash(defaultStateName);
        }

        private void Awake()
        {
            defaultStateHash = Animator.StringToHash(defaultStateName);
            notifyStates = new();
        }

        private void LateUpdate()
        {
            if (!isPlaying)
                return;

            var currentState = animator.GetCurrentAnimatorStateInfo(layer);
            var nextState = animator.GetNextAnimatorStateInfo(layer);

            normalizedTime = currentStateHash == nextState.shortNameHash ? nextState.normalizedTime : currentState.normalizedTime;

            switch (state)
            {
                case EAnimationState.TryPlay:
                    if (currentState.shortNameHash == currentStateHash || nextState.shortNameHash == currentStateHash)
                    {
                        SetAnimationState(EAnimationState.Start);
                    }
                    else
                    {
                        SetAnimationState(EAnimationState.Failed);

                    }
                    break;
                case EAnimationState.Start:
                    if (!animator.IsInTransition(layer) && currentStateHash == currentState.shortNameHash)
                    {
                        SetAnimationState(EAnimationState.Playing);
                    }
                    break;
                case EAnimationState.Playing:
                    if (normalizedTime > fadeOut)
                    {
                        SetAnimationState(EAnimationState.BlendOut);
                    }
                    break;
                case EAnimationState.BlendOut:
                    if (!animator.IsInTransition(layer))
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
            if (!animator)
            {
                Log("Animator Is Null. Try GetComponent");

                if (!gameObject.TryGetComponentInParentOrChildren(out animator))
                {
                    Log("Animator Is NULL!!", true);
                }
            }

            PlayAnimation(stateHash, fadeIn, fadeOut, offset);
        }
        private void PlayAnimation(int stateHash, float fadeIn, float fadeOut, float offset)
        {
            CancelAnimation();

            isPlaying = true;
            currentStateHash = stateHash;
            this.fadeIn = fadeIn;
            this.fadeOut = fadeOut;
            this.offset = offset;

            SetAnimationState(EAnimationState.TryPlay);
        }


        private void SetAnimationState(EAnimationState newState)
        {
            state = newState;

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
            Callback_OnFinishedBlendIn();
        }

        public void TryStopAnimation(int hash, float fadeTime = 0.2f)
        {
            if (currentStateHash != hash)
                return;

            ForceStopAnimation(fadeTime);
        }
        public void ForceStopAnimation(float fadeTime = 0.2f)
        {
            if (!IsPlaying)
                return;

            CancelAnimation();

            animator.CrossFade(defaultStateHash, fadeTime, layer);
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
            if (notifyStates.Count <= 0)
                return;

            foreach (var notify in notifyStates)
            {
                Callback_OnExitNotifyState(notify);
            }

            notifyStates.Clear();
        }

        private void OnTryPlay()
        {
            animator.CrossFade(currentStateHash, fadeIn, layer, offset);

            isPlaying = true;
            normalizedTime = 0f;

            ClearEvents();
        }
        private void OnCancel()
        {
            ExitAllNotify();

            Callback_OnCanceled();

            currentStateHash = default;

            ClearEvents();
        }
        private void OnStart()
        {
            Callback_OnStarted();
        }
        private void OnFail()
        {
            normalizedTime = 0f;
            isPlaying = false;
            currentStateHash = default;

            Callback_OnFailed();

            ClearEvents();
        }

        private void OnFinish()
        {
            ExitAllNotify();

            normalizedTime = 1f;
            isPlaying = false;
            currentStateHash = default;

            Callback_OnFinished();

            ClearEvents();
        }

        private void OnBlendOut()
        {
            Callback_OnStartedBlendOut();

            if (fadeOut < 1f)
            {
                animator.CrossFade(defaultStateHash, 1f - fadeOut, layer);
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

            if (notifyStates.Contains(notify))
                return;

            notifyStates.Add(notify);

            Callback_OnEnterNotifyState(notify);
        }
        public void AnimNotifyStateExit(string notify)
        {
            if (!IsPlaying)
                return;

            if (!notifyStates.Contains(notify))
                return;

            notifyStates.Remove(notify);

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
        protected void Callback_OnFinishedBlendIn()
        {
            Log("On Finished Blend In Animation");

            OnFinishedBlendIn?.Invoke();

            OnFinishedBlendIn = null;
        }
        protected void Callback_OnStartedBlendOut()
        {
            Log("On Started Blend Out Animation");

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