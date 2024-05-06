using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class ScriptableTaskMachineVariable : TaskMachineVariable
    {
        [Header(" [ Scriptable Task Machine Variable ] ")]
        [SerializeField] private TaskMachineObject _taskMachineObject;

        private TaskMachine _taskMachine;

        private ScriptableTaskMachineVariable _original;

        public override void Setup(GameObject owner)
        {
            base.Setup(owner);

            if (_taskMachine is null)
            {
                var taskMachine = _original is null ? _taskMachineObject : _original._taskMachineObject;

                _taskMachine = taskMachine.CreateMachine(owner);
            }
        }
        public override ITaskMachineVariable Clone()
        {
            var clone = new ScriptableTaskMachineVariable();

            clone._original = this;

            return clone;
        }

        public override TaskMachine GetValue()
        {
            return _taskMachine;
        }
    }
}
