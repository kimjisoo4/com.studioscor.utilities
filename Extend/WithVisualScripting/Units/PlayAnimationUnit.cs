#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("Play Animation")]
    [UnitShortTitle("PlayAnimation")]
    [UnitSubtitle("Animation Player Wait")]
    [UnitCategory("Time\\StudioScor\\")]
    public class PlayAnimationUnit : UpdateUnit
    {
        [DoNotSerialize]
        [PortLabel("Animation Player")]
        [PortLabelHidden]
        [NullMeansSelf]
        public ValueInput AnimationPlayer { get; private set; }

        [DoNotSerialize]
        [PortLabel("Animation")]
        public ValueInput Animation { get; private set; }

        [DoNotSerialize]
        [PortLabel("FadeIn")]
        public ValueInput FadeIn { get; private set; }

        [DoNotSerialize]
        [PortLabel("FadeOut")]
        public ValueInput FadeOut { get; private set; }

        [DoNotSerialize]
        [PortLabel("OffsetTime")]
        public ValueInput Offset { get; private set; }

        [Serialize]
        [Inspectable]
        [InspectorToggleLeft]
        public bool UseName { get; set; } = true;

        [DoNotSerialize]
        [PortLabel("On Started")]
        public ControlOutput Started { get; private set; }

        [DoNotSerialize]
        [PortLabel("On Failed")]
        public ControlOutput Failed { get; private set; }

        [DoNotSerialize]
        [PortLabel("On Finished")]
        public ControlOutput Finished { get; private set; }

        [DoNotSerialize]
        [PortLabel("On Started BlendOut")]
        public ControlOutput BlendIn { get; private set; }

        [DoNotSerialize]
        [PortLabel("On Notify")]
        public ControlOutput OnNotify { get; private set; }

        [DoNotSerialize]
        [PortLabel("End Notify")]
        public ControlOutput EndNotify { get; private set; }

        [DoNotSerialize]
        [PortLabel("Notify")]
        public ValueOutput Notify { get; private set; }

        [DoNotSerialize]
        [PortLabel("Elapsed(%)")]
        public ValueOutput ElapsedTime { get; private set; }


        public new class Data : UpdateUnit.Data
        {
            public AnimationPlayer AnimationPlayer;
            public int AnimationHash;
            public float FadeIn;
            public float FadeOut;
            public float Offset;

            public bool IsPlayingAnimation;
            public bool WasStarted;
            public bool WasFailed;
            public bool WasCanceled;
            public bool WasFinished;
            public bool WasStartedBlendOut;

            public List<string> OnNotifys;
            public List<string> EndNotifys;

            public void Setup()
            {
                if (OnNotifys is null)
                    OnNotifys = new();

                if (EndNotifys is null)
                    EndNotifys = new();

                OnNotifys.Clear();
                EndNotifys.Clear();

                IsPlayingAnimation = false;

                WasStarted = false;
                WasFailed = false;
                WasCanceled = false;
                WasFinished = false;
                WasStartedBlendOut = false;
            }
            public void OnActivate()
            {
                Setup();

                AnimationPlayer.PlayAnimation(AnimationHash, FadeIn, FadeOut, Offset);

                AnimationPlayer.OnStarted += OnStarted;
                AnimationPlayer.OnFailed += OnFailed;
                AnimationPlayer.OnCanceled += OnCanceled;
                AnimationPlayer.OnFinished += OnFinished;
                AnimationPlayer.OnStartedBlendOut += OnStartedBlendOut;

                AnimationPlayer.OnEnterNotifyState += AnimationPlayer_OnEnterNotifyState;
                AnimationPlayer.OnExitNotifyState += AnimationPlayer_OnExitNotifyState;
                AnimationPlayer.OnNotify += AnimationPlayer_OnNotify;

                IsActivate = true;
            }

            private void AnimationPlayer_OnNotify(string notify)
            {
                OnNotifys.Add(notify);
            }
            private void AnimationPlayer_OnEnterNotifyState(string notify)
            {
                OnNotifys.Add(notify);
            }
            private void AnimationPlayer_OnExitNotifyState(string notify)
            {
                EndNotifys.Add(notify);
            }

            private void OnStarted()
            {
                WasStarted = true;
            }
            private void OnFailed()
            {
                WasFailed = true;
            }
            private void OnCanceled()
            {
                WasCanceled = true;
            }
            private void OnFinished()
            {
                WasFinished = true;
            }
            private void OnStartedBlendOut()
            {
                WasStartedBlendOut = true;
            }
        }


        public override IGraphElementData CreateData()
        {
            return new Data();
        }

        protected override void Definition()
        {
            base.Definition();

            AnimationPlayer = ValueInput<AnimationPlayer>(nameof(AnimationPlayer), null).NullMeansSelf();

            if (UseName)
            {
                Animation = ValueInput<string>(nameof(Animation), "Name");
            }
            else
            {
                Animation = ValueInput<int>(nameof(Animation), 0);
            }

            FadeIn = ValueInput<float>(nameof(FadeIn), 0.2f);
            FadeOut = ValueInput<float>(nameof(FadeOut), 0.8f);
            Offset = ValueInput<float>(nameof(Offset), 0.0f);

            Started = ControlOutput(nameof(Started));
            Failed = ControlOutput(nameof(Failed));
            Finished = ControlOutput(nameof(Finished));
            BlendIn = ControlOutput(nameof(BlendIn));
            OnNotify = ControlOutput(nameof(OnNotify));
            EndNotify = ControlOutput(nameof(EndNotify));

            Notify = ValueOutput<string>(nameof(Notify));
            ElapsedTime = ValueOutput<float>(nameof(ElapsedTime));

            Succession(Enter, Exit);
            Succession(Enter, Started);
            Succession(Enter, Failed);
            Succession(Enter, Finished);
            Succession(Enter, Canceled);
            Succession(Enter, BlendIn);
            Succession(Enter, OnNotify);
            Succession(Enter, EndNotify);
            Succession(Enter, Update);
            Succession(Interrupt, Canceled);

            Requirement(AnimationPlayer, Enter);
            Requirement(Animation, Enter);
            Requirement(FadeIn, Enter);
            Requirement(Offset, Enter);

            Assignment(Enter, Notify);
            Assignment(Enter, ElapsedTime);
        }

        protected override void OnEnter(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            data.AnimationPlayer = flow.GetValue<AnimationPlayer>(AnimationPlayer);
            data.FadeIn = flow.GetValue<float>(FadeIn);
            data.FadeOut = flow.GetValue<float>(FadeOut);
            data.Offset = flow.GetValue<float>(Offset);

            if (UseName)
            {
                data.AnimationHash = Animator.StringToHash(flow.GetValue<string>(Animation));
            }
            else
            {
                data.AnimationHash = flow.GetValue<int>(Animation);
            }

            flow.SetValue(ElapsedTime, data.AnimationPlayer.NormalizedTime);

            data.OnActivate();
        }

        protected override void OnInterrupt(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            data.AnimationPlayer.StopAnimation();
        }

        private void UpdateAnimationEvents(Flow flow, Data data)
        {
            flow.SetValue(ElapsedTime, data.AnimationPlayer.NormalizedTime);

            if(!data.IsPlayingAnimation)
            {
                if (data.WasStarted)
                {
                    data.WasStarted = false;

                    data.IsPlayingAnimation = true;

                    flow.Invoke(Started);
                }
                else if (data.WasFailed)
                {
                    data.WasFailed = false;

                    data.IsActivate = false;

                    flow.Invoke(Failed);

                    return;
                }
            }
            else
            {
                if (data.OnNotifys.Count > 0)
                {
                    foreach (var notify in data.OnNotifys)
                    {
                        flow.SetValue(Notify, notify);

                        flow.Invoke(OnNotify);
                    }

                    data.OnNotifys.Clear();
                }
                if (data.EndNotifys.Count > 0)
                {
                    foreach (var notify in data.EndNotifys)
                    {
                        flow.SetValue(Notify, notify);

                        flow.Invoke(EndNotify);
                    }

                    data.EndNotifys.Clear();
                }

                if (data.WasCanceled)
                {
                    data.WasCanceled = false;

                    data.IsActivate = false;
                    flow.Invoke(Canceled);

                    return;
                }

                if (data.WasStartedBlendOut)
                {
                    data.WasStartedBlendOut = false;

                    flow.Invoke(BlendIn);
                }

                if (data.WasFinished)
                {
                    data.WasFinished = false;

                    data.IsActivate = false;
                    flow.Invoke(Finished);

                    return;
                }


                flow.Invoke(Update);

            }
        }


        protected override void OnUpdate(Flow flow)
        {
            var data = flow.stack.GetElementData<Data>(this);

            UpdateAnimationEvents(flow, data);
        }

        protected override ControlOutput OnManualUpdate(Flow flow)
        {
            return null;
        }
    }
}
#endif