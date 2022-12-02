using UnityEngine;

namespace StudioScor.GameplayTagSystem
{
    public abstract class TagEvent : ScriptableObject
    {
        [SerializeField] protected EGameplayTagEventType _GameplayTagEventType;
        [SerializeField] private GameplayTag _GameplayTag;
        [SerializeField] private bool _UseDebugMode = false;

        public EGameplayTagEventType GameplayTagEventType => _GameplayTagEventType;
        public GameplayTag GameplayTag => _GameplayTag;
        public bool UseDebugMode => _UseDebugMode;

        public abstract TagEventSpec CreateSpec(GameplayTagComponent gameplayTagComponent);
    }
}
