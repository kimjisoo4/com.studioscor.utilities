using UnityEngine;

namespace StudioScor.Utilities
{
    public interface ITaskAction
    {
        public GameObject Owner { get; }

        public ITaskAction Clone();
        public void Setup(GameObject owner);
        public void Action(GameObject target);
    }

    public abstract class TaskAction : ITaskAction
    {
        public GameObject Owner { get; protected set; }

        public virtual void Setup(GameObject owner)
        {
            Owner = owner;
        }

        public abstract void Action(GameObject target);
        public abstract ITaskAction Clone();
    }
}
