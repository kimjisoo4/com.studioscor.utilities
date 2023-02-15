using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class ExplosiveLLCEventListener : MonoBehaviour
    {
        public UnityEvent OnFootR;
        public UnityEvent OnFootL;
        public UnityEvent OnHit;

        public void FootR()
        {
            OnFootR?.Invoke();
        }
        public void FootL()
        {
            OnFootL?.Invoke();
        }
        public void Hit()
        {
            OnHit?.Invoke();
        }
    }
}