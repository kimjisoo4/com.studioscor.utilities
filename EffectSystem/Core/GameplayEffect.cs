using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

using StudioScor.GameplayTagSystem;
namespace StudioScor.EffectSystem
{
    public abstract class GameplayEffect : ScriptableObject
    {
        #region 태그
#if ODIN_INSPECTOR
        [TabGroup("Condition")]
        [InlineProperty]
        [HideLabel]
#else
        [field: Header("이펙트의 태그")]
#endif
        #endregion
        [field: SerializeField] public FGameplayEffectTags EffectTags { get; private set; }

        #region 디버그
#if ODIN_INSPECTOR
        [TabGroup("Setting")]
        [Title("Debug Mode")]
#else
        [field: Header(" 디버그 ")]
#endif
        #endregion

        [field: SerializeField] public bool UseDebug { get; private set; } = false;

        public bool TryTakeEffect(Transform target, int level = 0, object data = null)
        {
            if(target.TryGetComponent(out GameplayTagComponent gameplayTagComponent))
            {
                return TryTakeEffect(gameplayTagComponent, level, data);
            }

            return false;
        }

        public bool TryTakeEffect(GameplayTagComponent target, int level = 0, object data = null)
        {
            if (!CanTakeEffect(target, level, data))
                return false;

            OnTakeEffect(target, level, data);

            return true;
        }
        public virtual bool CanTakeEffect(GameplayTagComponent target, int level = 0, object data = null)
        {
            if (target is null)
                return false;

            if (!HasRequiredTags(target) && HasObstacledTags(target))
                return false;

            return true;
        }
        public abstract void OnTakeEffect(GameplayTagComponent target, int level = 0, object data = null);

        protected bool HasRequiredTags(GameplayTagComponent gameplayTagComponent)
        {
            return EffectTags.ActivateRequiredTags is null
                || gameplayTagComponent.ContainAllTagsInOwned(EffectTags.ActivateRequiredTags);
        }
        protected bool HasObstacledTags(GameplayTagComponent gameplayTagComponent)
        {
            return EffectTags.ActivateObstacledeTags is not null
              && gameplayTagComponent.ContainAnyTagsInOwned(EffectTags.ActivateObstacledeTags);
        }
    }
}

