using UnityEngine;

namespace StudioScor.Utilities
{
    public class AnimNotifyEventListener : BaseMonoBehaviour
    {
        [Header(" [ Anim Notify Event Listener ] ")]
        [SerializeField] private AnimationPlayer _AnimationPlayer;


        private void Reset()
        {
            gameObject.TryGetComponentInParentOrChildren(out _AnimationPlayer);
        }

        private void Awake()
        {
            if(!_AnimationPlayer)
            {
                if(!gameObject.TryGetComponentInParentOrChildren(out _AnimationPlayer))
                {
                    Log("Animation Player Is NULL!!", true);
                }

            }
        }

        public void AnimNotify(string notify)
        {
            _AnimationPlayer.AnimNotify(notify);
        }
        public void AnimNotifyStateEnter(string notify)
        {
            _AnimationPlayer.AnimNotifyStateEnter(notify);
        }
        public void AnimNotifyStateExit(string notify)
        {
            _AnimationPlayer.AnimNotifyStateEnter(notify);
        }
    }
}