using Animancer;
using System;
using UnityEngine;

namespace StudioScor.Utilities.Extend.Animancer
{
#if ANIMANCER
    [Serializable]
    public class AnimancerMainTask : Task, IMainTask
    {
        [Header(" [ Animancer Ability Task ] ")]
        [SerializeField] private ClipTransition _animation;
        [SerializeField][Range(0f, 1f)] private float _fadeOut = 0.2f;

        private ClipTransition Animation => _original is null ? _animation : _original._animation;
        private float FadeOut => _original is null ? _fadeOut : _original._fadeOut;

        public bool IsFixedUpdate => false;
        public float NormalizedTime => _animancerState.NormalizedTime;

        private float _endTime;
        private HybridAnimancerComponent _animancer;
        private AnimancerLayer _layer;
        private AnimancerState _animancerState;

        private AnimancerMainTask _original;

        public override ITask Clone()
        {
            var copy = new AnimancerMainTask();

            copy._original = this;

            return copy;
        }
        protected override void SetupTask()
        {
            base.SetupTask();
            
            _animancer = Owner.GetComponentInChildren<HybridAnimancerComponent>();
            _layer = _animancer.Layers[1];
        }

        protected override void EnterTask()
        {
            base.EnterTask();

            _endTime = 1f - FadeOut;

            _animancerState = _layer.GetOrCreateState(Animation);

            if(_animancerState.IsPlaying)
            {
                _animancerState = _layer.GetOrCreateWeightlessState(_animancerState);
            }

            _animancerState = _layer.Play(_animancerState, Animation.FadeDuration, Animation.FadeMode);
        }
        protected override void ExitTask()
        {
            base.ExitTask();

            _layer.StartFade(0f, FadeOut);
        }


        public void UpdateMainTask(float deltaTime)
        {
            if (!IsPlaying)
                return;

            float normalizedTime = _animancerState.NormalizedTime;

            if (normalizedTime >= _endTime)
            {
                EndTask();
            }
        }
        public void UpdateFixedTask(float deltaTime)
        {
            return;
        }
    }
#endif
}

