using UnityEngine;

namespace StudioScor.Utilities
{
    public abstract class BindingComponent<T> : BaseBindingComponent where T : Component
    {
        [Header(" [ Binding Component ] ")]
        [SerializeField] private T _component;

        public T Component => _component;

        protected virtual void Awake()
        {
            if(!_component)
            {
                _component = GetComponent<T>();
            }
        }

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (SUtility.IsPlayingOrWillChangePlaymode)
                return;

            if(!_component)
            {
                _component = GetComponentInParent<T>();
            }
#endif
        }
    }

}
