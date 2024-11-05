using UnityEngine;

namespace StudioScor.Utilities
{

    public class AnimNotifyStateBehaviour : BaseStateMachineBehaviour
    {
        [Header(" [ Anim Notify State Behaviour ] ")]
        [SerializeField][Range(0f, 1f)] private float _startTime = 0.2f;
        [SerializeField][Range(0f, 1f)] private float _endTime = 0.8f;

        private bool _wasStarted = false;
        private bool _wasEnded = false;

        private void OnValidate()
        {
            if (_startTime > _endTime)
            {
                _endTime = _startTime;
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Log($"{nameof(OnStateEnter).ToBold().ToColor(SUtility.STRING_COLOR_WHITE)}",animator);

            _wasStarted = false;
            _wasEnded = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            Log($"{nameof(OnStateUpdate).ToBold()}", animator);

            float normalizedTime = stateInfo.normalizedTime % 1;

            if(!_wasStarted)
            {
                if(normalizedTime >= _startTime)
                {
                    EnterNotify(animator, stateInfo, layerIndex);
                }
            }
            else
            {
                if(normalizedTime >= _endTime || normalizedTime < _startTime)
                {
                    ExitNotify(animator, stateInfo, layerIndex);
                }
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            Log($"{nameof(OnStateExit).ToBold().ToColor(SUtility.STRING_COLOR_GREY)}", animator);

            ExitNotify(animator, stateInfo, layerIndex);
        }

        protected virtual void EnterNotify(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_wasStarted)
                return;

            _wasStarted = true;

            Log($"{nameof(OnStateExit).ToBold().ToColor(SUtility.STRING_COLOR_WHITE)}", animator);

            OnEnterNotify(animator, stateInfo, layerIndex);
        }
        protected virtual void ExitNotify(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_wasEnded)
                return;

            _wasEnded = true;

            Log($"{nameof(OnStateExit).ToBold().ToColor(SUtility.STRING_COLOR_GREY)}", animator);

            OnExitNotify(animator, stateInfo, layerIndex);
        }


        protected virtual void OnEnterNotify(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
        protected virtual void OnExitNotify(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    }
}
