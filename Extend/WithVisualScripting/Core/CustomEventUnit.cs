#if SCOR_ENABLE_VISUALSCRIPTING
using System.Collections.Generic;
using UnityEngine;

using Unity.VisualScripting;
using System;

namespace StudioScor.Utilities.VisualScripting
{
    public abstract class CustomScriptableEventUnit<TTarget, TArgs> : EventUnit<TArgs> where TTarget : ScriptableObject
    {
        protected abstract string HookName { get; }

        [DoNotSerialize]
        [PortLabelHidden]
        [NullMeansSelf]
        public ValueInput Target;

        protected override bool register => true;

        public override EventHook GetHook(GraphReference reference)
        {
            if (!reference.hasData)
            {
                return HookName;
            }

            var data = reference.GetElementData<Data>(this);

            return new EventHook(HookName, data.Target);
        }


        public override IGraphElementData CreateData()
        {
            return new Data();
        }
        public new class Data : EventUnit<TArgs>.Data
        {
            public TTarget Target;
        }

        protected override void Definition()
        {
            base.Definition();

            Target = ValueInput<TTarget>(nameof(Target), null).NullMeansSelf();
        }

        private void UpdateTarget(GraphStack stack)
        {
            var data = stack.GetElementData<Data>(this);

            var wasListening = data.isListening;

            var target = Flow.FetchValue<TTarget>(Target, stack.ToReference());

            if (target != data.Target)
            {
                if (wasListening)
                {
                    StopListening(stack);
                }

                data.Target = target;

                if (wasListening)
                {
                    StartListening(stack, false);
                }
            }
        }

        protected void StartListening(GraphStack stack, bool updateTarget)
        {
            if (updateTarget)
            {
                UpdateTarget(stack);
            }

            var data = stack.GetElementData<Data>(this);

            if (data.Target is null)
            {
                return;
            }

            if (UnityThread.allowsAPI)
            {
                TryAddEventBus(data);
            }

            base.StartListening(stack);
        }

        protected abstract void TryAddEventBus(Data data);

        public override void StartListening(GraphStack stack)
        {
            StartListening(stack, true);
        }
    }

    public abstract class CustomInterfaceEventUnit<TTarget, TArgs> : EventUnit<TArgs> where TTarget : class
    {
        public abstract Type MessageListenerType { get; }
        protected abstract string HookName { get; }

        [DoNotSerialize]
        [PortLabelHidden]
        [NullMeansSelf]
        public ValueInput Target;

        protected override bool register => true;
        public override EventHook GetHook(GraphReference reference)
        {
            if (!reference.hasData)
            {
                return HookName;
            }

            var data = reference.GetElementData<Data>(this);

            return new EventHook(HookName, data.Target);
        }


        public override IGraphElementData CreateData()
        {
            return new Data();
        }
        public new class Data : EventUnit<TArgs>.Data
        {
            public GameObject GameObject;
            public TTarget Target;
        }

        protected override void Definition()
        {
            base.Definition();

            Target = ValueInput<GameObject>(nameof(Target), null).NullMeansSelf();
        }

        private void UpdateTarget(GraphStack stack)
        {
            var data = stack.GetElementData<Data>(this);

            var wasListening = data.isListening;

            var target = Flow.FetchValue<GameObject>(Target, stack.ToReference());

            if (data.GameObject != target)
            {
                if (wasListening)
                {
                    StopListening(stack);
                }

                data.GameObject = target;
                data.Target = data.GameObject.GetComponent<TTarget>();

                if (wasListening)
                {
                    StartListening(stack, false);
                }
            }
        }

        protected void StartListening(GraphStack stack, bool updateTarget)
        {
            if (updateTarget)
            {
                UpdateTarget(stack);
            }

            var data = stack.GetElementData<Data>(this);

            if (!data.GameObject)
            {
                return;
            }

            if (UnityThread.allowsAPI)
            {
                if (MessageListenerType != null)
                {
                    MessageListener.AddTo(MessageListenerType, data.GameObject);
                }
            }

            base.StartListening(stack);
        }

