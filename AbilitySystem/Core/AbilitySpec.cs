using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using System.Diagnostics;
using System;

using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    #region Events
    public delegate void OnAbilityHandler(IAbilitySpec abilitySpec);
    public delegate void AbilityLevelHandler(IAbilitySpec abilitySpec, int currentLevel, int prevLevel);

#endregion


    public abstract class AbilitySpecWithMono : MonoBehaviour, IAbilitySpec
    {
        private Ability _Ability;
        private AbilitySystem _AbilitySystem;

        private int _Level = 0;
        private bool _IsPlaying = false;

        public event OnAbilityHandler OnActivatedAbility;
        public event OnAbilityHandler OnEndedAbility;
        public event OnAbilityHandler OnFinishedAbility;
        public event OnAbilityHandler OnCanceledAbility;
        public event AbilityLevelHandler OnChangedAbilityLevel;

        public Ability Ability => _Ability;
        public AbilitySystem AbilitySystem => _AbilitySystem;
        public GameplayTagComponent GameplayTagComponent => AbilitySystem.GameplayTagSystem;
        public GameplayTag AbilityTag => Ability.Tag;
        public IReadOnlyCollection<GameplayTag> AttributeTags => Ability.AttributeTags;
        public IReadOnlyCollection<GameplayTag> CancelTags => Ability.CancelTags;
        public int Level => _Level;
        public bool IsPlaying => _IsPlaying;

        #region EDITOR ONLY
        [Conditional("UNITY_EDITOR")]
        protected void Log(object massage)
        {
#if UNITY_EDITOR
            if (Ability.UseDebug)
                UnityEngine.Debug.Log(AbilitySystem.gameObject.name + " [ " + GetType().Name + " ] : " + massage, Ability);
#endif
        }
        #endregion

        public virtual void Setup(Ability ability, AbilitySystem abilitySystem, int level = 0)
        {
            _Ability = ability;
            _AbilitySystem = abilitySystem;
            _Level = level;
        }

        public void OnAddAbility()
        {
            GrantAbility();
        }
        public void OnRemoveAbility()
        {
            LostAbility();
        }

        public bool TryActiveAbility()
        {
            if (TryReTriggerAbility())
                return true;

            if (CanActiveAbility())
            {
                ForceActiveAbility();

                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnReleaseAbility()
        {
            if (!IsPlaying)
                return;

            ReleaseAbility();
        }
        public bool TryReTriggerAbility()
        {
            if (CanReTriggerAbility())
            {
                ForceReTriggerAbility();

                return true;
            }

            return false;
        }

        public bool TryCancelAbility(GameplayTag[] cancelTags)
        {
            if (!IsPlaying)
                return false;

            if (cancelTags.Contains(Ability.Tag))
            {
                ForceCancelAbility();

                return true;
            }

            foreach (GameplayTag tag in cancelTags)
            {
                if (AttributeTags.Contains(tag))
                {
                    ForceCancelAbility();

                    return true;
                }
            }

            return false;
        }

        public virtual void ForceActiveAbility()
        {
            Log(" On Ability ");

            AbilitySystem.OnCancelAbility(CancelTags.ToArray());

            _IsPlaying = true;

            EnterAbility();

            OnActivatedAbility?.Invoke(this);
        }

        public void ForceReTriggerAbility()
        {
            Log(" ReTrigger Ability ");

            ReTriggerAbility();
        }

        public virtual void EndAbility()
        {
            if (!IsPlaying)
                return;

            _IsPlaying = false;


            Log(" Finish Ability Ability ");

            FinishAbility();

            OnFinishedAbility?.Invoke(this);


            Log(" Exit Ability ");

            ExitAbility();

            OnEndedAbility?.Invoke(this);
        }
        public virtual void ForceCancelAbility()
        {
            if (!IsPlaying)
                return;

            _IsPlaying = false;


            Log(" Cancel Ability ");

            CancelAbility();

            OnCanceledAbility?.Invoke(this);


            Log(" Exit Ability ");

            ExitAbility();

            OnEndedAbility?.Invoke(this);
        }

        public void OnUpdateAbility(float deltaTime)
        {
            if (!IsPlaying)
                return;

            UpdateAbility(deltaTime);
        }
        public void OnFixedUpdateAbility(float deltaTime)
        {
            if (!IsPlaying)
                return;

            FixedUpdateAbility(deltaTime);
        }

        public void SetAbilityLevel(int newLevel)
        {
            if (Level == newLevel)
                return;

            int prevLevel = Level;

            _Level = newLevel;

            ChangeAbilityLevel(prevLevel);

            OnChangeAbilityLevel(prevLevel);
        }


        protected virtual void GrantAbility() { }
        protected virtual void LostAbility() { }
        public void OnOverrideAbility(int level) { }
        protected abstract void EnterAbility();
        protected virtual void ExitAbility() { }
        protected virtual void FinishAbility() { }
        protected virtual void CancelAbility() { }
        protected virtual void ReleaseAbility() { }
        protected virtual void ReTriggerAbility() { }
        public virtual void UpdateAbility(float deltaTime) { }
        public virtual void FixedUpdateAbility(float deltaTime) { }
        protected virtual void ChangeAbilityLevel(int prevLevel) { }

        public virtual bool CanReTriggerAbility()
        {
            return IsPlaying;
        }

        public virtual bool CanActiveAbility()
        {
            return true;
        }

        #region Callback
        protected virtual void OnActiveAbility()
        {
            Log("On Activated Ability");

            OnActivatedAbility?.Invoke(this);
        }
        protected virtual void OnFinishAbility()
        {
            Log("On Finished Ability");

            OnFinishedAbility?.Invoke(this);
        }
        protected virtual void OnCancelAbility()
        {
            Log("On Canceled Ability");

            OnCanceledAbility?.Invoke(this);
        }
        protected virtual void OnChangeAbilityLevel(int prevLevel)
        {
            Log("Level Change - Current Level : " + Level + " Prev Level : " + prevLevel);

            OnChangedAbilityLevel?.Invoke(this, Level, prevLevel);
        }

        #endregion
    }


    public abstract class AbilitySpec : IAbilitySpec
    {
        private readonly Ability _Ability;
        private readonly AbilitySystem _AbilitySystem;

        private int _Level = 0;
        private bool _IsPlaying = false;

        public event OnAbilityHandler OnActivatedAbility;
        public event OnAbilityHandler OnEndedAbility;
        public event OnAbilityHandler OnFinishedAbility;
        public event OnAbilityHandler OnCanceledAbility;
        public event AbilityLevelHandler OnChangedAbilityLevel;

        public Ability Ability => _Ability;
        public AbilitySystem  AbilitySystem => _AbilitySystem;
        public GameplayTagComponent GameplayTagComponent => AbilitySystem.GameplayTagSystem;
        public GameplayTag AbilityTag => Ability.Tag;
        public IReadOnlyCollection<GameplayTag> AttributeTags => Ability.AttributeTags;
        public IReadOnlyCollection<GameplayTag> CancelTags => Ability.CancelTags;
        public int Level => _Level;
        public bool IsPlaying => _IsPlaying;
        public AbilitySpec(Ability ability, AbilitySystem abilitySystem, int level)
        {
            _Ability = ability;
            _AbilitySystem = abilitySystem;
            _Level = level;
        }

        #region EDITOR ONLY
        [Conditional("UNITY_EDITOR")]
        protected void Log(object massage)
        {
#if UNITY_EDITOR
            if (Ability.UseDebug)
                UnityEngine.Debug.Log(AbilitySystem.gameObject.name + " [ " + GetType().Name + " ] : " + massage, Ability);
#endif
        }
        #endregion

        public void OnAddAbility()
        {
            GrantAbility();
        }
        public void OnRemoveAbility()
        {
            LostAbility();
        }

        public bool TryActiveAbility()
        {
            if (TryReTriggerAbility())
                return true;

            if (CanActiveAbility())
            {
                ForceActiveAbility();

                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnReleaseAbility()
        {
            if (!IsPlaying)
                return;

            ReleaseAbility();
        }
        public bool TryReTriggerAbility()
        {
            if (CanReTriggerAbility())
            {
                ForceReTriggerAbility();

                return true;
            }

            return false;
        }

        public bool TryCancelAbility(GameplayTag[] cancelTags)
        {
            if (!IsPlaying)
                return false;

            if (cancelTags.Contains(Ability.Tag))
            {
                ForceCancelAbility();

                return true;
            }

            foreach (GameplayTag tag in cancelTags)
            {
                if (AttributeTags.Contains(tag))
                {
                    ForceCancelAbility();

                    return true;
                }
            }

            return false;
        }

        public virtual void ForceActiveAbility()
        {
            Log(" On Ability ");

            AbilitySystem.OnCancelAbility(CancelTags.ToArray());

            _IsPlaying = true;

            EnterAbility();

            OnActivatedAbility?.Invoke(this);
        }

        public void ForceReTriggerAbility()
        {
            Log(" ReTrigger Ability ");

            ReTriggerAbility();
        }

        public virtual void EndAbility()
        {
            if (!IsPlaying)
                return;

            _IsPlaying = false;

            
            Log(" Finish Ability Ability ");

            FinishAbility();

            OnFinishedAbility?.Invoke(this);


            Log(" Exit Ability ");

            ExitAbility();

            OnEndedAbility?.Invoke(this);
        }
        public virtual void ForceCancelAbility()
        {
            if (!IsPlaying)
                return;

            _IsPlaying = false;


            Log(" Cancel Ability ");

            CancelAbility();

            OnCanceledAbility?.Invoke(this);


            Log(" Exit Ability ");

            ExitAbility();

            OnEndedAbility?.Invoke(this);
        }

        public void OnUpdateAbility(float deltaTime)
        {
            if (!IsPlaying)
                return;

            UpdateAbility(deltaTime);
        }
        public void OnFixedUpdateAbility(float deltaTime)
        {
            if (!IsPlaying)
                return;

            FixedUpdateAbility(deltaTime);
        }

        public void SetAbilityLevel(int newLevel)
        {
            if (Level == newLevel)
                return;

            int prevLevel = Level;

            _Level = newLevel;

            ChangeAbilityLevel(prevLevel);

            OnChangeAbilityLevel(prevLevel);
        }


        protected virtual void GrantAbility() { }
        protected virtual void LostAbility() { }



        public void OnOverrideAbility(int level) { }
        protected abstract void EnterAbility();
        protected virtual void ExitAbility() { }
        protected virtual void FinishAbility() { }
        protected virtual void CancelAbility() { }
        protected virtual void ReleaseAbility() { }
        protected virtual void ReTriggerAbility() { }
        public virtual void UpdateAbility(float deltaTime) { }
        public virtual void FixedUpdateAbility(float deltaTime) { }
        protected virtual void ChangeAbilityLevel(int prevLevel) { }

        public virtual bool CanReTriggerAbility()
        {
            return IsPlaying;
        }

        public virtual bool CanActiveAbility() 
        {
            return true;
        }

        #region Callback
        protected virtual void OnActiveAbility()
        {
            Log("On Activated Ability");

            OnActivatedAbility?.Invoke(this);
        }
        protected virtual void OnFinishAbility()
        {
            Log("On Finished Ability");

            OnFinishedAbility?.Invoke(this);
        }
        protected virtual void OnCancelAbility()
        {
            Log("On Canceled Ability");

            OnCanceledAbility?.Invoke(this);
        }
        protected virtual void OnChangeAbilityLevel(int prevLevel)
        {
            Log("Level Change - Current Level : " + Level + " Prev Level : " + prevLevel);

            OnChangedAbilityLevel?.Invoke(this, Level, prevLevel);
        }

        #endregion
    }
}
