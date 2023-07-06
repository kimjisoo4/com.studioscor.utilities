#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections;

namespace StudioScor.Utilities.VisualScripting
{
    [TypeIcon(typeof(Unity.VisualScripting.Timer))]
    public abstract class UpdateUnit : Unit, IGraphEventListener, IGraphElementWithData
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput Enter { get; private set; }

        [DoNotSerialize]
        [PortLabel("Interrupt")]
        public ControlInput Interrupt { get; private set; }

        [DoNotSerialize]
        [PortLabel("Manual Update")]
        public ControlInput ManualUpdate { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput Exit { get; private set; }

        [DoNotSerialize]
        [PortLabel("Finished")]
        public ControlOutput Finished { get; private set; }

        [DoNotSerialize]
        [PortLabel("OnCanceled")]
        public ControlOutput Canceled { get; private set; }


        [DoNotSerialize]
        [PortLabel("Update")]
        public ControlOutput Update { get; private set; }

        [DoNotSerialize]
        [PortLabel("deltaTime")]
        public ValueInput ManualDeltaTime { get; protected set; }

        [DoNotSerialize]
        [PortLabel("NormalizedTime")]
        public ValueInput ManualNormalizedTime { get; protected set; }


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

        protected string HookName
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
            public bool IsActivate;
            public bool IsListening;
            public Delegate Update;
        }

        public abstract IGraphElementData CreateData();
        public bool IsListening(GraphPointer pointer)
        {
            if (!pointer.hasData)
                return false;

            return pointer.GetElementData<Data>(this).IsListening;
        }

        public virtual void StartListening(GraphStack stack)
        {
            var data = stack.GetElementData<Data>(this);

            if (data.IsListening)
                return;

            if(!IsManualUpdate)
            {
                var reference = stack.ToReference();
                var hook = new EventHook(HookName, stack.machine);

                Action<EmptyEventArgs> update = args => TriggerUpdate(reference);
                EventBus.Register(hook, update);

                data.Update = update;
            }
            else
            {
                data.Update = null;
            }
            
            data.IsListening = true;
        }

        public virtual void StopListening(GraphStack stack)
        {
            var data = stack.GetElementData<Data>(this);

            if (!data.IsListening)
                return;

            if (!IsManualUpdate)
            {
                var hook = new EventHook(HookName, stack.machine);

                EventBus.Unregister(hook, data.Update);
            }

            if(data.IsActivate)
            {
                var flow = Flow.New(stack.ToReference());

                OnInterrupt(flow);

                flow.Invoke(Canceled);
                flow.Dispose();
            }
                
            data.Update = null;
            data.IsActivate = false;
            data.IsListening = false;
        }

        private void TriggerUpdate(GraphReference reference)
        {
            using (var flow = Flow.New(reference))
            {
                Tick(flow);
            }
        }

        protected void ManualDefinition()
        {
            if (IsManualUpdate)
            {
                if (IsManualDeltaTime)
                {
                    ManualDeltaTime = ValueInput<float>(nameof(ManualDeltaTime));

                    Requirement(ManualDeltaTime, Enter);
                }
                else
                {
                    ManualNormalizedTime = ValueInput<float>(nameof(ManualNormalizedTime));

                    Requirement(ManualNormalizedTime, Enter);
                }

                Succession(ManualUpdate, Update);
            }
        }
        protected override void Definition()
        {
            Enter = ControlInput(nameof(Enter), Start);
            Interrupt = ControlInput(nameof(Interrupt), End);

            Exit = ControlOutput(nameof(Exit));
            Finished = ControlOutput(nameof(Finished));
            Canceled = ControlOutput(nameof(Canceled));
            Update = ControlOutput(nameof(Update));

            Succession(Enter, Exit);
            Succession(Enter, Finished);
            Succession(Interrupt, Canceled);

            if (IsManualUpdate)
            {
                ManualUpdate = ControlInput(nameof(ManualUpdate), ManualTick);
                
                Succession(ManualUpdate, Update);
            }
            else
            {
                Succession(Enter, Update);
            }
        }

        private ControlOutput Start(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            data.IsActivate = true;

            OnEnter(flow);

            return Exit;
        }

        private ControlOutput End(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            if(data.IsActivate)
            {
                OnInterrupt(flow);
            }

            data.IsActivate = false;

            return Canceled;
        }

        protected void Tick(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            if (!data.IsActivate)
                return;

            var stack = flow.PreserveStack();

            OnUpdate(flow);

            if(!data.IsActivate)
                flow.RestoreStack(stack);

            flow.DisposePreservedStack(stack);
        }

        private ControlOutput ManualTick(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            if (!data.IsActivate)
                return null;

            return OnManualUpdate(flow);
        }

        protected abstract void OnUpdate(Flow flow);
        protected abstract ControlOutput OnManualUpdate(Flow flow);
        protected abstract void OnEnter(Flow flow);
        protected abstract void OnInterrupt(Flow flow);
    }
}
#endif