using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Diagnostics;

using StudioScor.GameplayTagSystem;

namespace StudioScor.EffectSystem
{
    public class GameplayEffectSystem : MonoBehaviour
    {
        #region Event
        public delegate void ChangeGameplayEffectHandler(GameplayEffectSystem gameplayEffectSystem, GameplayEffectSpec gameplayEffectSpec);
        #endregion

        [SerializeField] private GameplayTagComponent _GameplayTagComponent;

        private bool _WasSetup = false;
        private List<GameplayEffectSpec> _GameplayEffects;

        [SerializeField] private bool _UseDebug;

        public IReadOnlyList<GameplayEffectSpec> GameplayEffects
        {
            get
            {
                if (!_WasSetup)
                    Setup();

                return _GameplayEffects;
            }
        }

        public GameplayTagComponent GameplayTagComponent => _GameplayTagComponent;
        public bool UseDebug => _UseDebug;

        public event ChangeGameplayEffectHandler OnAddedGameplayEffect;
        public event ChangeGameplayEffectHandler OnRemovedGameplayEffect;

        #region EDITOR ONLY

#if UNITY_EDITOR
        private void Reset()
        {
            TryGetComponent(out _GameplayTagComponent);
        }
#endif

        [Conditional("UNITY_EDITOR")]
        protected void Log(object massage)
        {
#if UNITY_EDITOR
            if (UseDebug)
                UnityEngine.Debug.Log("GameplayEffect [ " + GetType() + " ]" + massage);
#endif
        }
        #endregion


        private void Awake()
        {
            if (!_WasSetup)
                Setup();
        }
        protected virtual void Setup()
        {
            _WasSetup = true;

            Log("Setup");

            _GameplayEffects = new();
        }

        public void ResetEffectSystem()
        {
            CancelAllGameplayEffect();

            _GameplayEffects.Clear();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            for (int i = GameplayEffects.Count - 1; i >= 0; i--)
            {
                GameplayEffects[i].OnUpdateEffect(deltaTime);
            }
        }

        public bool ContainGameplayEffect(GameplayEffect containEffect)
        {
            foreach (var effect in GameplayEffects)
            {
                if (effect.GameplayEffect == containEffect)
                {
                    return true;
                }
            }

            return false;
        }
        public bool TryGetGameplayEffectSpec(GameplayEffect containEffect, out GameplayEffectSpec spec)
        {
            foreach (var effect in GameplayEffects)
            {
                if (effect.GameplayEffect == containEffect)
                {
                    spec = effect;

                    return true;
                }
            }

            spec = null;

            return false;
        }


        public void ForceTakeGameplayEffect(GameplayEffectSpec effectSpec)
        {
            if (TryGetGameplayEffectSpec(effectSpec.GameplayEffect, out GameplayEffectSpec spec))
            {
                if (spec.TryOverrideEffect(effectSpec))
                {
                    Log("Override Effect");

                    return;
                }
            }

            effectSpec.ActivateEffect();

            _GameplayEffects.Add(effectSpec);

            OnAddEffect(effectSpec);
        }

        public void RemoveGameplayEffectSpec(GameplayEffectSpec spec)
        {
            if (spec.IsActivate)
            {
                spec.EndGameplayEffect();
            }

            _GameplayEffects.Remove(spec);
        }

        #region Cancel Effect
        public void CancelGameplayEffect(GameplayEffect effect)
        {
            if (effect is null)
                return;

            if (TryGetGameplayEffectSpec(effect, out GameplayEffectSpec spec))
            {
                CancelGamepalyEffect(spec);
            }
        }
        public void CancelGameplayEffect(GameplayEffectSpec effectSpec)
        {
            if (effectSpec is null)
                return;

            if (GameplayEffects.Contains(effectSpec))
            {
                CancelGamepalyEffect(effectSpec);
            }
        }
        public void CancelAllGameplayEffect()
        {
            for(int i = GameplayEffects.Count - 1; i >= 0; i--)
            {
                CancelGameplayEffect(GameplayEffects[i]);
            }
        }
        private void CancelGamepalyEffect(GameplayEffectSpec effectSpec)
        {
            effectSpec.EndGameplayEffect();
        }
        
        public void CancelGameplayEffectWithTags(GameplayTag[] removeTags)
        {
            if (removeTags is null)
                return;

            for (int i = GameplayEffects.Count - 1; i >= 0; i--)
            {
                foreach (var removeTag in removeTags)
                {
                    if (GameplayEffects[i].GameplayEffect.EffectTags.AssetTag == removeTag
                        || GameplayEffects[i].GameplayEffect.EffectTags.AttributeTags.Contains(removeTag)
                        || GameplayEffects[i].GameplayEffect.BaseEffectTags.Tags.AssetTag == removeTag
                        || GameplayEffects[i].GameplayEffect.BaseEffectTags.Tags.AttributeTags.Contains(removeTag))
                    {
                        CancelGamepalyEffect(GameplayEffects[i]);
                    }
                }
            }
        }

        #endregion

        #region Callback
        protected virtual void OnAddEffect(GameplayEffectSpec addedSpec)
        {
            Log("Add Effect - " + addedSpec);

            OnAddedGameplayEffect?.Invoke(this, addedSpec);
        }
        protected virtual void OnRemoveEffect(GameplayEffectSpec removedSpec)
        {
            Log("Add Effect - " + removedSpec);

            OnRemovedGameplayEffect?.Invoke(this, removedSpec);
        }
        #endregion

    }
}
