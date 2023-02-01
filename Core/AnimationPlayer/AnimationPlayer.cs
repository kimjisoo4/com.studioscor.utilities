using UnityEngine;
using System;
using System.Diagnostics;

namespace StudioScor.Utilities
{
    public class AnimationPlayer : MonoBehaviour
    {
        [Header(" [ Animation Player ] ")]
        [SerializeField] private Animator _Animator;

        public Animator Animator => _Animator;

        public event Action OnCanceled;
        public event Action OnFinished;
        public event Action OnBlendOut;

        private int _CurrentActionNumber;
        private int _Hash;
        private AnimationCallback _AnimationCallback;

        public int CurrentHash => _Hash;
        public int CurrentActionNumber => _CurrentActionNumber;

        private readonly int DO_ACTION = Animator.StringToHash("DoAction");
        private readonly int DO_ENDACTION = Animator.StringToHash("DoEndAction");
        private readonly int ACTION_NUMBER = Animator.StringToHash("ActionNumber");

        #region EDITOR ONLY
#if UNITY_EDITOR
        [Header(" [ Use Debug ] ")]
        [SerializeField] private bool _UseDebug = false;

#endif

        [Conditional("UNITY_EDITOR")]
        private void Log(object message, bool isError = false)
        {
#if UNITY_EDITOR
            if (isError)
            {
                Utility.Debug.LogError("Animation Player [ " + name + " ] : " + message, this);
            }
            else if(_UseDebug)
            {
                Utility.Debug.Log("Animation Player [ " + name + " ] : " + message, this);
            }
#endif
        }

#if UNITY_EDITOR
        private void Reset()
        {
            gameObject.TryGetComponentInParentOrChildren(out _Animator);
        }
#endif

        #endregion


        public void PlayAction(int actionNumber, Action finished = null, Action canceled = null, Action blendOut = null)
        {
            PlayAction(actionNumber);

            if (finished is not null)
                OnFinished += finished;

            if (canceled is not null)
                OnCanceled += canceled;

            if (blendOut is not null)
                OnBlendOut += blendOut;
        }
        public void PlayAction(int actionNumber)
        {
            if (!Animator)
            {
                Log("Play Action - Failed! Needs Animator Reference!", true);

                return;
            }
            Log("Play Action -" + actionNumber);

            _Animator.ResetTrigger(DO_ACTION);
            _Animator.ResetTrigger(DO_ENDACTION);

            OnCanceled?.Invoke();

            OnCanceled = null;
            OnFinished = null;

            _AnimationCallback = null;
            _CurrentActionNumber = actionNumber;

            _Animator.SetInteger(ACTION_NUMBER, actionNumber);
            _Animator.SetTrigger(DO_ACTION);

            _Animator.applyRootMotion = false;
        }
        public bool PlayActionToName(int hashNumber, int layer = 0)
        {
            if(!Animator)
            {
                Log("Play Action - Failed! Needs Animator Reference!", true);

                return false;
            }
            if (!_Animator.HasState(layer, hashNumber))
            {
                return false;
            }

            Log("Play Action To Name - HashNumber -" + hashNumber);

            OnCanceled?.Invoke();

            OnCanceled = null;
            OnFinished = null;

            _AnimationCallback = null;
            _Hash = hashNumber;

            _Animator.Play(hashNumber, layer);

            _Animator.applyRootMotion = false;

            return true;
        }
        public void StopAction(int actionNumber)
        {
            if (_CurrentActionNumber != actionNumber)
                return;

            _Animator.ResetTrigger(DO_ACTION);
            _Animator.ResetTrigger(DO_ENDACTION);

            OnCanceled?.Invoke();

            OnCanceled = null;
            OnFinished = null;

            _AnimationCallback = null;
            _CurrentActionNumber = default;

            _Animator.SetTrigger(DO_ENDACTION);

            _Animator.applyRootMotion = false;
        }

        public bool IsPlayingAction(int actionNumber)
        {
            return _CurrentActionNumber == actionNumber;
        }
        public float GetActionNormalizedTime()
        {
            if (!_AnimationCallback)
                return -1f;

            return _AnimationCallback.NormalizedTime;
        }
        public float GetActionTime()
        {
            if (!_AnimationCallback)
                return -1f;

            return _AnimationCallback.Duration;
        }
        public void OnEnterAnimation(AnimationCallback animationCallback)
        {
            Log("On Enter Animation");  

            _AnimationCallback = animationCallback;

            _Animator.applyRootMotion = _AnimationCallback.UseRootMotion;
        }
        public void OnFinishAnimation(AnimationCallback animationCallback)
        {
            if (_AnimationCallback == animationCallback)
            {
                Log("On Finish Animation");

                OnFinished?.Invoke();

                OnCanceled = null;
                OnFinished = null;

                _Hash = default;

                _AnimationCallback = null;

                _Animator.applyRootMotion = false;
            }
        }
        public void OnBlendOutAnimation(AnimationCallback animationCallback)
        {
            if (_AnimationCallback == animationCallback)
            {
                Log("On Blend Out Animation");

                OnBlendOut?.Invoke();

                OnBlendOut = null;
            }
        }
    }
}
