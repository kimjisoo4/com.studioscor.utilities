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
                
            data.Update = null;
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

            if(IsManualUpdate)
            {
                ManualUpdate = ControlInput(nameof(ManualUpdate), ManualTick);
            }

            Exit = ControlOutput(nameof(Exit));
            Canceled = ControlOutput(nameof(Canceled));
            Update = ControlOutput(nameof(Update));

            Succession(Enter, Exit);
            Succession(Interrupt, Canceled);
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

            data.IsActivate = false;

            OnInterrupt(flow);

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

    [TypeIcon(typeof(Unity.VisualScripting.Timer))]
    public abstract class ToggleUpdateUnit : Unit, IGraphEventListener, IGraphElementWithData
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
        public ControlInput ManualUpdate;

        [DoNotSerialize]
        [PortLabel("Exit")]
        [PortLabelHidden]
        public ControlOutput Exit;

        [DoNotSerialize]
        [PortLabel("Canceled")]
        public ControlOutput Canceled;

        [DoNotSerialize]
        [PortLabel("Update")]
        public ControlOutput Update;

        [DoNotSerialize]
        [PortLabel("Manual DeltaTime")]
        public ValueInput ManualDeltaTime;

        [DoNotSerialize]
        [PortLabel("DeltaTime")]
        public ValueOutput DeltaTime;

        [Serialize]
        [Inspectable]
        [InspectorToggleLeft]
        public EUpdateState UpdateType { get; set; } = EUpdateState.Update;

        protected bool UseManual => UpdateType.Equals(EUpdateState.Manual);
        protected float GetDeltaTime
        {
            get
            {
                switch (UpdateType)
                {
                    case EUpdateState.Update:
                        return Time.deltaTime;
                    case EUpdateState.Fixed:
                        return Time.fixedDeltaTime;
                    case EUpdateState.Late:
                        return Time.deltaTime;
                    default:
                        return default;
                }
            }
        }
        public virtual string UpdateHookName
        {
            get
            {
                switch (UpdateType)
                {
                    case EUpdateState.Update:
                        return EventHooks.Update;
                    case EUpdateState.Fixed:
                        return EventHooks.FixedUpdate;
                    case EUpdateState.Late:
                        return EventHooks.LateUpdate;
                    default:
                        return default;
                }
            }
        }

        public class Data : IGraphElementData
        {
            public bool IsListening;
            public EventHook Hook;
            public Delegate Handler;

            public bool IsActivate;
            public bool IsManual;
            public float DeltaTime;

            public virtual bool IsPlaying => IsActivate;
        }

        public abstract IGraphElementData CreateData();

        public bool IsListening(GraphPointer pointer)
        {
            return pointer.GetElementData<Data>(this).IsListening;
        }
        protected override void Definition()
        {
            Enter = ControlInput(nameof(Enter), OnEnter);
            Interrupt = ControlInput(nameof(Interrupt), OnInterrupt);

            Exit = ControlOutput(nameof(Exit));
            Canceled = ControlOutput(nameof(Canceled));
            Update = ControlOutput(nameof(Update));

            DeltaTime = ValueOutput<float>(nameof(DeltaTime));

            Succession(Enter, Exit);
            Succession(Enter, Canceled);
            Succession(Enter, Update);
            Succession(Interrupt, Canceled);

            if(UseManual)
            {
                ManualUpdate = ControlInput(nameof(ManualUpdate), OnManualUpdate);
                ManualDeltaTime = ValueInput<float>(nameof(ManualDeltaTime), 0f);
                Succession(ManualUpdate, Update);
            }
        }

        public void StartListening(GraphStack stack)
        {
            var data = stack.GetElementData<Data>(this);

            if (data.IsListening)
                return;

            data.IsListening = true;
            data.IsActivate = false;
        }

        public void StopListening(GraphStack stack)
        {
            var data = stack.GetElementData<Data>(this);

            if (!data.IsListening)
                return;

            data.IsListening = false;

            if (data.IsActivate)
            {
                var flow = Flow.New(stack.ToReference());

                EndActivate(flow);

                flow.Invoke(Canceled);
                flow.Dispose();
            }
        }

        protected ControlOutput OnEnter(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            if (!data.IsActivate)
                OnActivate(flow);

            return Exit;
        }
        private ControlOutput OnInterrupt(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            if (!data.IsActivate)
                return null;

            EndActivate(flow);

            return Canceled;
        }

        protected virtual void OnActivate(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            data.IsActivate = true;

            data.IsManual = UseManual;

            if (!data.IsManual)
            {
                var reference = flow.stack.ToReference();
                var hook = new EventHook(UpdateHookName, reference.machine);

                Action<EmptyEventArgs> handler = args => TriggerUpdate(reference);
                EventBus.Register(hook, handler);

                data.Hook = hook;
                data.Handler = handler;
            }

            SetValue(flow);
        }
        protected virtual void EndActivate(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            data.IsActivate = false;

            if (!UseManual)
            {
                EventBus.Unregister(data.Hook, data.Handler);

                data.Handler = null;
            }

            ResetValue(flow);
        }
        private ControlOutput OnManualUpdate(Flow flow)
        {
            if (!ShouldTrigger(flow))
            {
                return null;
            }

            UpdateValue(flow);

            OnUpdate(flow);

            flow.Dispose();

            return null;
        }

        private void TriggerUpdate(GraphReference reference)
        {
            var flow = Flow.New(reference);

            if (!ShouldTrigger(flow))
            {
                flow.Dispose();
                return;
            }

            UpdateValue(flow);

            OnUpdate(flow);

            flow.Dispose();
        }

        protected abstract void OnUpdate(Flow flow);

        protected abstract void SetValue(Flow flow);

        protected virtual void ResetValue(Flow flow) { }

        protected virtual bool ShouldTrigger(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            return data.IsActivate;
        }

        protected abstract void UpdateValue(Flow flow);
    }

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

    [TypeIcon(typeof(Unity.VisualScripting.Timer))]
    public abstract class WaitTriggerEventUnit<TTarget, TArgs> : Unit, IGraphEventListener, IGraphElementWithData, IGraphEventHandler<TArgs>
    {
        [DoNotSerialize]
        [PortLabel("Enter")]
        [PortLabelHidden]
        public ControlInput Enter;

        [DoNotSerialize]
        [PortLabel("Interrupt")]
        public ControlInput Interrupt;

        [DoNotSerialize]
        [PortLabel("Target")]
        [PortLabelHidden]
        [NullMeansSelf]
        public ValueInput Target;

        [DoNotSerialize]
        [PortLabel("Exit")]
        [PortLabelHidden]
        public ControlOutput Exit;

        [DoNotSerialize]
        [PortLabel("Cancel")]
        public ControlOutput Cancel;

        [DoNotSerialize]
        [PortLabel("Trigger")]
        public ControlOutput TriggerEvent;

        public abstract string HookName { get; }
        public abstract Type MessageListenerType { get; }

        protected override void Definition()
        {
            Enter = ControlInput(nameof(Enter), OnEnter);
            Interrupt = ControlInput(nameof(Interrupt), OnInterrupt);
            Exit = ControlOutput(nameof(Exit));
            Cancel = ControlOutput(nameof(Cancel));
            TriggerEvent = ControlOutput(nameof(TriggerEvent));

            Target = ValueInput<GameObject>(nameof(Target), null).NullMeansSelf();

            Succession(Enter, Exit);
            Succession(Interrupt, Cancel);

        }

        public class Data : EventUnit<TArgs>.Data
        {
            public bool IsActivate;
            public GameObject GameObject;
            public TTarget Target;
        }

        public void StartListening(GraphStack stack)
        {
            var data = stack.GetElementData<Data>(this);

            if (data.isListening)
                return;

            data.isListening = true;
        }

        public void StopListening(GraphStack stack)
        {
            var data = stack.GetElementData<Data>(this);

            if (!data.isListening)
                return;

            EventBus.Unregister(data.hook, data.handler);

            data.isListening = false;
            data.IsActivate = false;
            data.handler = null;
        }

        public bool IsListening(GraphPointer pointer)
        {
            if (!pointer.hasData)
                return false;

            return pointer.GetElementData<Data>(this).isListening;
        }

        public abstract IGraphElementData CreateData();

        private ControlOutput OnInterrupt(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            if (!data.IsActivate)
                return null;

            EventBus.Unregister(data.hook, data.handler);

            data.isListening = false;
            data.IsActivate = false;
            data.handler = null;

            return Cancel;
        }
        private ControlOutput OnEnter(Flow flow)
        {
            var target = flow.GetValue<GameObject>(Target);

            if (MessageListenerType != null)
            {
                MessageListener.AddTo(MessageListenerType, target);
            }
            
            var data = flow.stack.GetElementData<Data>(this);

            data.GameObject = target;
            data.Target = target.GetComponent<TTarget>();

            SetData(flow);

            var reference = flow.stack.ToReference();
            var hook = GetHook(reference);
            Action<TArgs> handler = args => Trigger(reference, args);
            EventBus.Register(hook, handler);

            data.hook = hook;
            data.handler = handler;

            data.IsActivate = true;
            

            return Exit;
        }

        public virtual EventHook GetHook(GraphReference reference)
        {
            if (!reference.hasData)
            {
                return HookName;
            }

            var data = reference.GetElementData<Data>(this);

            return new EventHook(HookName, data.Target);
        }

        public void Trigger(GraphReference reference, TArgs args)
        {
            var flow = Flow.New(reference);

            if (!ShouldTrigger(flow, args))
            {
                flow.Dispose();
                return;
            }

            AssignArguments(flow, args);

            flow.Run(TriggerEvent);
        }

        protected virtual void SetData(Flow flow)
        {

        }
        protected virtual bool ShouldTrigger(Flow flow, TArgs args)
        {
            return true;
        }

        protected virtual void AssignArguments(Flow flow, TArgs args)
        {
        }
    }


}
#endif