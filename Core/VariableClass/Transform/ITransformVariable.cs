using UnityEngine;

namespace StudioScor.Utilities
{
    public interface ITransformVariable : IVariable<Transform>
    {
    }

    public abstract class TransformVariable : ITransformVariable
    {
        private GameObject _owner;
        public GameObject Owner => _owner;
        public abstract IVariable<Transform> Clone();
        public abstract Transform GetValue();
        public virtual void Setup(GameObject owner)
        {
            _owner = owner;
        }
    }
}
