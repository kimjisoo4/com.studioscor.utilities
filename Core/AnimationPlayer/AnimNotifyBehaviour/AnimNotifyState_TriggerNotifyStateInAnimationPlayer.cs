using UnityEngine;

namespace StudioScor.Utilities
{
    public class AnimNotifyState_TriggerNotifyStateInAnimationPlayer : AnimNotifyStateBehaviour
    {
        [Header(" [ Trigger Notify In Animation Player ] ")]
        [SerializeField] private string _eventName;

        protected override void OnEnterNotify(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.gameObject.TryGetAnimationPlayer(out AnimationPlayer animationPlayer))
            {
                animationPlayer.AnimNotifyStateEnter(_eventName);
            }
        }

        protected override void OnExitNotify(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.gameObject.TryGetAnimationPlayer(out AnimationPlayer animationPlayer))
            {
                animationPlayer.AnimNotifyStateExit(_eventName);
            }
        }
    }
}
