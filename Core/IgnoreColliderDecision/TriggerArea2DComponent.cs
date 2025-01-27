using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public interface ITriggerArea2D
    {
        public delegate void TriggerStateHandler(ITriggerArea2D triggerArea);
        public delegate void TriggerEventHandler(ITriggerArea2D triggerArea, Collider2D collider);

        public GameObject gameObject { get; }
        public Transform transform { get; }

        public void SetEnable(bool isEnable);
        public bool UseTriggerEnter { get; }
        public bool UseTriggerExit { get; }
        public bool UseTriggerStay { get; }

        public IReadOnlyList<Collider2D> StaryedColliders { get; }

        public event TriggerEventHandler OnEnteredTrigger;
        public event TriggerEventHandler OnExitedTrigger;
        public event TriggerStateHandler OnStayedTrigger;
    }
    public class TriggerArea2DComponent : BaseMonoBehaviour, ITriggerArea2D
    {
        [System.Serializable]
        public class UnityEvents
        {
            [SerializeField] private UnityEvent<Collider2D> _onEnteredTrigger;
            [SerializeField] private UnityEvent _onStayedTrigger;
            [SerializeField] private UnityEvent<Collider2D> _onExitedTrigger;
            public void AddUnityEvent(ITriggerArea2D triggerArea)
            {
                triggerArea.OnEnteredTrigger += TriggerArea_OnEnteredTrigger;
                triggerArea.OnExitedTrigger += TriggerArea_OnExitedTrigger;
                triggerArea.OnStayedTrigger += TriggerArea_OnStayedTrigger;
            }
            public void RemoveUnityEvent(ITriggerArea2D triggerArea)
            {
                if (triggerArea is not null)
                {
                    triggerArea.OnEnteredTrigger -= TriggerArea_OnEnteredTrigger;
                    triggerArea.OnExitedTrigger -= TriggerArea_OnExitedTrigger;
                    triggerArea.OnStayedTrigger -= TriggerArea_OnStayedTrigger;
                }
            }
            private void TriggerArea_OnStayedTrigger(ITriggerArea2D triggerArea)
            {
                _onStayedTrigger.Invoke();
            }

            private void TriggerArea_OnExitedTrigger(ITriggerArea2D triggerArea, Collider2D collider)
            {
                _onExitedTrigger?.Invoke(collider);
            }

            private void TriggerArea_OnEnteredTrigger(ITriggerArea2D triggerArea, Collider2D collider)
            {
                _onEnteredTrigger?.Invoke(collider);
            }
        }

        [Header(" [ Trigger Area 2D Component ] ")]
        [Header(" Trigger Enter & Exit ")]
        [SerializeField] private bool _useTriggerEnter = true;
        [SerializeField] private bool _isOnceEnter = false;
        [Space(5f)]
        [SerializeField] private bool _useTriggerExit = false;
        [SerializeField] private bool _isOnceExit = false;
        [Space(5f)]
        [SerializeField] private bool _useTriggerStay = false;

        [Header(" Unity Events ")]
        [SerializeField] private bool _useUnityEvent = false;
        [SerializeField] private UnityEvents _unityEvents;

        private List<Collider2D> _stayedColliders;
        public bool UseTriggerEnter => _useTriggerEnter;
        public bool UseTriggerExit => _useTriggerExit;
        public bool UseTriggerStay => _useTriggerStay;
        public IReadOnlyList<Collider2D> StaryedColliders => _stayedColliders;

        public event ITriggerArea2D.TriggerEventHandler OnEnteredTrigger;
        public event ITriggerArea2D.TriggerStateHandler OnStayedTrigger;
        public event ITriggerArea2D.TriggerEventHandler OnExitedTrigger;


        private bool _wasEnterTrigger;
        private bool _wasExitTrigger;
        private bool _shouldStayTrigger;

        #region EDITOR ONLY
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void DrawSphere(Vector3 position, Color color)
        {
#if UNITY_EDITOR
            if (UseDebug)
                SUtility.Debug.DrawSphere(position, 0.1f, color, 1f);
#endif
        }

        #endregion

        private void Awake()
        {
            if (_useUnityEvent)
                _unityEvents.AddUnityEvent(this);

            if (_useTriggerStay)
                _stayedColliders = new();
        }
        private void OnDestroy()
        {
            if (_useUnityEvent)
                _unityEvents.RemoveUnityEvent(this);
        }

        private void FixedUpdate()
        {
            if (!_useTriggerStay)
                return;

            if (!_shouldStayTrigger)
                return;

            TriggerStay();

            Invoke_OnStayedTrigger();
        }

       

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!enabled)
                return;

            if (!_useTriggerEnter && !_useTriggerStay)
                return;

            if (_isOnceEnter && _wasEnterTrigger)
                return;

            if (!CanTrigger(other))
                return;

            if (_useTriggerEnter)
            {
                _wasEnterTrigger = true;

                TriggerEnter(other);

                Invoke_OnEnteredTrigger(other);
            }

            if (_useTriggerStay && !_stayedColliders.Contains(other))
            {
                _stayedColliders.Add(other);

                _shouldStayTrigger = true;
            }

            DrawSphere(other.bounds.center, Color.green);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!enabled)
                return;

            if (!_useTriggerExit && !_useTriggerStay)
                return;

            if (_isOnceExit && _wasExitTrigger)
                return;

            if (!CanTrigger(other))
                return;

            if (_useTriggerExit)
            {
                _wasExitTrigger = true;

                TriggerExit(other);

                Invoke_OnExitedTrigger(other);
            }

            if (_useTriggerStay && _stayedColliders.Contains(other))
            {
                _stayedColliders.Remove(other);

                _shouldStayTrigger = _stayedColliders.Count > 0;
            }

            DrawSphere(other.bounds.center, Color.red);
        }

        public void SetEnable(bool enabled)
        {
            this.enabled = enabled;
        }

        protected virtual bool CanTrigger(Collider2D other)
        {
            return true;
        }
        protected virtual void TriggerEnter(Collider2D other)
        {

        }
        protected virtual void TriggerExit(Collider2D other)
        {

        }
        protected virtual void TriggerStay()
        {

        }

        #region Invoke
        private void Invoke_OnEnteredTrigger(Collider2D other)
        {
            Log($"{nameof(OnEnteredTrigger)} - [ {other.name} ]");

            OnEnteredTrigger?.Invoke(this, other);
        }
        private void Invoke_OnExitedTrigger(Collider2D other)
        {
            Log($"{nameof(OnExitedTrigger)} - [ {other.name} ]");

            OnExitedTrigger?.Invoke(this, other);
        }
        private void Invoke_OnStayedTrigger()
        {
            Log($"{nameof(OnStayedTrigger)} - [ {_stayedColliders.Count} ]");

            OnStayedTrigger?.Invoke(this);
        }

        #endregion
    }
}