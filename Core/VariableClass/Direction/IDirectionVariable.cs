using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IDirectionVariable : IVariable<Vector3>
    {
        public abstract IDirectionVariable Clone();
    }
    public abstract class DirectionVariable : IDirectionVariable
    {
        public GameObject Owner { get; protected set; }
        public abstract IDirectionVariable Clone();

        public abstract Vector3 GetValue();

        public virtual void Setup(GameObject owner)
        {
            Owner = owner;
        }
    }
}
