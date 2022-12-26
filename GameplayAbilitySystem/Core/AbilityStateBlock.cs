using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using StudioScor.Utilities;
using StudioScor.GameplayTagSystem;
using System.Diagnostics;


namespace StudioScor.AbilitySystem
{
    public abstract class AbilityStateBlock : ScriptableObject
    {
        #region EDITOR ONLY
#if UNITY_EDITOR
        public string NewName = "State";

        private void OnValidate()
        {
            if(this.name != NewName)
                this.name = NewName;
        }
#endif
        #endregion

        [field: Header(" [ Ability State ] ")]
        [field: SerializeField] public FConditionTags ConditionTags { get; private set; }
        [field: SerializeField] public FGameplayTags GrantTags { get; private set; }

        [field: Header(" [ Use Debug ] ")]
        [field: SerializeField] public bool UseDebug { get; private set; } = false;

        public abstract AbilityStateBlockBaseSpec CreateSpec(IAbilityState abilityState);
    }

    public interface IAbilityState
    {
        public IAbilitySpec AbilitySpec { get; }
        public FiniteStateMachineSystem<AbilityStateBlockSpec> StateMachine { get; }
        public void OnFinishState();
    }

    public class AbilityStateBlockSpec : BaseStateClass
    {
        protected override void EnterState()
        {
        }

        public void OnUpdateState(float delatTime)
        {
            UpdateState(delatTime);
        }
        public void OnFixedUpdateState(float deltaTime)
        {
            FixedUpdateState(deltaTime);
        }
        protected virtual void UpdateState(float delatTime) { }
        protected virtual void FixedUpdateState(float delatTime) { }
    }

    public class AbilityStateBlockBaseSpec : AbilityStateBlockSpec
    {
        protected readonly GameplayTagComponent _GameplayTagComponent;
        protected readonly AbilityStateBlock _AbilityStateBlock;
        protected readonly IAbilityState _AbilityState;

        protected override bool UseDebug => _AbilityStateBlock is not null && _AbilityStateBlock.UseDebug;
        protected virtual bool CanCancelState => true;

        public AbilityStateBlockBaseSpec(AbilityStateBlock abilityStateBlock, IAbilityState abilityState)
        {
            _AbilityStateBlock = abilityStateBlock;
            _AbilityState = abilityState;

            _GameplayTagComponent = abilityState.AbilitySpec.AbilitySystem.GameplayTagComponent;
        }


        protected override void Log(object massage)
        {
#if UNITY_EDITOR
            if (UseDebug)
                Utility.Debug.Log(_AbilityStateBlock.name + " [ " + GetType().Name + " ] :" + massage, _AbilityStateBlock);
#endif
        }

        public override bool CanEnterState()
        {
            if (!base.CanEnterState())
                return false;

            if (!HasRequiredTags())
                return false;

            if (HasObstacledTag())
                return false;
            
            return true;
        }
        public override bool CanExitState()
        {
            if (!base.CanExitState())
                return false;

            if (!CanCancelState)
                return false;

            return true;
        }

        private bool HasRequiredTags()
        {
            return _AbilityStateBlock.ConditionTags.Requireds is null
                || _GameplayTagComponent.ContainAllTagsInOwned(_AbilityStateBlock.ConditionTags.Requireds);
        }
        private bool HasObstacledTag()
        {
            return _AbilityStateBlock.ConditionTags.Obstacleds is not null
                && _GameplayTagComponent.ContainAnyTagsInOwned(_AbilityStateBlock.ConditionTags.Obstacleds);
        }
        protected override void EnterState()
        {
            _GameplayTagComponent.AddOwnedTags(_AbilityStateBlock.GrantTags.Owneds);
            _GameplayTagComponent.AddBlockTags(_AbilityStateBlock.GrantTags.Blocks);
        }
        protected override void ExitState()
        {
            _GameplayTagComponent.RemoveOwnedTags(_AbilityStateBlock.GrantTags.Owneds);
            _GameplayTagComponent.RemoveBlockTags(_AbilityStateBlock.GrantTags.Blocks);
        }
    }
}
