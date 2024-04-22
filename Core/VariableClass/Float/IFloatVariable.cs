using UnityEngine;

namespace StudioScor.Utilities
{

    public interface IFloatVariable : IVariable<float>
    {
        public abstract IFloatVariable Clone();
    }
    public abstract class FloatVariable : IFloatVariable
    {
        public GameObject Owner { get; protected set; }

        public abstract IFloatVariable Clone();
        public abstract float GetValue();
        public virtual void Setup(GameObject owner)
        {
            Owner = owner;
        }
    }
}
