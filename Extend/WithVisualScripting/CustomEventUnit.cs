using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.VisualScripting;
using System;

namespace StudioScor.Utilities.VisualScripting
{

    public abstract class CustomGameObjectEventUnit<TTarget, TArgs> : EventUnit<TArgs> where TTarget : MonoBehaviour
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
}
