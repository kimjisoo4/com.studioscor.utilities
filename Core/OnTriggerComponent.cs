using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class OnTriggerComponent : BaseMonoBehaviour
    {
        [Header(" [ Tags ] ")]
        [SerializeField][STagSelector] private string[] _Tags;
        [SerializeField] private bool _IsIgnoreTag;

        [Header(" [ Trigger ] ")]
        [SerializeField] private bool _UseTriggerEnter = true;
        [SerializeField] private bool _UseTriggerStay;
        [SerializeField] private bool _UseTriggerExit;

        [Header(" [ Trigger Events ] ")]
        [Space(5f)]
        [SerializeField] private UnityEvent<Collider> OnEnteredTrigger;
        [SerializeField] private UnityEvent<List<Collider>> OnStayedTrigger;
        [SerializeField] private UnityEvent<Collider> OnExitedTrigger;

        private List<Collider> _StayedColliders;

        #region EDITOR ONLY
        [Conditional("UNITY_EDITOR")]
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
            if(_UseTriggerStay)
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

            bool trigger = false;

            foreach (string tag in _Tags)
            {
                if (other.CompareTag(tag))
                {
                    if (_IsIgnoreTag)
                        continue;

                    trigger = true;
                    
                    break;
                }
                else
                {
                    if (!_IsIgnoreTag)
                        continue;

                    trigger = true;

                    break;
                }
            }

            if(trigger)
            {
                if (_UseTriggerEnter)
                    TriggerEnter(other);

                if (_UseTriggerStay && !_StayedColliders.Contains(other))
                    _StayedColliders.Add(other);

                DrawSphere(other.bounds.center, Color.red);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_UseTriggerExit && !_UseTriggerStay)
                return;

            bool trigger = false;

            foreach (string tag in _Tags)
            {
                if (other.CompareTag(tag))
                {
                    if (_IsIgnoreTag)
                        continue;

                    trigger = true;

                    break;
                }
                else
                {
                    if (!_IsIgnoreTag)
                        continue;

                    trigger = true;

                    break;
                }
            }

            if(trigger)
            {
                if (_UseTriggerExit)
                    TriggerExit(other);

                if (_UseTriggerStay && _StayedColliders.Contains(other))

                    _StayedColliders.Remove(other);

                DrawSphere(other.bounds.center, Color.red);
            }
            
        }

        #region Callback
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
        #endregion
    }
}