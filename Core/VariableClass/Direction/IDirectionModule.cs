using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IDirectionModule : IVariable
    {
        public Vector3 GetDirection();
    }
    public abstract class DirectionVariable : IDirectionModule
    {
        private GameObject _owner;
        public GameObject Owner => _owner;

        public abstract IVariable Clone();

        public abstract Vector3 GetDirection();

        public virtual void Setup(GameObject owner)
        {
            _owner = owner;
        }
    }
}
