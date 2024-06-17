#if ANIMANCER
using Animancer;
using System;
using UnityEngine;

namespace StudioScor.Utilities.Extend.Animancer
{
    [Serializable]
    public class AnimancerMainTask : Task, IMainTask
    {
        [Header(" [ Animancer Ability Task ] ")]
        [SerializeField] private ClipTransition _animation;
        [SerializeField][Range(0f, 1f)] private float _fadeOut = 0.8f;

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

            _endTime = _original is null ? _fadeOut : _original._fadeOut;
            var animation = _original is null ? _animation : _original._animation;

            _animancerState = _layer.GetOrCreateState(animation);

            if(_animancerState.IsPlaying)
            {
                _animancerState = _layer.GetOrCreateWeightlessState(_animancerState);
            }

            _animancerState = _layer.Play(_animancerState, animation.FadeDuration, animation.FadeMode);
        }

        protected override void OnCancelTask()
        {
            base.OnCancelTask();

            _layer.StartFade(0f, 0f);
        }

        protected override void OnComplateTask()
        {
            base.OnComplateTask();

            _layer.StartFade(0f, 1f - _endTime);
        }


        public void UpdateTask(float deltaTime)
        {
            if (!IsPlaying)
                return;

            float normalizedTime = _animancerState.NormalizedTime;

            if (normalizedTime >= _endTime)
            {
                ComplateTask();
            }
        }
        public void FixedUpdateTask(float deltaTime)
        {
            return;
        }
    }
}
#endif

