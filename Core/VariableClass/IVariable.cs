using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IVariable
    {
        public GameObject Owner { get; }
        public void Setup(GameObject owner);

        public IVariable Clone();
    }
}
