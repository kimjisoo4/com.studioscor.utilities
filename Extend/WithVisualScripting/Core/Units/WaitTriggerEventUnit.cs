#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace StudioScor.Utilities.VisualScripting
{
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