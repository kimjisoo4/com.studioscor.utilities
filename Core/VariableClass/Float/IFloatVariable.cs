using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IFloatVariable : IVariable
    {
        public float GetValue();
    }
    public abstract class FloatVariable : IFloatVariable
    {
        private GameObject _owner;
        public GameObject Owner => _owner;

        public abstract IVariable Clone();
        public abstract float GetValue();
        public virtual void Setup(GameObject owner)
        {
            _owner = Owner;
        }
    }
}
