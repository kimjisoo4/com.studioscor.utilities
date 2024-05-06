using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    public interface ITaskMachineVariable : IVariable<TaskMachine>
    {
        public abstract ITaskMachineVariable Clone();
    }

    [Serializable]
    public abstract class TaskMachineVariable : ITaskMachineVariable
    {
        public GameObject Owner { get; protected set; }

        public abstract ITaskMachineVariable Clone();

        public abstract TaskMachine GetValue();
        public virtual void Setup(GameObject owner)
        {
            Owner = owner;
        }
    }
}
