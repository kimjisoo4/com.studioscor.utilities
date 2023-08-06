using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Events;

namespace StudioScor.Utilities
{

    public class OnTriggerComponent : BaseMonoBehaviour
    {
        [Header(" [ On Trigger Component ] ")]
        [SerializeField] private IgnoreColliderDecision[] ignoreDecisions;

        [Header(" [ Trigger Enter & Exit ] ")]
        [SerializeField] private bool useTriggerEnter = true;
        [SerializeField] private bool isOnceEnter = false;
        [SerializeField] private bool useTriggerExit = false;
        [SerializeField] private bool isOnceExit = false;

        [Header(" [ Trigger Stay ]")]
        [SerializeField] private bool useTriggerStay = false;

        [Header(" [ Trigger Events ] ")]
        [Space(5f)]
        [SerializeField] private UnityEvent<Collider> OnEnteredTrigger;
        [SerializeField] private UnityEvent<List<Collider>> OnStayedTrigger;
        [SerializeField] private UnityEvent<Collider> OnExitedTrigger;

        private bool wasEnterTrigger;
        private bool wasExitTrigger;
        private bool shouldStayTrigger;
        private List<Collider> stayedColliders;

        public List<Collider> StayedColliders => stayedColliders;

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
            if(useTriggerStay)
                stayedColliders = new();
        }

        private void Update()
        {
            if (!useTriggerStay)
                return;

            if (!shouldStayTrigger)
                return;

            TriggerStay();

            Callback_TriggerStay();
        }

        protected virtual bool CanTrigger(Collider other)
        {
            if (ignoreDecisions is null || ignoreDecisions.Length == 0)
                return true;

            foreach (var ignoreDecision in ignoreDecisions)
            {
                if (!ignoreDecision.Decision(other))
                    return false;
            }

            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((!useTriggerEnter || (isOnceEnter && wasEnterTrigger)) && !useTriggerStay)
                return;
            
            if (!CanTrigger(other))
                return;

            if (useTriggerEnter)
            {
                wasEnterTrigger = true;

                TriggerEnter(other);

                Callback_TriggerEnter(other);
            }

            if (useTriggerStay && !stayedColliders.Contains(other))
            {
                stayedColliders.Add(other);

                shouldStayTrigger = true;
            }

            DrawSphere(other.bounds.center, Color.red);
        }

        private void OnTriggerExit(Collider other)
        {
            if ((!useTriggerExit || (isOnceExit && wasExitTrigger)) && !useTriggerStay)
                return;

            if (!CanTrigger(other))
                return;

            if (useTriggerExit)
            {
                wasExitTrigger = true;

                TriggerExit(other);

                Callback_TriggerExit(other);
            }

            if (useTriggerStay && stayedColliders.Contains(other))
            {
                stayedColliders.Remove(other);

                shouldStayTrigger = stayedColliders.Count > 0;
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
        


        #region Callback
        private void Callback_TriggerEnter(Collider other)
        {
            Log($"On Trigger Enter - [ {other.name} ]");

            OnEnteredTrigger?.Invoke(other);
        }
        private void Callback_TriggerExit(Collider other)
        {
            Log($"On Trigger Exit - [ {other.name} ]");

            OnExitedTrigger?.Invoke(other);
        }
        private void Callback_TriggerStay()
        {
            if (stayedColliders.Count <= 0)
                return;

            Log($"On Trigger Stay - [ {stayedColliders.Count} ]");

            OnStayedTrigger?.Invoke(stayedColliders);
        }

        #endregion
    }
}