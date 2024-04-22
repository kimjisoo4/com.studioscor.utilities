using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IVariable<T>
    {
        public GameObject Owner { get; }
        public void Setup(GameObject owner);
        public T GetValue();
    }

}
