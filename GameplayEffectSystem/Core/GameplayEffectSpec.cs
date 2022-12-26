using UnityEngine;
using System.Linq;
using System.Diagnostics;
using UnityEngine.Events;

using StudioScor.GameplayTagSystem;
namespace StudioScor.EffectSystem
{
    [System.Serializable]
    public abstract class GameplayEffectSpec
    {
        #region Events
        public delegate void GameplayEffectSpecState(GameplayEffectSpec gameplayEffectSpec);
        #endregion
        private readonly GameplayEffect _GameplayEffect;
        private readonly GameplayEffectSystem _GameplayEffectSystem;

        private bool _Activate = false;
        private bool _IsApply = false;
        protected int _Level;
        protected object _Data; 

        public GameplayEffect GameplayEffect => _GameplayEffect;
        public GameplayEffectSystem GameplayEffectSystem => _GameplayEffectSystem;
        public GameplayTagComponent GameplayTagComponent => GameplayEffectSystem.GameplayTagComponent;
        public FGameplayEffectTags BaseEffectTags => GameplayEffect.BaseEffectTags.Tags;
        public FGameplayEffectTags EffectTags => GameplayEffect.EffectTags;
        public bool IsActivate => _Activate;
        public bool IsApply => _IsApply;
        public int Level => _Level;
        public object Data => _Data;

        public GameplayEffectSpec(GameplayEffect gameplayEffect, GameplayEffectSystem gameplayEffectSystem, int level = 0, object data = default)
        {
            _GameplayEffect = gameplayEffect;
            _GameplayEffectSystem = gameplayEffectSystem;
            _Level = level;
            _Data = data;
        }

        #region EDITOR ONLY
        [Conditional("UNITY_EDITOR")]
        protected void Log(object massage)
        {
#if UNITY_EDITOR
            if (GameplayEffect.UseDebug)
                UnityEngine.Debug.Log(GameplayEffectSystem.gameObject.name + " [ " + GetType().Name + " ]" + massage, GameplayEffect);
#endif
        }
        #endregion


        #region OverrideEffect
        public bool TryOverrideEffect(GameplayEffectSpec gameplayEffectSpec)
        {
            if (!CanOverrideEffect(gameplayEffectSpec))
                return false;

            Log("Override Effect");

            OnOverrideEffect(gameplayEffectSpec);

            return true;
        }
        public virtual bool CanOverrideEffect(GameplayEffectSpec gameplayEffectSpec)
        {
            return false;
        }
        public virtual void OnOverrideEffect(GameplayEffectSpec gameplayEffectSpec) { }

        #endregion


        public void ActivateEffect()
        {
            if (IsActivate) 
                return;

            _Activate = true;

            GameplayEffectSystem.CancelGameplayEffectWithTags(EffectTags.RemoveGameplayEffectsAsTags);
            GameplayEffectSystem.CancelGameplayEffectWithTags(BaseEffectTags.RemoveGameplayEffectsAsTags);

            GameplayEffectSystem.GameplayTagComponent.AddOwnedTags(EffectTags.ActivateGrantedTags.Owneds);
            GameplayEffectSystem.GameplayTagComponent.AddOwnedTags(BaseEffectTags.ActivateGrantedTags.Owneds);

            GameplayEffectSystem.GameplayTagComponent.AddBlockTags(EffectTags.ActivateGrantedTags.Blocks);
            GameplayEffectSystem.GameplayTagComponent.AddBlockTags(BaseEffectTags.ActivateGrantedTags.Blocks);

            if (HasApplyConditionTag())
            {
                GameplayEffectSystem.GameplayTagComponent.OnAddedNewOwnedTag += GameplayTagSystem_OnUpdateOwnedTag;
                GameplayEffectSystem.GameplayTagComponent.OnRemovedOwnedTag += GameplayTagSystem_OnUpdateOwnedTag;
            }

            EnterEffect();

            Log("Activate Effect");

            if (CanApplyGameplayEffect())
                OnApplyEffect();
        }

        public void OnUpdateEffect(float deltaTime)
        {
            if (!IsActivate)
            {
                GameplayEffectSystem.RemoveGameplayEffectSpec(this);

                return;
            }

            UpdateEffect(deltaTime);

            if (IsApply)
                UpdateApplyEffect(deltaTime);
        }

        private void GameplayTagSystem_OnUpdateOwnedTag(GameplayTagComponent gameplayTagComponent, GameplayTag changedTag)
        {
            if (EffectTags.ApplyConditionTags.Obstacleds.Contains(changedTag) || EffectTags.ApplyConditionTags.Requireds.Contains(changedTag)
             || BaseEffectTags.ApplyConditionTags.Obstacleds.Contains(changedTag) || BaseEffectTags.ApplyConditionTags.Requireds.Contains(changedTag))
            {
                TryApplyEffect();
            }
        }

