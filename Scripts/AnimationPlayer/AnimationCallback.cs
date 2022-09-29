using UnityEngine;
using System.Diagnostics;

namespace KimScor.Utilities
{
    public class AnimationCallback : StateMachineBehaviour
    {
        private AnimationPlayer _AnimationPlayer;

        [SerializeField] private float _NormalizedTime = 0f;
        [SerializeField] private int _Hash;
        [SerializeField] private bool _UseDebug = false;

        public int Hash => _Hash;
        public float NormalizedTime => _NormalizedTime;

        private bool _FinishedTransition = false;
        private bool _IsStartedBlendOut = false;


        [Conditional("UNITY_EDITOR")]
        private void Log(string log)
        {
            if (!_UseDebug)
                return;

            UnityEngine.Debug.Log("AnimationCallBack [ " + _AnimationPlayer?.name + " ] : " + log);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.TryGetComponent(out _AnimationPlayer);

            Log("On State Enter");

            _FinishedTransition = false;
            _IsStartedBlendOut = false;
            _Hash = stateInfo.shortNameHash;
            _NormalizedTime = stateInfo.normalizedTime;

            _AnimationPlayer?.OnEnterAnimation(this);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _NormalizedTime = stateInfo.normalizedTime;

            if (!_FinishedTransition)
            {
                if (!animator.IsInTransition(layerIndex))
                {
                    Log("Finish Transition");

                    _FinishedTransition = true;
                }
            }
            else
            {
                if (!_IsStartedBlendOut && animator.IsInTransition(layerIndex))
                {
                    Log("Start Blend Out");

                    _IsStartedBlendOut = true;

                    _AnimationPlayer?.OnBlendOutAnimation(this);
                }
            }

        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Log("On State Exit");

            _NormalizedTime = stateInfo.normalizedTime;

            _AnimationPlayer?.OnFinishAnimation(this);

            _AnimationPlayer = null;
        }
    }
}