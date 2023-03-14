using UnityEngine;

namespace StudioScor.Utilities
{
    public abstract class KeepData : BaseScriptableObject
    {
        public abstract void ResetData();
        public abstract void Save(GameObject gameObject);
        public abstract void Load(GameObject gameObject);
    }
}
