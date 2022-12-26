using UnityEngine;

namespace StudioScor.Utilities
{

    public abstract class SimpleAmountModifier : BaseMonoBehaviour
    {
        public enum EAutoPlayState
        {
            Create,
            Enable,
        }

        [Header(" [ Simple Amount Modifier ] ")]
        [SerializeField] private SimpleAmount _SimpleAmount;
        [SerializeField] private EAutoPlayState _AutoPlay = EAutoPlayState.Create;
        protected SimpleAmount SimpleAmount => _SimpleAmount;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            gameObject.TryGetComponentInParentOrChildren(out _SimpleAmount);
        }
#endif

        private void Awake()
        {
            if (_AutoPlay.Equals(EAutoPlayState.Create))
            {
                _SimpleAmount.AddModifier(this);
            }
        }

        private void OnEnable()
        {
            if (_AutoPlay.Equals(EAutoPlayState.Enable))
            {
                OnAddModifier();
            }
        }
        private void OnDisable()
        {
            if (_AutoPlay.Equals(EAutoPlayState.Enable))
            {
                OnRemoveModifier();
            }
        }

        private void OnAddModifier()
        {
            if (!_SimpleAmount && gameObject.TryGetComponentInParentOrChildren(out _SimpleAmount))
            {
                Log(" Simple Amount Is Null!", true);
            }

            _SimpleAmount.AddModifier(this);
        }
        private void OnRemoveModifier()
        {
            if (!_SimpleAmount && gameObject.TryGetComponentInParentOrChildren(out _SimpleAmount))
            {
                Log(" Simple Amount Is Null!", true);
            }

            _SimpleAmount.RemoveModifier(this);

        }
        public abstract void UpdateValue();
    }

}