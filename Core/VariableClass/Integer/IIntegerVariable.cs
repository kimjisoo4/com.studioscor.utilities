using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IIntegerVariable : IVariable<int>
    {
        public abstract IIntegerVariable Clone();
    }

    public abstract class IntegerVariable : IIntegerVariable
    {
        public GameObject Owner { get; protected set; }

        public abstract IIntegerVariable Clone();
        public abstract int GetValue();
        public virtual void Setup(GameObject owner)
        {
            Owner = owner;
        }
    }
}
