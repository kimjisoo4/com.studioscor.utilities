using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class ParticleStopEventPoster : BaseMonoBehaviour
    {
        [Header(" [ Particle Stop Event Receiver ] ")]
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private bool _useDisable = true;
        [SerializeField] private ToggleableUnityEvent _onStoppedParticle;

        public ParticleSystem ParticleSystem => _particleSystem;
        public event UnityAction OnStoppedParticle;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if(!_particleSystem)
            {
                _particleSystem = GetComponent<ParticleSystem>();
            }
            else
            {
                if(_particleSystem.main.stopAction != ParticleSystemStopAction.Callback)
                {
                    var main = _particleSystem.main;

                    main.stopAction = ParticleSystemStopAction.Callback;
                }
            }
#endif
        }
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