        public void EndGameplayEffect()
        {
            if (!IsActivate)
            {
                return;
            }

            _Activate = false;

            OnIgnoreEffect();

            if (HasApplyConditionTag())
            {
                GameplayEffectSystem.GameplayTagComponent.OnAddedNewOwnedTag -= GameplayTagSystem_OnUpdateOwnedTag;
                GameplayEffectSystem.GameplayTagComponent.OnRemovedOwnedTag -= GameplayTagSystem_OnUpdateOwnedTag;
            }

            GameplayEffectSystem.GameplayTagComponent.RemoveOwnedTags(EffectTags.ActivateGrantedTags.Owneds);
            GameplayEffectSystem.GameplayTagComponent.RemoveOwnedTags(BaseEffectTags.ActivateGrantedTags.Owneds);
            GameplayEffectSystem.GameplayTagComponent.RemoveBlockTags(EffectTags.ActivateGrantedTags.Blocks);
            GameplayEffectSystem.GameplayTagComponent.RemoveBlockTags(BaseEffectTags.ActivateGrantedTags.Blocks);

            ExitEffect();

            Log("End Effect");
        }

        protected bool TryApplyEffect()
        {
            if (CanApplyGameplayEffect())
            {
                OnApplyEffect();

                return true;
            }
            else
            {
                OnIgnoreEffect();

                return false;
            }
        }

        protected void OnApplyEffect()
        {
            if (IsApply)
                return;

            _IsApply = true;

            GameplayEffectSystem.GameplayTagComponent.AddOwnedTags(EffectTags.ApplyGrantedTags.Owneds);
            GameplayEffectSystem.GameplayTagComponent.AddOwnedTags(BaseEffectTags.ApplyGrantedTags.Owneds);
            GameplayEffectSystem.GameplayTagComponent.AddBlockTags(EffectTags.ApplyGrantedTags.Blocks);
            GameplayEffectSystem.GameplayTagComponent.AddBlockTags(BaseEffectTags.ApplyGrantedTags.Blocks);

            ApplyEffect();

            Log("Apply Effect");
        }
        protected void OnIgnoreEffect()
        {
            if (!IsApply)
                return;
            
            _IsApply = false;


            GameplayEffectSystem.GameplayTagComponent.RemoveOwnedTags(EffectTags.ApplyGrantedTags.Owneds);
            GameplayEffectSystem.GameplayTagComponent.RemoveOwnedTags(BaseEffectTags.ApplyGrantedTags.Owneds);
            GameplayEffectSystem.GameplayTagComponent.RemoveBlockTags(EffectTags.ApplyGrantedTags.Blocks);
            GameplayEffectSystem.GameplayTagComponent.RemoveBlockTags(BaseEffectTags.ApplyGrantedTags.Blocks);

            IgnoreEffect();

            Log("Ignore Effect");
        }

        protected virtual bool CanApplyGameplayEffect()
        {
            return HasRequiredApplyTags() && !HasObstacledApplyTags();
        }

        protected bool HasApplyConditionTag()
        {
            return EffectTags.ApplyConditionTags.Requireds.Length > 0 || EffectTags.ApplyConditionTags.Obstacleds.Length > 0
                || BaseEffectTags.ApplyConditionTags.Requireds.Length > 0 || BaseEffectTags.ApplyConditionTags.Obstacleds.Length > 0;
        }
        protected bool HasRequiredApplyTags()
        {
            return (EffectTags.ApplyConditionTags.Requireds is null || GameplayTagComponent.ContainAllTagsInOwned(EffectTags.ApplyConditionTags.Requireds))
                && (BaseEffectTags.ApplyConditionTags.Requireds is null || GameplayTagComponent.ContainAllTagsInOwned(BaseEffectTags.ApplyConditionTags.Requireds));
        }
        protected bool HasObstacledApplyTags()
        {
            return (EffectTags.ApplyConditionTags.Obstacleds is not null && GameplayTagComponent.ContainAnyTagsInOwned(EffectTags.ApplyConditionTags.Obstacleds))
                || (BaseEffectTags.ApplyConditionTags.Obstacleds is not null && GameplayTagComponent.ContainAnyTagsInOwned(BaseEffectTags.ApplyConditionTags.Obstacleds));
        }

        protected abstract void EnterEffect();
        protected abstract void UpdateEffect(float deltaTime);
        protected virtual void UpdateApplyEffect(float deltaTime) { }
        protected virtual void ExitEffect() { }
        protected virtual void ApplyEffect() { }
        protected virtual void IgnoreEffect() { }
        
    }
}
