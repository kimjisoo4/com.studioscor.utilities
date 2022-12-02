using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

using StudioScor.GameplayTagSystem;
namespace StudioScor.EffectSystem
{
    [System.Serializable]   
    public struct FGameplayEffectTags
    {
        #region 이펙트 태그
#if ODIN_INSPECTOR
        [BoxGroup("Attribute Tags")]
        [ListDrawerSettings(Expanded = true)]
#else
        [Header("이펙트 태그")]
#endif
        #endregion
        public GameplayTag AssetTag;

        #region 속성
#if ODIN_INSPECTOR
        [BoxGroup("Attribute Tags")]
        [ListDrawerSettings(Expanded = true)]
#else
        [Header("이펙트 속성")]
#endif
        #endregion
        public GameplayTag[] AttributeTags;

        #region 활성화시 부여 태그
#if ODIN_INSPECTOR
        [BoxGroup("Activate Tags")]
        [ListDrawerSettings(Expanded = true)]
#else
        [Header("활성화 부여 태그")]
#endif
        #endregion
        public GameplayTag[] ActivateGrantedTags;

        #region 활성화 필수 태그
#if ODIN_INSPECTOR
        [BoxGroup("Activate Tags")]
        [Title("Activate Condition")]
        [ListDrawerSettings(Expanded = true)]
#else
        [Header("활성화 필수 태그")]
#endif
        #endregion
        public GameplayTag[] ActivateRequiredTags;
        #region 활성화 방해 태그
#if ODIN_INSPECTOR
        [BoxGroup("Activate Tags")]
        [ListDrawerSettings(Expanded = true)]
#endif
        #endregion
        public GameplayTag[] ActivateObstacledeTags;

        #region 적용시 부여 태그
#if ODIN_INSPECTOR
        [BoxGroup("Apply Tags")]
        [ListDrawerSettings(Expanded = true)]
#else
        [Header("젹용 부여 태그")]
#endif
        #endregion
        public GameplayTag[] ApplyGrantedTags;
        #region 적용 필수 태그
#if ODIN_INSPECTOR
        [BoxGroup("Apply Tags")]
        [Title("Apply Condition")]
        [ListDrawerSettings(Expanded = true)]
#else
        [Header("적용 필수 태그")]
#endif        
        #endregion
        public GameplayTag[] ApplyRequiredTags;
        #region 적용 방해 태그
#if ODIN_INSPECTOR
        [BoxGroup("Apply Tags")]
        [ListDrawerSettings(Expanded = true)]
#endif
        #endregion
        public GameplayTag[] ApplyObstacledTags;

        #region 제거할 이펙트 태그
#if ODIN_INSPECTOR
        [BoxGroup("Remove Effect Tags")]
        [ListDrawerSettings(Expanded = true)]
#else
        [Header("태그로 이펙트를 제거")]
#endif
        #endregion
        public GameplayTag[] RemoveGameplayEffectsAsTags;
    }
}
