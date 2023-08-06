using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class ParticleStopEventReceiver : MonoBehaviour
    {
        [Header(" [ Particle Stop Event Receiver ] ")]
        [SerializeField] private UnityEvent onStoppedParticle;

        public event UnityAction OnStoppedParticle;

        public void OnParticleSystemStopped()
        {
            onStoppedParticle?.Invoke();
            OnStoppedParticle?.Invoke();
        }
    }
}