        public override void StartListening(GraphStack stack)
        {
            StartListening(stack, true);
        }

    }

    public abstract class CustomEventUnit<TTarget, TArgs> : EventUnit<TArgs> where TTarget : MonoBehaviour
    {
        public abstract Type MessageListenerType { get; }
        protected abstract string HookName { get; }

        [DoNotSerialize]
        [PortLabelHidden]
        [NullMeansSelf]
        public ValueInput Target;

        protected override bool register => true;
        public override EventHook GetHook(GraphReference reference)
        {
            if (!reference.hasData)
            {
                return HookName;
            }

            var data = reference.GetElementData<Data>(this);

            return new EventHook(HookName, data.Target);
        }


        public override IGraphElementData CreateData()
        {
            return new Data();
        }
        public new class Data : EventUnit<TArgs>.Data
        {
            public TTarget Target;
        }

        protected override void Definition()
        {
            base.Definition();

            Target = ValueInput<TTarget>(nameof(Target), null).NullMeansSelf();
        }

        private void UpdateTarget(GraphStack stack)
        {
            var data = stack.GetElementData<Data>(this);

            var wasListening = data.isListening;

            var target = Flow.FetchValue<TTarget>(Target, stack.ToReference());

            if (target != data.Target)
            {
                if (wasListening)
                {
                    StopListening(stack);
                }

                data.Target = target;

                if (wasListening)
                {
                    StartListening(stack, false);
                }
            }
        }

        protected void StartListening(GraphStack stack, bool updateTarget)
        {
            if (updateTarget)
            {
                UpdateTarget(stack);
            }

            var data = stack.GetElementData<Data>(this);

            if (data.Target is null)
            {
                return;
            }

            if (UnityThread.allowsAPI)
            {
                if (MessageListenerType != null)
                    MessageListener.AddTo(MessageListenerType, data.Target.gameObject);
            }

            base.StartListening(stack);
        }

        public override void StartListening(GraphStack stack)
        {
            StartListening(stack, true);
        }
    }

    public abstract class GameObjectCustomEventUnit<TArgs> : EventUnit<TArgs>
    {
        public abstract Type MessageListenerType { get; }
        protected abstract string HookName { get; }

        [DoNotSerialize]
        [PortLabelHidden]
        [NullMeansSelf]
        public ValueInput Target;

        protected override bool register => true;
        public override EventHook GetHook(GraphReference reference)
        {
            if (!reference.hasData)
            {
                return HookName;
            }

            var data = reference.GetElementData<Data>(this);

            return new EventHook(HookName, data.Target);
        }


        public override IGraphElementData CreateData()
        {
            return new Data();
        }
        public new class Data : EventUnit<TArgs>.Data
        {
            public GameObject Target;
        }

        protected override void Definition()
        {
            base.Definition();

            Target = ValueInput<GameObject>(nameof(Target), null).NullMeansSelf();
        }

        private void UpdateTarget(GraphStack stack)
        {
            var data = stack.GetElementData<Data>(this);

            var wasListening = data.isListening;

            var target = Flow.FetchValue<GameObject>(Target, stack.ToReference());

            if (target != data.Target)
            {
                if (wasListening)
                {
                    StopListening(stack);
                }

                data.Target = target;

                if (wasListening)
                {
                    StartListening(stack, false);
                }
            }
        }

        protected void StartListening(GraphStack stack, bool updateTarget)
        {
            if (updateTarget)
            {
                UpdateTarget(stack);
            }

            var data = stack.GetElementData<Data>(this);

            if (data.Target is null)
            {
                return;
            }

            if (UnityThread.allowsAPI)
            {
                if (MessageListenerType != null)
                    TryAddTo(data);
            }

            base.StartListening(stack);
        }

        protected virtual void TryAddTo(Data data)
        {
            MessageListener.AddTo(MessageListenerType, data.Target);
        }

        public override void StartListening(GraphStack stack)
        {
            StartListening(stack, true);
        }
    }
}

#endif