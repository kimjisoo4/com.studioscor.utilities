using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IConditionVariable : IVariable<bool>
    {
    }

    public abstract class ConditionVariable : IConditionVariable
    {
        public GameObject Owner { get; protected set; }
        public abstract IVariable<bool> Clone();
        public abstract bool GetValue();

        public virtual void Setup(GameObject owner)
        {
            Owner = owner;
        }
    }
}
