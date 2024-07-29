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
        [SerializeField] private GameObject _target;
        [SerializeField] private EAutoPlayState _autoPlay = EAutoPlayState.Create;
        
        private ISimpleAmount _simpleAmount;
        protected ISimpleAmount SimpleAmount => _simpleAmount;

        protected virtual void Reset()
        {
#if UNITY_EDITOR
            _target = gameObject;

            if(_target.TryGetComponentInParentOrChildren(out _simpleAmount))
            {
                var target = _simpleAmount as MonoBehaviour;

                _target = target.gameObject;
            }
#endif
        }

        private void Awake()
        {
            if (_autoPlay.Equals(EAutoPlayState.Create))
            {
                OnAddModifier();
            }
        }
        private void OnDestroy()
        {
            if (_autoPlay.Equals(EAutoPlayState.Create) && _simpleAmount is not null)
            {
                OnRemoveModifier();
            }
        }

        private void OnEnable()
        {
            if (_autoPlay.Equals(EAutoPlayState.Enable))
            {
                OnAddModifier();
            }
        }
        private void OnDisable()
        {
            if (_autoPlay.Equals(EAutoPlayState.Enable))
            {
                OnRemoveModifier();
            }
        }

        private void OnAddModifier()
        {
            if(!_target.TryGetComponentInParentOrChildren(out _simpleAmount))
                LogError(" Simple Amount Is Null!");

            _simpleAmount.AddModifier(this);
        }

        private void OnRemoveModifier()
        {
            if(_simpleAmount is not null)
                _simpleAmount.RemoveModifier(this);
        }

        public abstract void UpdateModifier();
    }

}