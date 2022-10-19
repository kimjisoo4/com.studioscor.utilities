using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;
using UnityEngine.Events;

namespace KimScor.Utilities
{
    public class OnTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string[] _Tags;

        [SerializeField] private bool _UseTriggerEnter;
        [SerializeField] private bool _UseTriggerStay;
        [SerializeField] private bool _UseTriggerExit;

        [SerializeField] private UnityEvent<Collider> OnEnteredTrigger;
        [SerializeField] private UnityEvent<List<Collider>> OnStayedTrigger;
        [SerializeField] private UnityEvent<Collider> OnExitedTrigger;

        [SerializeField] private bool _UseDebug;

        private List<Collider> _StayedColliders;

#if UNITY_EDITOR
        [Conditional("UNITY_EDITOR")]
        void Log(object message)
        {
            if(_UseDebug)
                Utilities.Debug.Log("OnTriggerComponent [ " + name + " ] : " + message, this);
        }
        [Conditional("UNITY_EDITOR")]
        void DrawSphere(Vector3 position, Color color)
        {
            if (_UseDebug)
                Utilities.Debug.DrawSphere(position, 0.1f, color, 1f);
        }
#endif

        private void Awake()
        {
            _StayedColliders = new();
        }

        private void Update()
        {
            if (!_UseTriggerStay)
                return;

            TriggerStay();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_UseTriggerEnter && !_UseTriggerStay)
                return;

            foreach (string tag in _Tags)
            {
                if (other.CompareTag(tag))
                {
                    if(_UseTriggerEnter)
                        TriggerEnter(other);

                    if (_UseTriggerStay && !_StayedColliders.Contains(other))
                        _StayedColliders.Add(other);

                    return;
                }
            }

            DrawSphere(other.bounds.center, Color.red);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_UseTriggerExit && !_UseTriggerStay)
                return;

            foreach (string tag in _Tags)
            {
                if (other.CompareTag(tag))
                {
                    if(_UseTriggerExit)
                        TriggerExit(other);

                    if (_UseTriggerStay && _StayedColliders.Contains(other))
                        _StayedColliders.Remove(other);

                    return;
                }
            }

            DrawSphere(other.bounds.center, Color.red);
        }

        private void TriggerEnter(Collider other)
        {
            Log("On Trigger Enter - " + other);
            DrawSphere(other.bounds.center, Color.green);

            OnEnteredTrigger?.Invoke(other);
        }
        private void TriggerExit(Collider other)
        {
            Log("On Trigger Exit - " + other);
            DrawSphere(other.bounds.center, Color.green);

            OnExitedTrigger?.Invoke(other);
        }
        private void TriggerStay()
        {
            if (_StayedColliders.Count <= 0)
                return;

            Log("On Trigger Stay - " + _StayedColliders.Count);

            OnStayedTrigger?.Invoke(_StayedColliders);
        }
    }
}