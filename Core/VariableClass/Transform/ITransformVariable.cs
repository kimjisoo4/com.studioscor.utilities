using System;
using UnityEngine;

namespace StudioScor.Utilities
{

   

    public interface ITransformVariable : IVariable<Transform>
    {
        public abstract ITransformVariable Clone();
    }

    public abstract class TransformVariable : ITransformVariable
    {
        public GameObject Owner { get; protected set; }
        public abstract ITransformVariable Clone();
        public abstract Transform GetValue();
        public virtual void Setup(GameObject owner)
        {
            Owner = owner;
        }
    }
}
