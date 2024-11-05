using UnityEngine;

namespace StudioScor.Utilities
{


    public abstract class AnimNotifyBehaviour : BaseStateMachineBehaviour
    {
        [Header(" [ Anim Notify Behaviour ] ")]
        [SerializeField][Range(0f, 1f)] private float _triggerTime = 0.2f;
        [SerializeField] private bool _useLoop = false;

        private bool _wasTrigger = false;
        private int _loopCount = 0;


        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _wasTrigger = false;
            _loopCount = 0;

        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if(_useLoop)
            {
                int loopCount = Mathf.FloorToInt(stateInfo.normalizedTime);

                if (_loopCount != loopCount)
                {
                    _wasTrigger = false;
                    _loopCount = loopCount;
                }
            }

            if (!_wasTrigger)
            {
                float normalizedTime = stateInfo.normalizedTime % 1f;

                if(normalizedTime >= _triggerTime)
                {
                    NotifyTrigger(animator, stateInfo, layerIndex);
                }
            }
        }

        protected virtual void NotifyTrigger(Animator animator,AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_wasTrigger)
                return;

            _wasTrigger = true;

            Log(nameof(OnNotify).ToBold(), animator);

            OnNotify(animator, stateInfo, layerIndex);
        }
        protected virtual void OnNotify(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    }
}
