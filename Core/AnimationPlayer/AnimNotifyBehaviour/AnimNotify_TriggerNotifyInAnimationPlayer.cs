using UnityEngine;

namespace StudioScor.Utilities
{
    public class AnimNotify_TriggerNotifyInAnimationPlayer : AnimNotifyBehaviour
    {
        [Header(" [ Trigger Notify In Animation Player ] ")]
        [SerializeField] private string _eventName;

        protected override void OnNotify(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(animator.gameObject.TryGetAnimationPlayer(out AnimationPlayer animationPlayer))
            {
                animationPlayer.AnimNotify(_eventName);
            }
        }
    }
}
