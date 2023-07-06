#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("Test Custom Timer Event Unit ")]
    public class CustomTimerUnit : ToggleUpdateUnit
    {
        [DoNotSerialize]
        [PortLabel("Finished")]
        public ControlOutput Finished;

        [DoNotSerialize]
        [PortLabel("Duration")]
        public ValueInput Duration;

        [DoNotSerialize]
        [PortLabel("ElapsedTime")]
        public ValueOutput ElapsedTime;

        [DoNotSerialize]
        [PortLabel("Progress")]
        public ValueOutput Progress;

        public new class Data : ToggleUpdateUnit.Data
        {
            public float Duration;
            public float ElapsedTime;
            public float Progress;
        }

        public override IGraphElementData CreateData()
        {
            return new Data();
        }


        protected override void Definition()
        {
            base.Definition();

            Finished = ControlOutput(nameof(Finished));

            Duration = ValueInput<float>(nameof(Duration), 2f);
            ElapsedTime = ValueOutput<float>(nameof(ElapsedTime));
            Progress = ValueOutput<float>(nameof(Progress));

            Succession(Enter, Finished);
        }

        protected override void OnUpdate(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);
            
            flow.Invoke(Update);

            if (data.Progress < 1f)
            {
                return;
            }

            EndActivate(flow);

            flow.Invoke(Finished);
        }

        protected override void SetValue(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            data.Duration = flow.GetValue<float>(Duration);
            data.DeltaTime = 0f;
            data.Progress = 0f;
            data.ElapsedTime = 0f;
        }

        protected override bool ShouldTrigger(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            return data.IsActivate;
        }

        protected override void UpdateValue(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            data.DeltaTime = data.IsManual ? flow.GetValue<float>(ManualDeltaTime) : GetDeltaTime;
            data.ElapsedTime += data.DeltaTime;
            data.Progress = data.ElapsedTime.SafeDivide(data.Duration);
            data.Progress = Mathf.Min(1f, data.Progress);

            flow.SetValue(Progress, data.Progress);
            flow.SetValue(ElapsedTime, data.ElapsedTime);
            flow.SetValue(DeltaTime, data.DeltaTime);
        }
    }


}
#endif