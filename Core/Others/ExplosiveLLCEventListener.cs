using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class ExplosiveLLCEventListener : BaseMonoBehaviour
    {
        [System.Serializable]
        public class UnityEvents
        {
            [Header("Foot R")]
            [SerializeField] private UnityEvent _onFootR;

            [Header("Foot L")]
            [SerializeField] private UnityEvent _onFootL;

            [Header("Hit")]
            [SerializeField] private UnityEvent _onHit;

            [Header("Land")]
            [SerializeField] private UnityEvent _onLand;

            [Header("Shoot")]
            [SerializeField] private UnityEvent _onShoot;

            public void AddUnityEvent(ExplosiveLLCEventListener explosive)
            {
                if (!explosive)
                    return;

                explosive.OnFootL += Explosive_OnFootL;
                explosive.OnFootR += Explosive_OnFootR;
                explosive.OnHit += Explosive_OnHit;
                explosive.OnLand += Explosive_OnLand;
                explosive.OnShoot += Explosive_OnShoot;
            }
            public void RemoveUnityEvent(ExplosiveLLCEventListener explosive)
            {
                if (!explosive)
                    return;

                explosive.OnFootL -= Explosive_OnFootL;
                explosive.OnFootR -= Explosive_OnFootR;
                explosive.OnHit -= Explosive_OnHit;
                explosive.OnLand -= Explosive_OnLand;
                explosive.OnShoot -= Explosive_OnShoot;
            }


            private void Explosive_OnLand()
            {
                _onLand?.Invoke();
            }

            private void Explosive_OnHit()
            {
               _onHit?.Invoke();
            }

            private void Explosive_OnFootR()
            {
                _onFootR?.Invoke();
            }

            private void Explosive_OnFootL()
            {
                _onFootL?.Invoke();
            }

            private void Explosive_OnShoot()
            {
                _onShoot?.Invoke();
            }
        }
        [Header(" [ Explosive LLC Event Listener ] ")]
        [SerializeField] private bool _useUnityEvent = false;
        [SerializeField] private UnityEvents _unityEvents;

        public event UnityAction OnFootR;
        public event UnityAction OnFootL;
        public event UnityAction OnHit;
        public event UnityAction OnLand;
        public event UnityAction OnShoot;

        private void Awake()
        {
            if(_useUnityEvent)
                _unityEvents.AddUnityEvent(this);
        }
        private void OnDestroy()
        {
            if (_useUnityEvent)
                _unityEvents.RemoveUnityEvent(this);
        }

        public void FootR()
        {
            Inovke_OnFootR();
        }
        public void FootL()
        {
            Invoke_OnFootL();
        }
        public void Hit()
        {
            Invoke_OnHit();
        }
        public void Land()
        {
            Invoke_OnLand();
        }
        public void Shoot()
        {
            Invoke_OnShoot();
        }
        protected virtual void Inovke_OnFootR()
        {
            Log($"{nameof(OnFootR)}");

            OnFootR?.Invoke();
        }
        protected virtual void Invoke_OnFootL()
        {
            Log($"{nameof(OnFootL)}");

            OnFootL?.Invoke();
        }
        protected virtual void Invoke_OnHit()
        {
            Log($"{nameof(OnHit)}");

            OnHit?.Invoke();
        }
        protected virtual void Invoke_OnLand()
        {
            Log($"{nameof(OnLand)}");

            OnLand?.Invoke();
        }
        protected virtual void Invoke_OnShoot()
        {
            Log($"{nameof(OnShoot)}");

            OnShoot?.Invoke();
        }
    }
}