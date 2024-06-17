using System;
using UnityEngine;

namespace StudioScor.Utilities
{

    [Serializable]
    public class TimerMainTask : Task, IMainTask
    {
        [Header(" [ Timer Ability Task ] ")]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IFloatVariable _duration = new DefaultFloatVariable(2f);
        private float Duration => _original is not null ? _original._duration.GetValue() : _duration.GetValue();

        private readonly Timer _timer = new();

        public bool IsFixedUpdate => false;
        public float NormalizedTime => _timer.NormalizedTime;

        private TimerMainTask _original;

        public override ITask Clone()
        {
            var copy = new TimerMainTask();

            copy._original = this;

            return copy;
        }

        public void UpdateTask(float deltaTime)
        {
            if (!IsPlaying)
                return;

            Update(deltaTime);
        }
        public void FixedUpdateTask(float deltaTime)
        {
            return;
        }

        protected override void EnterTask()
        {
            base.EnterTask();

            _timer.OnTimer(Duration);
        }
        protected virtual void Update(float deltaTime)
        {
            _timer.UpdateTimer(deltaTime);

            if (_timer.IsFinished)
            {
                ComplateTask();
            }
        }
        protected override void ExitTask()
        {
            base.EnterTask();

            _timer.EndTimer();
        }
    }

}
