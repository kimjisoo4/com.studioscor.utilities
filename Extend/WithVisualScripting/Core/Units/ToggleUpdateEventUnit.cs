#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace StudioScor.Utilities.VisualScripting
{
    public class ToggleUpdateEventUnit<TArgs> : Unit, IGraphEventListener, IGraphElementWithData
    {
        [DoNotSerialize]
        [PortLabel("Enter")]
        [PortLabelHidden]
        public ControlInput Enter;

        [DoNotSerialize]
        [PortLabel("Interrupt")]
        public ControlInput Interrupt;

        [DoNotSerialize]
        [PortLabel("Manual")]
        public ControlInput Manual;

        [DoNotSerialize]
        [PortLabel("Exit")]
        [PortLabelHidden]
        public ControlOutput Exit;

        [DoNotSerialize]
        [PortLabel("Cancel")]
        public ControlOutput Cancel;


        [DoNotSerialize]
        [PortLabel("Update")]
        public ControlOutput Update;

        [DoNotSerialize]
        [PortLabel("DeltaTime")]
        public ValueInput ManualDeltaTime;


        [DoNotSerialize]
        [PortLabel("NormalizedTime")]
        public ValueInput ManualNormalizedTime;

        [DoNotSerialize]
        [PortLabel("Time")]
        [PortLabelHidden]
        public ValueOutput OutputTime;


        [Serialize]
        [Inspectable]
        [InspectorToggleLeft]
        public EUpdateType UpdateType { get; set; } = EUpdateType.Update;

        protected bool IsManualUpdate => IsManualDeltaTime || IsManualNormalizedTime;
        protected bool IsManualDeltaTime => UpdateType.Equals(EUpdateType.ManualDeltaTime);
        protected bool IsManualNormalizedTime => UpdateType.Equals(EUpdateType.ManualNormalizedTime);

        protected float DeltaTime
        {
            get
            {
                switch (UpdateType)
                {
                    case EUpdateType.Update:
                        return Time.deltaTime;
                    case EUpdateType.Fixed:
                        return Time.fixedDeltaTime;
                    case EUpdateType.Late:
                        return Time.deltaTime;
                    default:
                        return default;
                }
            }
        }
        public virtual string HookName
        {
            get
            {
                switch (UpdateType)
                {
                    case EUpdateType.Update:
                        return EventHooks.Update;
                    case EUpdateType.Fixed:
                        return EventHooks.FixedUpdate;
                    case EUpdateType.Late:
                        return EventHooks.LateUpdate;
                    default:
                        return default;
                }
            }
        }
        public class Data : IGraphElementData
        {
            public bool IsListening;
            public bool IsActivate;
            public EventHook Hook;
            public Delegate Handler;

        }
        public IGraphElementData CreateData()
        {
            return new Data();
        }

        protected virtual EventHook GetHook(GraphReference reference)
        {
            if (!reference.hasData)
            {
                return HookName;
            }

            return new EventHook(HookName, reference.machine);
        }
        protected override void Definition()
        {
            Enter = ControlInput(nameof(Enter), OnEnter);
            Interrupt = ControlInput(nameof(Interrupt), OnInterrupt);

            Exit = ControlOutput(nameof(Exit));
            Cancel = ControlOutput(nameof(Cancel));
            Update = ControlOutput(nameof(Update));

            OutputTime = ValueOutput<float>(nameof(OutputTime));

            Succession(Enter, Exit);
            Succession(Enter, Cancel);
            Succession(Interrupt, Cancel);
            
            if (IsManualUpdate)
            {
                Manual = ControlInput(nameof(Manual), ManualUpdate);

                if(IsManualNormalizedTime)
                {
                    ManualNormalizedTime = ValueInput<float>(nameof(ManualNormalizedTime), 0f);
                    
                    Requirement(ManualNormalizedTime, OutputTime);
                }
                else
                {
                    ManualDeltaTime = ValueInput<float>(nameof(ManualDeltaTime), 0f);

                    Requirement(ManualDeltaTime, OutputTime);
                }

                Assignment(Manual, OutputTime);
                Succession(Manual, Update);
            }
            else
            {
                Assignment(Enter, OutputTime);
                Succession(Enter, Update);
            }
        }

        
        private ControlOutput OnEnter(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            SetData(flow);

            if (!IsManualUpdate)
            {
                var reference = flow.stack.ToReference();
                var hook = GetHook(reference);
                
                Action<TArgs> handler = args => Trigger(reference, args);
                EventBus.Register(hook, handler);

                data.Hook = hook;
                data.Handler = handler;
            }
            
            data.IsActivate = true;

            return Exit;
        }
        private ControlOutput OnInterrupt(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            if (!data.IsActivate)
                return null;

            if(!IsManualUpdate)
            {
                EventBus.Unregister(data.Hook, data.Handler);
             
                data.Handler = null;
            }

            data.IsActivate = false;
            
            return Cancel;
        }


        private void Trigger(GraphReference reference, TArgs args)
        {
            var flow = Flow.New(reference);

            if (!ShouldTrigger(flow, args))
            {
                flow.Dispose();
                return;
            }

            AssignArguments(flow, args);

            flow.Run(Update);
        }

        private ControlOutput ManualUpdate(Flow flow)
        {
            if (!ShouldTrigger(flow, default))
            {
                return null;
            }

            AssignArguments(flow, default);

            return Update;
        }

        public void StartListening(GraphStack stack)
        {
            var data = stack.GetElementData<Data>(this);

            if (data.IsListening)
                return;

            data.IsListening = true;
        }

        public void StopListening(GraphStack stack)
        {
            var data = stack.GetElementData<Data>(this);

            if (!data.IsListening)
                return;

            if(data.IsActivate)
            {
                data.IsActivate = false;

                var flow = Flow.New(stack.ToReference());

                flow.Run(Cancel);

                EventBus.Unregister(data.Hook, data.Handler);
            }

            data.IsListening = false;
            data.Handler = null;
        }

        public bool IsListening(GraphPointer pointer)
        {
            return pointer.GetElementData<Data>(this).IsListening;
        }
        protected virtual void SetData(Flow flow)
        {

        }

        protected virtual bool ShouldTrigger(Flow flow, TArgs args)
        {
            var data = flow.stack.GetElementData<Data>(this);

            return data.IsActivate;
        }

        protected virtual void AssignArguments(Flow flow, TArgs args)
        {
            float value;

            if (IsManualUpdate)
            {
                if (IsManualNormalizedTime)
                {
                    value = flow.GetValue<float>(ManualNormalizedTime);
                }
                else
                {
                    value = flow.GetValue<float>(ManualDeltaTime);
                }
            }
            else
            {
                value = DeltaTime;
            }

            flow.SetValue(OutputTime, value);

        }

    }


}
#endif