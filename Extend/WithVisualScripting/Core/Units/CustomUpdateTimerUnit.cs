#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using System;

namespace StudioScor.Utilities.VisualScripting
{ 
    [UnitTitle("Task Timer")]
    [UnitCategory("StudioScor\\Task")]
    public class CustomUpdateTimerUnit : UpdateUnit
    {
        [DoNotSerialize]
        [PortLabel("Duration")]
        public ValueInput Duration { get; private set; }

        [DoNotSerialize]
        [PortLabel("Elapsed(%)")]
        public ValueOutput NormalizedTime { get; private set; }

        public new class Data : UpdateUnit.Data
        {
            public float Duration;
            public float ElapsedTime;
            public float NormalizedTime;
        }
        public override IGraphElementData CreateData()
        {
            return new Data();
        }
        protected override void Definition()
        {
            base.Definition();

            if (!IsManualNormalizedTime)
            {
                Duration = ValueInput<float>(nameof(Duration), 0.2f);

                Requirement(Duration, Enter);
            }
            
            NormalizedTime = ValueOutput<float>(nameof(NormalizedTime));

            Succession(Enter, Finished);
            
            Assignment(Enter, NormalizedTime);

            ManualDefinition();
        }
        protected override void OnEnter(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            data.Duration = IsManualNormalizedTime ? 0f : flow.GetValue<float>(Duration);
            data.ElapsedTime = 0f;
            data.NormalizedTime = 0f;
        }

        protected override void OnInterrupt(Flow flow)
        {

        }

        protected override ControlOutput OnManualUpdate(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            UpdateData(flow, data);

            return null;
        }

        protected override void OnUpdate(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            UpdateData(flow, data);
        }


        private void UpdateData(Flow flow, Data data)
        {
            if (IsManualNormalizedTime)
            {
                data.NormalizedTime = flow.GetValue<float>(ManualNormalizedTime);
                data.ElapsedTime = data.Duration * data.NormalizedTime;
            }
            else
            {
                if (IsManualDeltaTime)
                {
                    data.ElapsedTime += flow.GetValue<float>(ManualDeltaTime);
                }
                else
                {
                    data.ElapsedTime += DeltaTime;
                }

                data.NormalizedTime = data.ElapsedTime / data.Duration;
                data.NormalizedTime = MathF.Min(data.NormalizedTime, 1f);
            }

            flow.SetValue(NormalizedTime, data.NormalizedTime);

            flow.Invoke(Update);

            if (data.NormalizedTime >= 1f)
            {
                data.IsActivate = false;

                flow.Invoke(Finished);

                return;
            }

        }

        
    }
}
#endif