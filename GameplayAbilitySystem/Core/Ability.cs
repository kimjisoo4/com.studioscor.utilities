using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;
using StudioScor.GameplayTagSystem;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace StudioScor.AbilitySystem
{
    public abstract class Ability : ScriptableObject
    {
        #region Attribute
#if ODIN_INSPECTOR
        [VerticalGroup("Ability/Properties")]
        [PropertyOrder(1)]
#else
        [Header(" 어빌리티의 이름 ")]
#endif
        [SerializeField]
        #endregion
        protected string _Name;
        #region Attribute
#if ODIN_INSPECTOR
        [PropertyOrder(0)]
        [HorizontalGroup("Ability", Width = 50), HideLabel, PreviewField(50)]
#else
        [Header(" 어빌리티의 아이콘")]
#endif
        [SerializeField]
        #endregion
        protected Sprite _Icon;
        #region Attribut
#if ODIN_INSPECTOR
        [BoxGroup("Gameplay Tags")]
        [PropertyOrder(0)]
#else
        [Header(" 어빌리티의 태그 ")]
#endif
        [SerializeField]
# endregion
        private GameplayTag _Tag;
        #region Attribut
#if ODIN_INSPECTOR
        [BoxGroup("Gameplay Tags")]
        [PropertyOrder(1)]
        [ListDrawerSettings(Expanded = true)]
#else
        [Header(" 어빌리티의 속성 ")]
#endif
        [SerializeField]
# endregion
        private GameplayTag[] _AttributeTags;
        #region Attribut
#if ODIN_INSPECTOR
        [BoxGroup("Gameplay Tags")]
        [PropertyOrder(2)]
        [ListDrawerSettings(Expanded = true)]
#else
        [Header(" 어빌리티 취소")]
#endif
        [SerializeField]
# endregion
        private GameplayTag[] _CancelTags;


        public string Name => _Name;
        public Sprite Icon => _Icon;
        public GameplayTag Tag => _Tag;
        public IReadOnlyCollection<GameplayTag> AttributeTags => _AttributeTags;
        public IReadOnlyCollection<GameplayTag> CancelTags => _CancelTags;

        [Header(" 디버그 ")]
        [SerializeField] private bool _DebugMode;
        public bool UseDebug => _DebugMode;


        #region EDITOR ONLY

        [Conditional("UNITY_EDITOR")]
        protected void Log(object massage)
        {
#if UNITY_EDITOR
            if (UseDebug)
                UnityEngine.Debug.Log(name + " [ " + GetType().Name + " ] : " + massage, this);
#endif
        }
        #endregion

        public abstract IAbilitySpec CreateSpec(AbilitySystem abilitySystem, int level = 0);

        public virtual bool CanGrantAbility(AbilitySystem abilitySystem)
        {
            return true;
        }
    }
}
