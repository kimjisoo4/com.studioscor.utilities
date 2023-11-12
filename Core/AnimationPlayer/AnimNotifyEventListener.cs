using UnityEngine;

namespace StudioScor.Utilities
{

    public class AnimNotifyEventListener : BaseMonoBehaviour
    {
        [Header(" [ Anim Notify Event Listener ] ")]
        [SerializeField] private AnimationPlayer _animationPlayer;

        private void Reset()
        {
#if UNITY_EDITOR
            gameObject.TryGetComponentInParentOrChildren(out _animationPlayer);
#endif
        }

        private void Awake()
        {
            if(!_animationPlayer)
            {
                if(!gameObject.TryGetComponentInParentOrChildren(out _animationPlayer))
                {
                    LogError("Animation Player Is NULL!!");
                }

            }
        }

        public void AnimNotify(string notify)
        {
            _animationPlayer.AnimNotify(notify);
        }
        public void AnimNotifyStateEnter(string notify)
        {
            _animationPlayer.AnimNotifyStateEnter(notify);
        }
        public void AnimNotifyStateExit(string notify)
        {
            _animationPlayer.AnimNotifyStateEnter(notify);
        }
    }
}