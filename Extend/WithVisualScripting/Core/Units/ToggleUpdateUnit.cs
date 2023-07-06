#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace StudioScor.Utilities.VisualScripting
{
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

        /// <summary>
        /// Enter 에 Flow 가 진입할 때.
        /// </summary>
        /// <param name="flow"></param>
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
        /// <summary>
        /// Interrupt 되거나 StopListening 되었을 때.
        /// </summary>
        /// <param name="flow"></param>
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

        /// <summary>
        /// 수동, 자동 상관없이 업데이트 되었을 때
        /// </summary>
        /// <param name="flow"></param>
        protected abstract void OnUpdate(Flow flow);

        /// <summary>
        /// 활성화 되었을 때, 값 설정
        /// </summary>
        /// <param name="flow"></param>
        protected abstract void SetValue(Flow flow);

        /// <summary>
        /// 유닛이 비활성화 될 때
        /// </summary>
        /// <param name="flow"></param>
        protected virtual void ResetValue(Flow flow) { }

        /// <summary>
        /// 업데이트 가능 여부 확인
        /// </summary>
        /// <param name="flow"></param>
        /// <returns></returns>
        protected virtual bool ShouldTrigger(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            return data.IsActivate;
        }

        /// <summary>
        /// Update 가 실행되기 전에 Value 를 업데이트할 때.
        /// </summary>
        /// <param name="flow"></param>
        protected abstract void UpdateValue(Flow flow);
    }


}
#endif