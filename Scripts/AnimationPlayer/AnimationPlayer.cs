using UnityEngine;
using System;
using System.Diagnostics;

namespace KimScor.Utilities
{
    public class AnimationPlayer : MonoBehaviour
    {
        [SerializeField] private Animator _Animator;
        [SerializeField] private bool _UseDebug = false;
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
        private readonly int ACTION_NUMBER = Animator.StringToHash("ActionNumber");

        #region Debug
        [Conditional("UNITY_EDITOR")]
        private void Log(string log)
        {
#if UNITY_EDITOR
            if (!_UseDebug)
                return;

            UnityEngine.Debug.Log("Animation Player [ " + name + " ] : " + log);
#endif
        }
        #endregion
#if UNITY_EDITOR
        private void Reset()
        {
            gameObject.TryGetComponentInParentOrChildren(out _Animator);
        }
#endif

        public void PlayAction(int actionNumber)
        {
            if (!Animator)
            {
                Log("Play Action - Failed! Needs Animator Reference!");

                return;
            }
            Log("Play Action -" + actionNumber);

            _Animator.ResetTrigger(DO_ACTION);

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
                Log("Play Action - Failed! Needs Animator Reference!");

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
