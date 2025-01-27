using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class ParticleStopEventPoster : BaseMonoBehaviour
    {
        [Header(" [ Particle Stop Event Receiver ] ")]
        [SerializeField] private bool _useDisable = true;
        [SerializeField] private ToggleableUnityEvent _onStoppedParticle;

        public event UnityAction OnStoppedParticle;

        public void OnParticleSystemStopped()
        {
            Log(nameof(OnStoppedParticle));

            _onStoppedParticle.Invoke();
            OnStoppedParticle?.Invoke();

            if (_useDisable)
                gameObject.SetActive(false);
        }
    }
}