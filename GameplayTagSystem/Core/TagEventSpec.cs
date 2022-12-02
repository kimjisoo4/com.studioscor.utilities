namespace StudioScor.GameplayTagSystem
{
    public abstract class TagEventSpec
    {
        #region
        public delegate void TagEventHandler(TagEventSpec spec);
        #endregion
        private TagEvent _TagEvent;
        public TagEvent TagEvent => _TagEvent;

        private bool _Toggle = false;

        public event TagEventHandler OnTriggerTagEvent;
        public event TagEventHandler OnReTriggerTagEvent;

        private GameplayTagComponent _GameplayTagComponent;
        public GameplayTagComponent GameplayTagComponent => _GameplayTagComponent;

        public bool UseDebug => _TagEvent.UseDebugMode;


        public TagEventSpec(TagEvent tagEvent, GameplayTagComponent gameplayTagComponent)
        {
            _TagEvent = tagEvent;
            _GameplayTagComponent = gameplayTagComponent;

            switch (TagEvent.GameplayTagEventType)
            {
                case EGameplayTagEventType.ToggleTag:
                    gameplayTagComponent.OnAddedNewOwnedTag += GameplayTagSystem_OnToggle;
                    gameplayTagComponent.OnRemovedOwnedTag += GameplayTagSystem_OnToggle;
                    break;
                case EGameplayTagEventType.AddTag:
                    gameplayTagComponent.OnAddedNewOwnedTag += GameplayTagSystem_OnTriggerTag;
                    break;
                case EGameplayTagEventType.RemoveTag:
                    gameplayTagComponent.OnRemovedOwnedTag += GameplayTagSystem_OnTriggerTag;
                    break;
                case EGameplayTagEventType.TriggerTag:
                    gameplayTagComponent.OnTriggeredTag += GameplayTagSystem_OnTriggerTag;
                    break;
                default:
                    break;
            }
        }
        private void GameplayTagSystem_OnToggle(GameplayTagComponent gameplayTagComponent, GameplayTag changedTag)
        {
            if (TagEvent.GameplayTag == changedTag)
            {
                if (!_Toggle)
                {
                    OnTriggerEnter();

                    OnTriggerTagEvent?.Invoke(this);
                }
                else
                {
                    OnTriggerExit();

                    OnReTriggerTagEvent?.Invoke(this);
                }
            } 
        }

        private void GameplayTagSystem_OnTriggerTag(GameplayTagComponent gameplayTagComponent, GameplayTag changedTag)
        {
            if (TagEvent.GameplayTag == changedTag)
            {
                OnTriggerEnter();

                OnTriggerTagEvent?.Invoke(this);
            }
        }

        public abstract void OnTriggerEnter();
        public abstract void OnTriggerExit();
        public abstract void OnTriggerStay(float deltaTime);

        public virtual void OnDrawGizmo()
        {

        }
    }
}
