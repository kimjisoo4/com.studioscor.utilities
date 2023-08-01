using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Events;
using System.Collections;
using System.Linq;

namespace StudioScor.Utilities
{
    public class OnTriggerComponent : BaseMonoBehaviour
    {
        [Header(" [ Tags ] ")]
        [SerializeField][STagSelector] private string[] tags;
        [SerializeField] private bool isIgnoreTag = false;

        [Header(" [ Layers ] ")]
        [SerializeField] private LayerMask layers;
        [SerializeField] private bool isIgnoreLayer = true;

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
        private Coroutine updateHandler;
        private List<Collider> stayedColliders;

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

        private IEnumerator UpdateTrigger()
        {
            while (useTriggerStay)
            {
                TriggerStay();

                yield return null;
            }

            yield break;
        }

        protected bool CheckIgnoreTag(Collider other)
        {
            if (tags.Contains(other.tag))
                return !isIgnoreTag;
            else
                return isIgnoreTag;
        }
        protected bool CheckIgnoreLayer(Collider other)
        {
            if(other.gameObject.ContainLayer(layers))
                return !isIgnoreLayer;
            else 
                return isIgnoreLayer;
        }
        private void OnTriggerEnter(Collider other)
        {
            if ((!useTriggerEnter || (isOnceEnter && wasEnterTrigger)) && !useTriggerStay)
                return;
            
            if (!CheckIgnoreTag(other))
                return;

            if (!CheckIgnoreLayer(other))
                return;

            if (useTriggerEnter)
            {
                wasEnterTrigger = true;

                TriggerEnter(other);
            }

            if (useTriggerStay && !stayedColliders.Contains(other))
            {
                stayedColliders.Add(other);

                if (stayedColliders.Count == 1)
                    updateHandler = StartCoroutine(UpdateTrigger());

            }

            DrawSphere(other.bounds.center, Color.red);
        }

        private void OnTriggerExit(Collider other)
        {
            if ((!useTriggerExit || (isOnceExit && wasExitTrigger)) && !useTriggerStay)
                return;

            if (!CheckIgnoreTag(other))
                return;

            if (!CheckIgnoreLayer(other))
                return;

            if (useTriggerExit)
            {
                wasExitTrigger = true;

                TriggerExit(other);
            }

            if (useTriggerStay && stayedColliders.Contains(other))
            {
                stayedColliders.Remove(other);

                if (updateHandler is not null)
                {
                    StopCoroutine(updateHandler);
                    updateHandler = null;
                }
            }

            DrawSphere(other.bounds.center, Color.red);
        }

        #region Callback
        private void TriggerEnter(Collider other)
        {
            Log($"On Trigger Enter - [ {other.name} ]");

            OnEnteredTrigger?.Invoke(other);
        }
        private void TriggerExit(Collider other)
        {
            Log($"On Trigger Exit - [ {other.name} ]");

            OnExitedTrigger?.Invoke(other);
        }
        private void TriggerStay()
        {
            if (stayedColliders.Count <= 0)
                return;

            Log($"On Trigger Stay - [ {stayedColliders.Count} ]");

            OnStayedTrigger?.Invoke(stayedColliders);
        }
        #endregion
    }
}