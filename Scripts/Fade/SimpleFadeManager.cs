using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KimScor.Utilities
{
    public class SimpleFadeManager : Singleton<SimpleFadeManager>
    {
        [SerializeField] private FiniteStateMachineSystem<FadeState> _StateMachine;
        public FiniteStateMachineSystem<FadeState> StateMachine => _StateMachine;

        [SerializeField] private FadeState _DefaultState;
        [SerializeField] private FadeState _FadeInState;
        [SerializeField] private FadeState _FadeOutState;
        [SerializeField] private FadeState _FadedState;

        [SerializeField] private float _FadeInTime = 1f;
        [SerializeField] private float _FadeOutTime = 1f;

        private bool _IsPlayFromStart = false;

        public float FadeAmount => StateMachine.CurrentState.FadeAmount;
        public bool IsFading => StateMachine.CurrentState != _DefaultState && StateMachine.CurrentState != _FadedState;
        public bool IsFadeIn => StateMachine.CurrentState == _FadeInState || StateMachine.CurrentState == _FadedState;
        public bool IsFadeOut => !IsFadeIn;

        public bool IsPlayFromStart => _IsPlayFromStart;

        private float _InstantFadeTime = -1f;
        private bool UseInstantTime => _InstantFadeTime > 0f;
        public float FadeInTime
        {
            get
            {
                return UseInstantTime ? _InstantFadeTime : _FadeInTime;
            }
        }
        public float FadeOutTime
        {
            get
            {
                return UseInstantTime ? _InstantFadeTime : _FadeOutTime;
            }
        }

        protected override void Setup()
        {
            base.Setup();

            _StateMachine = new FiniteStateMachineSystem<FadeState>(_DefaultState);

            _StateMachine.Setup();
        }

        public void OnFadeOut(float duration = -1f, bool playFromStart = false)
        {
            _InstantFadeTime = duration;
            _IsPlayFromStart = playFromStart;

            StateMachine.TrySetState(_FadeOutState);
        }
        public void OnFadeIn(float duration = -1f, bool playFromStart = false)
        {
            _InstantFadeTime = duration;
            _IsPlayFromStart = playFromStart;

            StateMachine.TrySetState(_FadeInState);
        }
        public void EndFadeOut()
        {
            StateMachine.TrySetState(_DefaultState);
        }
        public void EndFadeIn()
        {
            StateMachine.TrySetState(_FadedState);
        }
    }

}
