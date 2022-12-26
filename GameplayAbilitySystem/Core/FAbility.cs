using UnityEngine;

namespace StudioScor.AbilitySystem
{
    [System.Serializable]
    public struct FAbility
    {
        public FAbility(Ability ability, int level)
        {
            Ability = ability;
            Level = level;
        }

        [field: SerializeField] public Ability Ability { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
    }
}
