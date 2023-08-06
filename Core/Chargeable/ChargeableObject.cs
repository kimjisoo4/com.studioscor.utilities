using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class ChargeableObject : BaseMonoBehaviour
    {
        [Header(" [ Charageable Object ] ")]
        [SerializeField] private Chargeable chargeable;
        [SerializeField][SRange(0f, 1f)] private float offset = 0f;
        [SerializeField] private bool isAutoPlaying = true;

        [Header(" [ Events ] ")]
        [SerializeField] private UnityEvent onStartedCharging;
        [SerializeField] private UnityEvent onFinishedCharging;
        [SerializeField] private UnityEvent onReachedCharging;
        public Chargeable Chargeable => chargeable;
        private void Awake()
        {
            chargeable.OnStartedCharging += Callback_OnStartedCharging;
            chargeable.OnFinishedCharging += Callback_OnFinishedCharging;
            chargeable.OnReachedCharging += Callback_OnReachedCharging;
        }

        private void OnEnable()
        {
            if (isAutoPlaying)
                OnCharging();
        }
        private void OnDisable()
        {
            EndCharging();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            UpdateCharging(deltaTime);
        }

        public void OnCharging()
        {
            chargeable.OnCharging(chargedOffset : offset);
        }

        public void EndCharging()
        {
            chargeable.EndCharging();
        }
        public void UpdateCharging(float deltaTime)
        {
            chargeable.UpdateCharging(deltaTime);
        }

        protected virtual void Callback_OnStartedCharging(Chargeable chargeable)
        {
            onStartedCharging?.Invoke();
        }

        protected virtual void Callback_OnReachedCharging(Chargeable chargeable)
        {
            onReachedCharging?.Invoke();
        }

        protected virtual void Callback_OnFinishedCharging(Chargeable chargeable)
        {
            onFinishedCharging?.Invoke();
        }
    }

    
}