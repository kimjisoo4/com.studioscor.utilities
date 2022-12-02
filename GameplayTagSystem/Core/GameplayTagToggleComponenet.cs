using UnityEngine;

namespace StudioScor.GameplayTagSystem
{
    public class GameplayTagToggleComponenet : MonoBehaviour
    {
        private GameplayTagComponent _GameplayTagComponent;
        [SerializeField] private bool _AutoActivate = true;

        [SerializeField] private GameplayTag[] _ToggleTags;

        private void OnEnable()
        {
            _GameplayTagComponent = GetComponentInParent<GameplayTagComponent>();

            if(_AutoActivate)
                AddGameplayTags();
        }
        private void OnDisable()
        {
            RemoveGameplayTags();

            if(_AutoActivate)
                _GameplayTagComponent = null;
        }

        public void AddGameplayTags()
        {
            if (_GameplayTagComponent is null)
                return;

            _GameplayTagComponent.AddOwnedTags(_ToggleTags);
        }
        public void RemoveGameplayTags()
        {
            if (_GameplayTagComponent is null)
                return;

            _GameplayTagComponent.RemoveOwnedTags(_ToggleTags);
        }
    }
}
