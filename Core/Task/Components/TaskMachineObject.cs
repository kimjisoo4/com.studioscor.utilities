using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/new TaskMachine", fileName = "TM_")]
    public class TaskMachineObject : BaseScriptableObject
    {
        [Header(" [ Task Machine Instance ] ")]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IMainTask _mainTask;

#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ISubTask[] _subTasks;

        public TaskMachine CreateMachine(GameObject owner)
        {
            return new TaskMachine(owner, _mainTask, _subTasks);
        }
    }

}