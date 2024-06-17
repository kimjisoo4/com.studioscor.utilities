using UnityEngine;


namespace StudioScor.Utilities
{
    public interface ITaskInstanceActor
    {
        public void SetOwner(GameObject owner);
        public void Activate();
        public void Deactivate();

        public GameObject Owner { get; }
    }

    public class TaskMachineInstaceActor : BaseMonoBehaviour, ITaskInstanceActor
    {
        [Header(" [ Task Machine Instance ] ")]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITaskMachineVariable _taskMachine = new ScriptableTaskMachineVariable();

        private TaskMachine _taskMachineInstance;

        private bool _isPlaying;
        private GameObject _owner;
        public bool IsPlaying => _isPlaying;
        public GameObject Owner => _owner;


        public void SetOwner(GameObject owner)
        {
            _owner = owner;

            if (_taskMachineInstance is null)
            {
                _taskMachine.Setup(gameObject);

                _taskMachineInstance = _taskMachine.GetValue();
            }
        }

        public void Activate()
        {

            if (IsPlaying)
                return;

            _isPlaying = true;

            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            _taskMachineInstance.OnTask();
        }

        public void Deactivate()
        {
            if (!IsPlaying)
                return;

            _isPlaying = false;
            _taskMachineInstance.ComplateTask();

            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!IsPlaying)
                return;

            float deltaTime = Time.deltaTime;

            _taskMachineInstance.UpdateTask(deltaTime);

            if (!_taskMachineInstance.IsPlaying)
                Deactivate();
        }

        private void FixedUpdate()
        {
            if (!IsPlaying)
                return;

            float deltaTime = Time.fixedDeltaTime;

            _taskMachineInstance.FixedUpdateTask(deltaTime);

            if (!_taskMachineInstance.IsPlaying)
                Deactivate();
        }
    }

}