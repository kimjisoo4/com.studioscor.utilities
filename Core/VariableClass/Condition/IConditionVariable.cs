using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IConditionVariable : IVariable<bool>
    {
    }

    public abstract class ConditionVariable : IConditionVariable
    {
        private GameObject _owner;
        public GameObject Owner => _owner;

        public abstract IVariable<bool> Clone();
        public abstract bool GetValue();

        public virtual void Setup(GameObject owner)
        {
            _owner = owner;
        }
    }
}
