using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class ParticleCallBack : MonoBehaviour
    {
        [SerializeField] private UnityEvent _OnParticleStopped;

        public void OnParticleSystemStopped()
        {
            _OnParticleStopped?.Invoke();    
        }
    }
}