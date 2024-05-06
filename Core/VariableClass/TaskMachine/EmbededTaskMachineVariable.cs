using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class EmbededTaskMachineVariable : TaskMachineVariable
    {
        [Header(" [ Embeded Task Machine Variable ] ")]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IMainTask _mainTask;

#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ISubTask[] _subTasks;

        private TaskMachine _taskMachine;
        public override void Setup(GameObject owner)
        {
            base.Setup(owner);

            if(_taskMachine is null)
            {
                _taskMachine = new TaskMachine(Owner, _mainTask, _subTasks);
            }
        }

        public override ITaskMachineVariable Clone()
        {
            var clone = new EmbededTaskMachineVariable();

            clone._mainTask = _mainTask.CloneTask();
            clone._subTasks = _subTasks.CloneTask();

            return clone;
        }

        public override TaskMachine GetValue()
        {
            return _taskMachine;
        }
    }
}
