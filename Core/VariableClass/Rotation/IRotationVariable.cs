using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IRotationVariable : IVariable<Quaternion>
    {
        public abstract IRotationVariable Clone();
    }
    public abstract class RotationVariable : IRotationVariable
    {
        public GameObject Owner { get; protected set; }
        public abstract IRotationVariable Clone();
        public abstract Quaternion GetValue();

        public virtual void Setup(GameObject owner)
        {
            Owner = owner;
        }
    }

}
