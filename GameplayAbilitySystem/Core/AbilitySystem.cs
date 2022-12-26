using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Diagnostics;

using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{

    [RequireComponent(typeof(GameplayTagComponent))]
    public class AbilitySystem : MonoBehaviour
    {
        #region Events
        public delegate void AbilityChangedHandler(AbilitySystem abilitySystem, IAbilitySpec abilitySpec);
        #endregion

        [Header(" [ Reference ] ")]
        [SerializeField] private GameplayTagComponent _GameplayTagComponent;

        [Header(" [ Setup] ")]
        [SerializeField] private FAbility[] _InitializationAbilities;
        [SerializeField] private float _BufferDuration = 0.2f;

        [Header(" [ Use Debug ] ")]
        [SerializeField] private bool _UseDebug;

        private bool _WasSetup = false;

        private List<IAbilitySpec> _Abilities;
        private AbilityInputBuffer _AbilityInputBuffer;
        public GameplayTagComponent GameplayTagComponent
        {
            get
            {
                if (!_WasSetup)
                    Setup();

                return _GameplayTagComponent;
            }
        }
        public IReadOnlyList<IAbilitySpec> Abilities
        {
            get
            {
                if (!_WasSetup)
                    Setup();

                return _Abilities;
            }
        }
        public AbilityInputBuffer AbilityInputBuffer
        {
            get
            {
                if (!_WasSetup)
                    Setup();

                return _AbilityInputBuffer;
            }
        }

        public event AbilityChangedHandler OnStartedAbility;
        public event AbilityChangedHandler OnFinishedAbility;

        public event AbilityChangedHandler OnAddedAbility;
        public event AbilityChangedHandler OnRemovedAbility;

        #region EDITOR ONLY
#if UNITY_EDITOR
        private void Reset()
        {
            if (!GameplayTagComponent)
                TryGetComponent(out _GameplayTagComponent);
        }
#endif
        [Conditional("UNITY_EDITOR")]
        protected void Log(object content, bool isError = false)
        {
#if UNITY_EDITOR
            if(isError)
            {
                UnityEngine.Debug.LogError("Ability Sytstem [ " + transform.name + " ] : " + content, this);

                return;
            }

            if (_UseDebug)
                UnityEngine.Debug.Log("Ability Sytstem [ " + transform.name + " ] : " + content, this);
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
            if (_WasSetup)
                return;

            _WasSetup = true;

            if (!GameplayTagComponent)
            {
                if(!TryGetComponent(out _GameplayTagComponent))
                {
                    Log("GameplayTag System Is Null", true);
                }
            }

            _Abilities = new();
            _AbilityInputBuffer = new();

            foreach (var ability in _InitializationAbilities)
            {
                TryAddAbility(ability.Ability, ability.Level);
            }

            Log("Setup");
        }

        public void ResetAbilitySystem()
        {
            RemoveAllAbility();

            _Abilities.Clear();
            _AbilityInputBuffer.ResetAbilityInputBuffer();

            foreach (var ability in _InitializationAbilities)
            {
                TryAddAbility(ability.Ability, ability.Level);
            }
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            AbilityInputBuffer.Buffer(deltaTime);

            for (int i = 0; i < Abilities.Count; i++)
            {
                Abilities[i].OnUpdateAbility(deltaTime);
            }
        }

        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            for (int i = 0; i < Abilities.Count; i++)
            {
                Abilities[i].OnFixedUpdateAbility(deltaTime);
            }
        }

        #region Get Ability Spec
        public bool TryGetAbilitySpec(Ability abilitym, out IAbilitySpec spec)
        {
            spec = GetAbilitySpec(abilitym);

            return spec is not null;
        }
        public bool TryGetAbilitySpec(Type type, out IAbilitySpec abilitySpec)
        {
            abilitySpec = GetAbilitySpec(type);

            return abilitySpec is not null;
        }
        public bool TryGetAbilitySpec(GameplayTag abilityTag, out IAbilitySpec abilitySpec)
        {
            abilitySpec = GetAbilitySpec(abilityTag);

            return abilitySpec is not null;
        }

        public IAbilitySpec GetAbilitySpec(GameplayTag abilityTag)
        {
            foreach (IAbilitySpec spec in Abilities)
            {
                if (spec.Ability.Tag == abilityTag)
                {
                    return spec;
                }
            }

            return null;
        }

        public IAbilitySpec GetAbilitySpec(Ability ability)
        {
            foreach (var spec in Abilities)
            {
                if (spec.Ability == ability)
                {
                    return spec;
                }
            }

            return null;
        }
        public IAbilitySpec GetAbilitySpec(Type type)
        {
            foreach (var ability in Abilities)
            {
                if (ability.Ability.GetType() == type)
                {
                    return ability;
                }
            }

            return null;
        }
        

        #endregion
        #region Has Ability Spec
        public bool HasAbility(Ability ability)
        {
            return GetAbilitySpec(ability) is not null;
        }
        public bool HasAbility(GameplayTag abilityTag)
        {
            return GetAbilitySpec(abilityTag) is not null;
        }
        public bool HasAbility(Type abilityType)
        {
            return GetAbilitySpec(abilityType) is not null;
        }
        #endregion
        #region Can Activate Ability

        public bool CanActivateAbility(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                return spec.CanActiveAbility();
            }
            else
            {
                return false;
            }
        }
        public bool CanActiveAbility(GameplayTag abilityTag)
        {
            if (TryGetAbilitySpec(abilityTag, out IAbilitySpec spec))
            {
                return spec.CanActiveAbility();
            }
            else
            {
                return false;
            }
        }
        public bool CanActiveAbility(Type abilityType)
        {
            if (TryGetAbilitySpec(abilityType, out IAbilitySpec spec))
            {
                return spec.CanActiveAbility();
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region Is Playung Ability
        public bool IsPlayingAbility(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                return spec.IsPlaying;
            }
            else
            {
                return false;
            }
        }
        public bool IsPlayingAbility(GameplayTag abilityTag)
        {
            if (TryGetAbilitySpec(abilityTag, out IAbilitySpec spec))
            {
                return spec.IsPlaying;
            }
            else
            {
                return false;
            }
        }
        public bool IsPlayingAbility(Type type)
        {
            if (TryGetAbilitySpec(type, out var abilitySpec))
            {
                return abilitySpec.IsPlaying;
            }

            return false;
        }
        #endregion


        #region Activate Ability Spec
        public bool TryActivateAbility(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                return spec.TryActiveAbility();
            }
            else
            {
                return false;
            }
        }
        public void TryActivateAbilityAsInputBuffer(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                if (!spec.TryActiveAbility())
                {
                    AbilityInputBuffer.SetBuffer(spec, _BufferDuration);
                }
            }
        }
        public bool TryActivateAbility(GameplayTag abilityTag)
        {
            if (TryGetAbilitySpec(abilityTag, out IAbilitySpec spec))
            {
                return spec.TryActiveAbility();
            }

            return false;
        }

        public void ForceActivateAbility(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                spec.ForceActiveAbility();
            }
        }
        #endregion
        #region Released Ability Spec
        public void OnReleasedAbility(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                if(spec.IsPlaying)
                    spec.OnReleaseAbility();
            }
        }
        public void OnReleasedAbility(GameplayTag abilityTag)
        {
            if (TryGetAbilitySpec(abilityTag, out IAbilitySpec spec))
            {
                if (spec.IsPlaying)
                    spec.OnReleaseAbility();
            }
        }
        #endregion
        #region Canceled Ability Spec
        public void CancelAbility(GameplayTag[] cancelTags)
        {
            foreach (IAbilitySpec spec in Abilities)
            {
                spec.TryCancelAbility(cancelTags);
            }
        }
        public void CancelAbility(IReadOnlyCollection<GameplayTag> cancelTags)
        {
            GameplayTag[] tags = cancelTags.ToArray();

            foreach (IAbilitySpec spec in Abilities)
            {
                spec.TryCancelAbility(tags);
            }
        }
        public void CancelAllAbility()
        {
            foreach (var spec in Abilities)
            {
                spec.ForceCancelAbility();
            }
        }
        #endregion

        #region Add Ability
        public bool TryAddAbility(Ability addAbility, int level = 0)
        {
            if (!CanAddAbility(addAbility, level))
                return false;

            ForceAddAbility(addAbility, level);

            return true;
        }
        public virtual bool CanAddAbility(Ability ability, int level = 0)
        {
            if (!ability)
            {
                Log(ability + " is Null");
                
                return false;
            }

            return true;
        }
        public void ForceAddAbility(Ability ability, int level = 0)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                spec.OnOverrideAbility(level);

                return;
            }

            var newAbilitySpec = ability.CreateSpec(this, level);

            newAbilitySpec.OnAddAbility();

            _Abilities.Add(newAbilitySpec);

            newAbilitySpec.OnActivatedAbility += Spec_OnStartedAbility;
            newAbilitySpec.OnFinishedAbility += Spec_OnFinishedAbility;

            OnAddAbility(newAbilitySpec);
        }
        #endregion
        #region Remove Ability
        public bool TryRemoveAbility(Ability ability)
        {
            if (!CanRemoveAbility(ability))
                return false;

            ForceRemoveAbility(ability);

            return true;
        }

        public virtual bool CanRemoveAbility(Ability ability)
        {
            if (!ability)
            {
                Log(ability + "is Null");

                return false;
            }

            if (!HasAbility(ability))
            {
                Log("Has Not " + ability);

                return false;
            }

            return true;

        }
        public void ForceRemoveAbility(Ability ability)
        {
            var spec = GetAbilitySpec(ability);

            RemoveAbility(spec);
        }
        private void RemoveAbility(IAbilitySpec abilitySpec)
        {
            if (abilitySpec.IsPlaying)
            {
                abilitySpec.ForceCancelAbility();
            }

            _Abilities.Remove(abilitySpec);

            abilitySpec.OnActivatedAbility -= Spec_OnStartedAbility;
            abilitySpec.OnFinishedAbility -= Spec_OnFinishedAbility;

            abilitySpec.OnRemoveAbility();

            OnRemoveAbility(abilitySpec);
        }
        public void RemoveAllAbility()
        {
            for(int i = Abilities.Count - 1; i >= 0; i--)
            {
                RemoveAbility(Abilities[i]);
            }
        }

        #endregion

        #region Ability Spec Delegate
        private void Spec_OnFinishedAbility(IAbilitySpec abilitySpec)
        {
            OnFinishAbility(abilitySpec);
        }
        private void Spec_OnStartedAbility(IAbilitySpec abilitySpec)
        {
            OnStartAbility(abilitySpec);
        }
        #endregion
        #region CallBack
        protected virtual void OnAddAbility(IAbilitySpec addAbilitySpec)
        {
            Log("On Added Ability - " + addAbilitySpec);

            OnAddedAbility?.Invoke(this, addAbilitySpec);
        }
        protected virtual void OnRemoveAbility(IAbilitySpec removeAbilitySpec)
        {
            Log("On Removed Ability - " + removeAbilitySpec);

            OnRemovedAbility?.Invoke(this, removeAbilitySpec);
        }
        protected virtual void OnFinishAbility(IAbilitySpec abilitySpec)
        {
            Log("On Finished Ability - " + abilitySpec);

            OnFinishedAbility?.Invoke(this, abilitySpec);
        }
        protected virtual void OnStartAbility(IAbilitySpec abilitySpec)
        {
            Log("On Started Ability - " + abilitySpec);

            OnStartedAbility?.Invoke(this, abilitySpec);
        }
        #endregion
    }
}
