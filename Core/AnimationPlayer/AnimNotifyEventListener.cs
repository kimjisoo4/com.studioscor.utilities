using UnityEngine;

namespace StudioScor.Utilities
{

    public class AnimNotifyEventListener : BaseMonoBehaviour
    {
        [Header(" [ Anim Notify Event Listener ] ")]
        [SerializeField] private AnimationPlayer animationPlayer;


        private void Reset()
        {
            gameObject.TryGetComponentInParentOrChildren(out animationPlayer);
        }

        private void Awake()
        {
            if(!animationPlayer)
            {
                if(!gameObject.TryGetComponentInParentOrChildren(out animationPlayer))
                {
                    Log("Animation Player Is NULL!!", true);
                }

            }
        }

        public void AnimNotify(string notify)
        {
            animationPlayer.AnimNotify(notify);
        }
        public void AnimNotifyStateEnter(string notify)
        {
            animationPlayer.AnimNotifyStateEnter(notify);
        }
        public void AnimNotifyStateExit(string notify)
        {
            animationPlayer.AnimNotifyStateEnter(notify);
        }
    }
}