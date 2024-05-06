using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    

    public interface IPositionVariable : IVariable<Vector3>
    {
        public abstract IPositionVariable Clone();
    }
    public abstract class PositionVariable : IPositionVariable
    {
        public GameObject Owner { get; protected set; }
        public abstract IPositionVariable Clone();
        public abstract Vector3 GetValue();
        public virtual void Setup(GameObject owner)
        {
            Owner = owner;
        }
    }

}
