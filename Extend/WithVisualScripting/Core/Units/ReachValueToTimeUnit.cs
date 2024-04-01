#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using System;
using UnityEngine;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("Task ReachValueToTime")]
    [UnitCategory("StudioScor\\Task")]
    public class ReachValueToTimeUnit : UpdateUnit
    {
        [DoNotSerialize]
        [PortLabel("Duration")]
        public ValueInput Duration { get; private set; }

        [DoNotSerialize]
        [PortLabel("Distance")]
        public ValueInput Distance { get; private set; }

        [DoNotSerialize]
        [PortLabel("Curve")]
        public ValueInput Curve { get; private set; }

        [DoNotSerialize]
        [PortLabel("Speed")]
        public ValueOutput Speed { get; private set; } 

        [DoNotSerialize]
        [PortLabel("Elapsed(%)")]
        public ValueOutput NormalizedTime { get; private set; }

        public new class Data : UpdateUnit.Data
        {
            public float Duration;
            public float Distance;
            public AnimationCurve Curve;
            public float Speed;
            public float ElapsedTime;
            public float NormalizedTime;
            public float PrevDistance;
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

            Distance = ValueInput<float>(nameof(Distance), 5f);
            Curve = ValueInput<AnimationCurve>(nameof(Curve), AnimationCurve.Linear(0, 0, 1, 1));
            NormalizedTime = ValueOutput<float>(nameof(NormalizedTime));
            Speed = ValueOutput<float>(nameof(Speed));

            Succession(Enter, Finished);

            Assignment(Enter, NormalizedTime);

            ManualDefinition();
        }


        protected override void OnEnter(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            data.Duration = IsManualNormalizedTime ? 0f : flow.GetValue<float>(Duration);
            data.Distance = flow.GetValue<float>(Distance);
            data.Curve = flow.GetValue<AnimationCurve>(Curve);
            data.Speed = 0f;
            data.ElapsedTime = 0f;
            data.NormalizedTime = 0f;
            data.PrevDistance = 0f;
        }

        protected override void OnInterrupt(Flow flow)
        {
        }
        protected override void OnUpdate(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            UpdateData(flow, data);
        }

        protected override ControlOutput OnManualUpdate(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            UpdateData(flow, data);

            return null;
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


            float curveValue = data.Curve.Evaluate(data.NormalizedTime);
            float distance = data.Distance * curveValue;
            float speed = distance - data.PrevDistance;
            data.PrevDistance = distance;

            flow.SetValue(Speed, speed);

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