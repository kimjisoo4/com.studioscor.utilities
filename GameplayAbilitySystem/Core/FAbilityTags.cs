using UnityEngine;

using StudioScor.GameplayTagSystem;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace StudioScor.AbilitySystem
{
    [System.Serializable]
    public struct FAbilityTags
    {
        [Header("조건 태그")]
        public FConditionTags ConditionTags;

        [Header(" 부여 태그 ")]
        public FGameplayTags GrantTags;

        [Header(" 어빌리티 취소 태그 ")]
        public GameplayTag[] CancelAbilityTags;
    }
}
