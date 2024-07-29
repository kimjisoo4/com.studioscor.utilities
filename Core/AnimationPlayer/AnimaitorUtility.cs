using UnityEngine;

namespace StudioScor.Utilities
{
    public static class AnimaitorUtility
    {
        public static AnimationPlayer GetAnimationPlayer(this GameObject gameObject)
        {
            return gameObject.GetComponent<AnimationPlayer>();
        }
        public static AnimationPlayer GetAnimationPlayer(this Component component)
        {
            return component.GetComponent<AnimationPlayer>();
        }
        public static bool TryGetAnimationPlayer(this GameObject gameObject, out AnimationPlayer animationPlayer)
        {
            return gameObject.TryGetComponent(out animationPlayer);
        }
        public static bool TryGetAnimationPlayer(this Component component, out AnimationPlayer animationPlayer)
        {
            return component.TryGetComponent(out animationPlayer);
        }

        
        public static bool IsPlayingAnimatorState(this Animator animator, int layer, string stateName)
        {
            int hash = Animator.StringToHash(stateName);

            return animator.IsPlayingAnimatorState(layer, hash);
        }
        public static bool IsPlayingAnimatorState(this Animator animator, int layer, int hash)
        {
            var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(layer);

            if (animatorStateInfo.shortNameHash == hash)
            {
                return true;
            }
            else
            {
                animatorStateInfo = animator.GetNextAnimatorStateInfo(layer);

                if (animatorStateInfo.shortNameHash == hash)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetAnimatorState(this Animator animator, int layer, string stateName, out AnimatorStateInfo animatorStateInfo)
        {
            int hash = Animator.StringToHash(stateName);

            return animator.TryGetAnimatorState(layer, hash, out animatorStateInfo);
        }

        public static bool TryGetAnimatorState(this Animator animator, int layer, int hash, out AnimatorStateInfo animatorStateInfo)
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(layer);

            if (animatorStateInfo.shortNameHash == hash)
            {
                return true;
            }
            else
            {
                animatorStateInfo = animator.GetNextAnimatorStateInfo(layer);

                if(animatorStateInfo.shortNameHash == hash)
                {
                    return true;
                }
            }

            animatorStateInfo = default;
            return false;
        }
    }
}