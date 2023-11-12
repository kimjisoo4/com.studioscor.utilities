using UnityEngine;

namespace StudioScor.Utilities
{

    public abstract class SimpleAmountModifier : BaseMonoBehaviour, ISimpleAmountModifier
    {
        public enum EAutoPlayState
        {
            None,
            Create,
            Enable,
        }

        [Header(" [ Simple Amount Modifier ] ")]
        [SerializeField] private GameObject _Target;
        [SerializeField] private EAutoPlayState _AutoPlay = EAutoPlayState.Create;
        
        private ISimpleAmount _SimpleAmount;
        protected ISimpleAmount SimpleAmount => _SimpleAmount;

        protected virtual void Reset()
        {
#if UNITY_EDITOR
            _Target = gameObject;

            if(_Target.TryGetComponentInParentOrChildren(out _SimpleAmount))
            {
                var target = _SimpleAmount as MonoBehaviour;

                _Target = target.gameObject;
            }
#endif
        }

        private void Awake()
        {
            if (_AutoPlay.Equals(EAutoPlayState.Create))
            {
                OnAddModifier();
            }
        }
        private void OnDestroy()
        {
            if (_AutoPlay.Equals(EAutoPlayState.Create) && _SimpleAmount is not null)
            {
                OnRemoveModifier();
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
            if(!_Target.TryGetComponentInParentOrChildren(out _SimpleAmount))
                LogError(" Simple Amount Is Null!");

            _SimpleAmount.AddModifier(this);
        }

        private void OnRemoveModifier()
        {
            if (!_Target.TryGetComponentInParentOrChildren(out _SimpleAmount))
                LogError(" Simple Amount Is Null!");

            _SimpleAmount.RemoveModifier(this);

        }
        public abstract void UpdateModifier();
    }

}