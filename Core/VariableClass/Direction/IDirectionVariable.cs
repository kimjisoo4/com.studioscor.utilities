using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IDirectionVariable : IVariable<Vector3>
    {
    }
    public abstract class DirectionVariable : IDirectionVariable
    {
        private GameObject _owner;
        public GameObject Owner => _owner;

        public abstract IVariable<Vector3> Clone();

        public abstract Vector3 GetValue();

        public virtual void Setup(GameObject owner)
        {
            _owner = owner;
        }
    }
}
