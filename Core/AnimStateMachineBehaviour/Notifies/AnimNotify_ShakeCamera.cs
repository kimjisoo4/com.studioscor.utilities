#if SCOR_ENABLE_CINEMACHINE
using Unity.Cinemachine;
using UnityEngine;

namespace StudioScor.Utilities
{
    public class AnimNotify_ShakeCamera : AnimNotifyBehaviour
    {
        [Header(" [ Shake Camera ] ")]
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _velocity;

        [Header(" Inpulse Definition ")]
        [SerializeField] private CinemachineImpulseDefinition _impulse = new();

        private void OnValidate()
        {
            if(_impulse is not null)
                _impulse.OnValidate();
        }

        protected override void OnNotify(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _impulse.CreateEvent(animator.transform.TransformPoint(_position), animator.transform.TransformDirection(_velocity));
        }
    }
}
#endif