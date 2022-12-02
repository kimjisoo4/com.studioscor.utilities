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

        public GameplayEffect GameplayEffect => _GameplayEffect;
        public GameplayEffectSystem GameplayEffectSystem => _GameplayEffectSystem;
        public GameplayTagComponent GameplayTagComponent => GameplayEffectSystem.GameplayTagComponent;

        public bool IsActivate => _Activate;
        public bool IsApply => _IsApply;
        public int Level => _Level;


        public event GameplayEffectSpecState OnFinishedGameplayEffect;
        public GameplayEffectSpec(GameplayEffect gameplayEffect, GameplayEffectSystem gameplayEffectSystem, int level = 0)
        {
            _GameplayEffect = gameplayEffect;
            _GameplayEffectSystem = gameplayEffectSystem;
            _Level = level;
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

            GameplayEffectSystem.CancelGameplayEffectWithTags(GameplayEffect.EffectTags.RemoveGameplayEffectsAsTags);

            GameplayEffectSystem.GameplayTagComponent.AddOwnedTags(GameplayEffect.EffectTags.ActivateGrantedTags);

            if (GameplayEffect.EffectTags.ApplyRequiredTags.Length > 0 || GameplayEffect.EffectTags.ApplyObstacledTags.Length > 0)
            {
                GameplayEffectSystem.GameplayTagComponent.OnAddedNewOwnedTag += GameplayTagSystem_OnUpdateOwnedTag;
                GameplayEffectSystem.GameplayTagComponent.OnRemovedOwnedTag += GameplayTagSystem_OnUpdateOwnedTag;
            }


            EnterEffect();
                
            if (CanApplyGameplayEffect())
                OnApplyEffect();
        }

        public void OnUpdateEffect(float deltaTime)
        {
            if (!IsActivate)
                return;

            UpdateEffect(deltaTime);

            if (IsApply)
                UpdateApplyEffect(deltaTime);
        }

        private void GameplayTagSystem_OnUpdateOwnedTag(GameplayTagComponent gameplayTagComponent, GameplayTag changedTag)
        {
            if (_GameplayEffect.EffectTags.ApplyObstacledTags.Contains(changedTag) 
                || _GameplayEffect.EffectTags.ApplyRequiredTags.Contains(changedTag))
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


            if (GameplayEffect.EffectTags.ApplyRequiredTags.Length > 0 || GameplayEffect.EffectTags.ApplyObstacledTags.Length > 0)
            {
                GameplayEffectSystem.GameplayTagComponent.OnAddedNewOwnedTag -= GameplayTagSystem_OnUpdateOwnedTag;
                GameplayEffectSystem.GameplayTagComponent.OnRemovedOwnedTag -= GameplayTagSystem_OnUpdateOwnedTag;
            }

            GameplayEffectSystem.GameplayTagComponent.RemoveOwnedTags(GameplayEffect.EffectTags.ActivateGrantedTags);

            ExitEffect();
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

            GameplayTagComponent.AddOwnedTags(_GameplayEffect.EffectTags.ApplyGrantedTags);

            ApplyEffect();
        }
        protected void OnIgnoreEffect()
        {
            if (!IsApply)
                return;
            
            _IsApply = false;

            GameplayTagComponent.RemoveOwnedTags(_GameplayEffect.EffectTags.ApplyGrantedTags);

            IgnoreEffect();
        }

        protected virtual bool CanApplyGameplayEffect()
        {
            return HasRequiredApplyTags() && !HasObstacledApplyTags();
        }
        protected bool HasRequiredApplyTags()
        {
            return GameplayEffect.EffectTags.ApplyRequiredTags is null
               || GameplayEffectSystem.GameplayTagComponent.ContainAllTagsInOwned(GameplayEffect.EffectTags.ApplyRequiredTags);
        }
        protected bool HasObstacledApplyTags()
        {
            return GameplayEffect.EffectTags.ApplyObstacledTags is not null
               && GameplayEffectSystem.GameplayTagComponent.ContainAnyTagsInOwned(GameplayEffect.EffectTags.ApplyObstacledTags);
        }


        protected abstract void EnterEffect();
        protected abstract void UpdateEffect(float deltaTime);
        protected virtual void UpdateApplyEffect(float deltaTime) { }
        protected virtual void ExitEffect() { }
        protected virtual void ApplyEffect() { }
        protected virtual void IgnoreEffect() { }
    }
}
