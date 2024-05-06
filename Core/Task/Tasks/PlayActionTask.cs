using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class PlayActionTask : Task, ISubTask
    {
        [Header(" [ Play Action Task ] ")]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IGameObjectVariable _target = new SelfGameObjectVariable();
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITaskAction[] _taskActions;

        [SerializeField] private float _startTime = 0f;

        private bool _wasPlay = false;
        private PlayActionTask _original;

        protected override void SetupTask()
        {
            base.SetupTask();

            _target.Setup(Owner);
            _taskActions.Setup(Owner);
        }
        public override ITask Clone()
        {
            var clone = new PlayActionTask();

            clone._original = this;
            clone._target = _target.Clone();
            clone._taskActions = _taskActions.CloneTask();

            return clone;
        }

        protected override void EnterTask()
        {
            base.EnterTask();

            _wasPlay = false;
        }
        public void FixedUpdateSubTask(float deltaTime, float normalizedTime)
        {
            return;
        }

        public void UpdateSubTask(float deltaTime, float normalizedTime)
        {
            if (_wasPlay)
                return;

            if (normalizedTime > _startTime)
            {
                _wasPlay = true;

                var target = _target.GetValue();

                _taskActions.Action(target);
            }
        }
    }

}
