using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Events;
using static StudioScor.Utilities.ITriggerArea;

namespace StudioScor.Utilities
{
    public interface ITriggerArea
    {
        public delegate void TriggerStateHandler(ITriggerArea triggerArea);
        public delegate void TriggerEventHandler(ITriggerArea triggerArea, Collider collider);


        public GameObject gameObject { get; }
        public Transform transform { get; }
        public bool UseTriggerEnter { get; }
        public bool UseTriggerExit { get; }
        public bool UseTriggerStay { get; }

        public IReadOnlyList<Collider> StaryedColliders { get; }

        public void Activate();
        public void Deactivate();

        public event TriggerEventHandler OnEnteredTrigger;
        public event TriggerEventHandler OnExitedTrigger;
        public event TriggerStateHandler OnStayedTrigger;
    }

    public class TriggerAreaComponent : BaseMonoBehaviour, ITriggerArea
    {
        [System.Serializable]
        public class UnityEvents
        {
            [SerializeField] private UnityEvent<Collider> _onEnteredTrigger;
            [SerializeField] private UnityEvent _onStayedTrigger;
            [SerializeField] private UnityEvent<Collider> _onExitedTrigger;
            public void AddUnityEvent(ITriggerArea triggerArea)
            {
                triggerArea.OnEnteredTrigger += TriggerArea_OnEnteredTrigger;
                triggerArea.OnExitedTrigger += TriggerArea_OnExitedTrigger;
                triggerArea.OnStayedTrigger += TriggerArea_OnStayedTrigger;
            }
            public void RemoveUnityEvent(ITriggerArea triggerArea)
            {
                if (triggerArea is not null)
                {
                    triggerArea.OnEnteredTrigger -= TriggerArea_OnEnteredTrigger;
                    triggerArea.OnExitedTrigger -= TriggerArea_OnExitedTrigger;
                    triggerArea.OnStayedTrigger -= TriggerArea_OnStayedTrigger;
                }
            }
            private void TriggerArea_OnStayedTrigger(ITriggerArea triggerArea)
            {
                _onStayedTrigger.Invoke();
            }

            private void TriggerArea_OnExitedTrigger(ITriggerArea triggerArea, Collider collider)
            {
                _onExitedTrigger?.Invoke(collider);
            }

            private void TriggerArea_OnEnteredTrigger(ITriggerArea triggerArea, Collider collider)
            {
                _onEnteredTrigger?.Invoke(collider);
            }
        }

        [Header(" [ Trigger Area Component ] ")]
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

        private readonly List<Collider> _stayedColliders = new();
        public bool UseTriggerEnter => _useTriggerEnter;
        public bool UseTriggerExit => _useTriggerExit;
        public bool UseTriggerStay => _useTriggerStay;
        public IReadOnlyList<Collider> StaryedColliders => _stayedColliders;

        public event TriggerEventHandler OnEnteredTrigger;
        public event TriggerStateHandler OnStayedTrigger;
        public event TriggerEventHandler OnExitedTrigger;


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
        }
        private void OnDestroy()
        {
            if (_useUnityEvent)
                _unityEvents.RemoveUnityEvent(this);
        }

        protected virtual void OnDisable()
        {
            if(!_stayedColliders.IsNullOrEmpty())
            {
                for (int i = 0; i < _stayedColliders.Count; i++)
                {
                    var collider = _stayedColliders[i];

                    if (!collider)
                        continue;

                    TriggerExit(collider);
                    Invoke_OnExitedTrigger(collider);
                }

                _stayedColliders.Clear();
            }

            _wasEnterTrigger = false;
            _wasExitTrigger = false;
            _shouldStayTrigger = false;
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

        public virtual void Activate()
        {
            enabled = true;
        }

        public virtual void Deactivate()
        {
            enabled = false;
        }

        protected virtual bool CanTriggerEnter(Collider other)
        {
            if (_stayedColliders.Contains(other))
                return false;

            return true;
        }
        protected virtual bool CanTriggerExit(Collider other)
        {
            if (!_stayedColliders.Contains(other))
                return false;

            return true;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!_useTriggerEnter && !_useTriggerStay)
                return;

            if (_isOnceEnter && _wasEnterTrigger)
                return;

            if (!CanTriggerEnter(other))
                return;

            _stayedColliders.Add(other);

            if (_useTriggerEnter)
            {
                _wasEnterTrigger = true;

                TriggerEnter(other);

                Invoke_OnEnteredTrigger(other);
            }

            if (_useTriggerStay && !_shouldStayTrigger)
            {
                _shouldStayTrigger = true;
            }

            DrawSphere(other.bounds.center, Color.green);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_useTriggerExit && !_useTriggerStay)
                return;

            if (_isOnceExit && _wasExitTrigger)
                return;

            if (!CanTriggerExit(other))
                return;

            _stayedColliders.Remove(other);

            if (_useTriggerExit)
            {
                _wasExitTrigger = true;

                TriggerExit(other);

                Invoke_OnExitedTrigger(other);
            }

            if (_useTriggerStay && _shouldStayTrigger)
            {
                _shouldStayTrigger = _stayedColliders.Count > 0;
            }

            DrawSphere(other.bounds.center, Color.red);
        }


        protected virtual void TriggerEnter(Collider other)
        {

        }
        protected virtual void TriggerExit(Collider other)
        {

        }
        protected virtual void TriggerStay()
        {

        }

        #region Invoke
        private void Invoke_OnEnteredTrigger(Collider other)
        {
            Log($"{nameof(OnEnteredTrigger)} - [ {other.name} ]");

            OnEnteredTrigger?.Invoke(this, other);
        }
        private void Invoke_OnExitedTrigger(Collider other)
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