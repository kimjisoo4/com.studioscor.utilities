using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class ExplosiveLLCEventListener : MonoBehaviour
    {
        [SerializeField] private UnityEvent onFootR;
        [SerializeField] private UnityEvent onFootL;
        [SerializeField] private UnityEvent onHit;

        public event UnityAction OnFootR;
        public event UnityAction OnFootL;
        public event UnityAction OnHit;

        public void FootR()
        {
            Callback_OnFootR();
        }
        public void FootL()
        {
            Callback_OnFootL();
        }
        public void Hit()
        {
            Callback_OnHit();
        }

        protected virtual void Callback_OnFootR()
        {
            OnFootR?.Invoke();
            onFootR?.Invoke();
        }
        protected virtual void Callback_OnFootL()
        {
            OnFootL?.Invoke();
            onFootL?.Invoke();
        }
        protected virtual void Callback_OnHit()
        {
            OnHit?.Invoke();
            onHit?.Invoke();
        }
    }
}