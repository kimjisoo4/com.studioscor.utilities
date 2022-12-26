using UnityEngine;
using System;
using UnityEditor;

using StudioScor.GameplayTagSystem;
namespace StudioScor.EffectSystem
{
    public abstract class GameplayEffect : ScriptableObject
    {
#if UNITY_EDITOR
        public bool AutoReName = false;
        public string FileName;

        private void OnValidate()
        {
            if (AutoReName && name != FileName)
            {
                name = FileName;
            }
        }

        public void ResetEffectTags()
        {
            EffectTags = new();
        }
#endif

        [field: Header(" [ Gameplay Effect ] ")]
        [field: SerializeField] public BaseEffectTags BaseEffectTags { get; private set; }

        [field: SerializeField, ContextMenuItem("Reset Effect Tags", "ResetEffectTags")] public FGameplayEffectTags EffectTags { get; private set; }


        [field: Header(" [ Debug ] ")]
        [field: SerializeField] public bool UseDebug { get; private set; } = false;

        public bool TryTakeEffect(GameplayEffectSystem target, int level = 0, object data = null)
        {
            if (!CanTakeEffect(target, level, data))
                return false;

            OnTakeEffect(target, level, data);

            return true;
        }
        public virtual bool CanTakeEffect(GameplayEffectSystem target, int level = 0, object data = null)
        {
            if (target is null)
                return false;

            if (!HasRequiredTags(target.GameplayTagComponent) 
                || HasObstacledTags(target.GameplayTagComponent))
                return false;

            return true;
        }
        public abstract void OnTakeEffect(GameplayEffectSystem target, int level = 0, object data = null);

        protected bool HasRequiredTags(GameplayTagComponent gameplayTagComponent)
        {
            return (EffectTags.ActivateConditionTags.Requireds is null || gameplayTagComponent.ContainAllTagsInOwned(EffectTags.ActivateConditionTags.Requireds))
                && (BaseEffectTags.Tags.ActivateConditionTags.Requireds is null || gameplayTagComponent.ContainAllTagsInOwned(BaseEffectTags.Tags.ActivateConditionTags.Requireds));
        }
        protected bool HasObstacledTags(GameplayTagComponent gameplayTagComponent)
        {
            return (EffectTags.ActivateConditionTags.Obstacleds is not null && gameplayTagComponent.ContainAnyTagsInOwned(EffectTags.ActivateConditionTags.Obstacleds))
                || (BaseEffectTags.Tags.ActivateConditionTags.Obstacleds is not null && gameplayTagComponent.ContainAnyTagsInOwned(BaseEffectTags.Tags.ActivateConditionTags.Obstacleds));
        }
    }
}

