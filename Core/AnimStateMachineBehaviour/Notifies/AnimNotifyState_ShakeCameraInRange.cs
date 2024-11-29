using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

namespace StudioScor.Utilities
{
    public class AnimNotifyState_ShakeCameraInRange : AnimNotifyStateBehaviour
    {
        [Header(" [ Shake Camera InRange ] ")]
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _velocity;
        [SerializeField] private bool _forceNoDecay = false;

        [Header(" Inpulse Definition ")]
        [SerializeField] private CinemachineImpulseDefinition _impulse = new();

        private CinemachineImpulseManager.ImpulseEvent _impulseEvent;

        private void OnValidate()
        {
            if(_impulse is not null)
                _impulse.OnValidate();
        }
        protected override void OnEnterNotify(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnEnterNotify(animator, stateInfo, layerIndex);

            _impulseEvent = _impulse.CreateAndReturnEvent(animator.transform.TransformPoint(_position), animator.transform.TransformDirection(_velocity));
        }
        protected override void OnExitNotify(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnExitNotify(animator, stateInfo, layerIndex);

            if(_impulseEvent is not null)
            {
                float stopTime = CinemachineCore.CurrentTime;

                _impulseEvent.Cancel(stopTime, _forceNoDecay);
                _impulseEvent = null;
            }
        }
    }
}
