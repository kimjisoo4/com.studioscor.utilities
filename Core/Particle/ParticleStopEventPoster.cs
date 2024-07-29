using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class ParticleStopEventPoster : BaseMonoBehaviour
    {
        [Header(" [ Particle Stop Event Receiver ] ")]
        [SerializeField] private bool _useUnityEvent = true;
        [SerializeField] private UnityEvent _onStoppedParticle;

        public event UnityAction OnStoppedParticle;

        public void OnParticleSystemStopped()
        {
            Log(nameof(OnStoppedParticle));

            if(_useUnityEvent)
                _onStoppedParticle?.Invoke();

            OnStoppedParticle?.Invoke();
        }
    }
}