using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    public interface IAbilitySpec
    {
        public Ability Ability { get; }
        public AbilitySystem AbilitySystem { get; }
        public bool IsPlaying { get; }
        public int Level { get; }
        public void OnAddAbility();
        public void OnRemoveAbility();
        public void OnOverrideAbility(int level);

        public void SetAbilityLevel(int level);

        public bool TryActiveAbility();
        public bool CanActiveAbility();
        public void ForceActiveAbility();

        public void OnReleaseAbility();
        public bool TryCancelAbility(GameplayTag[] cancelTags);
        public void ForceCancelAbility();

        public void EndAbility();

        public void OnUpdateAbility(float deltaTime);
        public void OnFixedUpdateAbility(float deltaTime);

        public event OnAbilityHandler OnActivatedAbility;
        public event OnAbilityHandler OnEndedAbility;
        public event OnAbilityHandler OnFinishedAbility;
        public event OnAbilityHandler OnCanceledAbility;
        public event AbilityLevelHandler OnChangedAbilityLevel;
    }
}
