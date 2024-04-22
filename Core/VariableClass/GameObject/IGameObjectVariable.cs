using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IGameObjectVariable : IVariable<GameObject>
    {
        public abstract IGameObjectVariable Clone();
    }

    public abstract class GameObjectVariable : IGameObjectVariable
    {
        public GameObject Owner { get; protected set; }
        public abstract IGameObjectVariable Clone();
        public abstract GameObject GetValue();
        public virtual void Setup(GameObject owner)
        {
            Owner = owner;
        }
    }

}
