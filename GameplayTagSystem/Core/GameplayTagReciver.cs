using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.GameplayTagSystem
{

    [System.Serializable]
    public class GameplayTagEvent
    {
        public enum ETagEventType
        {
            AddOwned,
            RemoveOwned,
            AddBlock,
            RemoveBlock,
            Trigger,
        }
#if UNITY_EDITOR
        public string Name = "Tag Event";
#endif

        [SerializeField] private ETagEventType _TagEventType;
        [SerializeField] private GameplayTag[] _GameplayTags;
        [SerializeField] private UnityEvent _TagEvent;

        public void SetGameplayEvent(GameplayTagComponent gameplayTagComponent)
        {
            switch (_TagEventType)
            {
                case ETagEventType.AddOwned:
                    gameplayTagComponent.OnAddedNewOwnedTag += GameplayTagSystem_OnTagEvent;
                    break;
                case ETagEventType.RemoveOwned:
                    gameplayTagComponent.OnRemovedOwnedTag += GameplayTagSystem_OnTagEvent;
                    break;
                case ETagEventType.AddBlock:
                    gameplayTagComponent.OnAddedNewBlockTag += GameplayTagSystem_OnTagEvent;
                    break;
                case ETagEventType.RemoveBlock:
                    gameplayTagComponent.OnRemovedBlockTag += GameplayTagSystem_OnTagEvent;
                    break;
                case ETagEventType.Trigger:
                    gameplayTagComponent.OnTriggeredTag += GameplayTagSystem_OnTagEvent;
                    break;
                default:
                    break;
            }
        }

        private void GameplayTagSystem_OnTagEvent(GameplayTagComponent gameplayTagComponent, GameplayTag changedTag)
        {
            foreach (GameplayTag tag in _GameplayTags)
            {
                if (tag == changedTag)
                {
                    _TagEvent?.Invoke();
                }
            }
        }
    }

    public class GameplayTagReciver : MonoBehaviour
    {
        [SerializeField] private GameplayTagComponent _GameplayTagComponent;
        public GameplayTagComponent GameplayTagSystem
        {
            get
            {
                if (_GameplayTagComponent == null)
                {
                    _GameplayTagComponent = GetComponentInParent<GameplayTagComponent>();
                }
                return _GameplayTagComponent;
            }
        }
        [SerializeField] private GameplayTagEvent[] _GameplayEvents;

#if UNITY_EDITOR
        private void Reset()
        {
            _GameplayTagComponent = GetComponentInParent<GameplayTagComponent>();
        }
#endif

        private void Awake()
        {
            foreach (GameplayTagEvent gameplayTagEvent in _GameplayEvents)
            {
                gameplayTagEvent.SetGameplayEvent(GameplayTagSystem);
            }
        }
    }
}
