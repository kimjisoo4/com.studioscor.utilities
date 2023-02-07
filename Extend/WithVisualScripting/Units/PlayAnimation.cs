using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("Play Animation")]
    [UnitShortTitle("PlayAnimation")]
    [UnitSubtitle("Animation Player Unit")]
    [UnitCategory("Time\\StudioScor\\")]
    public class PlayAnimation : Unit
    {
        [DoNotSerialize]
        [PortLabel("Enter")]
        [PortLabelHidden]
        public ControlInput Enter { get; private set; }

        [DoNotSerialize]
        [PortLabel("Animation Player")]
        [PortLabelHidden]
        [NullMeansSelf]
        public ValueInput AnimationPlayer { get; private set; }

        [DoNotSerialize]
        [PortLabel("Animation")]
        public ValueInput Animation { get; private set; }

        [DoNotSerialize]
        [PortLabel("FadeTime")]
        public ValueInput Fade { get; private set; }

        [DoNotSerialize]
        [PortLabel("OffsetTime")]
        public ValueInput Offset { get; private set; }
        
        [Serialize]
        [Inspectable]
        [InspectorToggleLeft]
        public bool UseName { get; set; } = false;


        [DoNotSerialize]
        [PortLabel("Exit")]
        [PortLabelHidden]
        public ControlOutput Exit { get; private set; }

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
        [PortLabel("On Canceled")]
        public ControlOutput Canceled { get; private set; }

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


        private bool _WasStarted;
        private bool _WasFailed;
        private bool _WasCanceled;
        private bool _WasFinished;
        private bool _WasBlendIn;

        private List<string> _OnNotifys;
        private List<string> _EndNotifys;

        protected override void Definition()
        {
            Enter = ControlInputCoroutine(nameof(Enter), OnFlow);
            AnimationPlayer = ValueInput<AnimationPlayer>(nameof(AnimationPlayer), null).NullMeansSelf();

            if(UseName)
            {
                Animation = ValueInput<string>(nameof(Animation), "Name");
            }
            else
            {
                Animation = ValueInput<int>(nameof(Animation), 0);
            }

            Fade = ValueInput<float>(nameof(Fade), 0.2f);
            Offset = ValueInput<float>(nameof(Offset), 0.0f);

            Exit = ControlOutput(nameof(Exit));
            Started = ControlOutput(nameof(Started));
            Failed = ControlOutput(nameof(Failed));
            Finished = ControlOutput(nameof(Finished));
            Canceled = ControlOutput(nameof(Canceled));
            BlendIn = ControlOutput(nameof(BlendIn));

            OnNotify = ControlOutput(nameof(OnNotify));
            EndNotify = ControlOutput(nameof(EndNotify));
            Notify = ValueOutput<string>(nameof(Notify));

            Succession(Enter, Exit);
            Succession(Enter, Started);
            Succession(Enter, Failed);
            Succession(Enter, Finished);
            Succession(Enter, Canceled);
            Succession(Enter, BlendIn);
            Succession(Enter, OnNotify);
            Succession(Enter, EndNotify);

            Requirement(AnimationPlayer, Enter);
            Requirement(Animation, Enter);
            Requirement(Fade, Enter);
            Requirement(Offset, Enter);

            Assignment(Enter, Notify);
        }

        private IEnumerator OnFlow(Flow flow)
        {
            if (_OnNotifys is null)
                _OnNotifys = new();

            if (_EndNotifys is null)
                _EndNotifys = new();

            _OnNotifys.Clear();
            _EndNotifys.Clear();

            _WasStarted = false;
            _WasCanceled = false;
            _WasFinished = false;
            _WasBlendIn = false;

            var animationPlayer = flow.GetValue<AnimationPlayer>(AnimationPlayer);
            var fadeTime = flow.GetValue<float>(Fade);
            var offsetTime = flow.GetValue<float>(Offset);

            if (UseName)
            {
                var name = flow.GetValue<string>(Animation);

                animationPlayer.PlayAnimation(name, fadeTime, offsetTime);
            }
            else
            {
                var hash = flow.GetValue<int>(Animation);

                animationPlayer.PlayAnimation(hash, fadeTime, offsetTime);
            }

            animationPlayer.OnStarted += AnimationPlayer_OnStarted;
            animationPlayer.OnFailed += AnimationPlayer_OnFailed;
            animationPlayer.OnFinished += AnimationPlayer_OnFinished;
            animationPlayer.OnCanceled += AnimationPlayer_OnCanceled;
            animationPlayer.OnStartedBlendOut += AnimationPlayer_OnStartedBlendOut;

            animationPlayer.OnNotify += AnimationPlayer_OnNotify;
            animationPlayer.OnEnterNotifyState += AnimationPlayer_OnEnterNotifyState;
            animationPlayer.OnExitNotifyState += AnimationPlayer_OnExitNotifyState;

            yield return Exit;

            bool wasStarted = false;
            bool wasBlendIn = false;

            while(true)
            {
                if (!wasStarted)
                {
                    if (_WasStarted)
                    {
                        wasStarted = true;

                        yield return Started;
                    }
                    else if (_WasFailed)
                    {
                        yield return Failed;

                        yield break;
                    }
                }

                if (_OnNotifys.Count > 0)
                {
                    foreach (var notify in _OnNotifys)
                    {
                        flow.SetValue(Notify, notify);

                        yield return OnNotify;
                    }

                    _OnNotifys.Clear();
                }

                if (_EndNotifys.Count > 0)
                {
                    foreach (var notify in _EndNotifys)
                    {
                        flow.SetValue(Notify, notify);

                        yield return EndNotify;
                    }

                    _EndNotifys.Clear();
                }

                if (_WasCanceled)
                {
                    yield return Canceled;

                    yield break;
                }

                if (!wasBlendIn && _WasBlendIn)
                {
                    wasBlendIn = true;

                    yield return BlendIn;
                }

                if (_WasFinished)
                {
                    yield return Finished;

                    yield break;
                }

                yield return null;
            }

        }

        private void AnimationPlayer_OnExitNotifyState(string notify)
        {
            _EndNotifys.Add(notify);
        }
        private void AnimationPlayer_OnEnterNotifyState(string notify)
        {
            _OnNotifys.Add(notify);
        }
        private void AnimationPlayer_OnNotify(string notify)
        {
            _OnNotifys.Add(notify);
        }
        private void AnimationPlayer_OnStartedBlendOut()
        {
            _WasBlendIn = true;
        }
        private void AnimationPlayer_OnCanceled()
        {
            _WasCanceled = true;
        }

        private void AnimationPlayer_OnFinished()
        {
            _WasFinished = true;
        }
        private void AnimationPlayer_OnFailed()
        {
            _WasFailed = true;
        }
        private void AnimationPlayer_OnStarted()
        {
            _WasStarted = true;
        }
    }
}
