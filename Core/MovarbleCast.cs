using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class MovarbleCast : BaseMonoBehaviour
    {
        [Header(" [ Movarble ] ")]
        [SerializeField] private float radius;
        [SerializeField] private LayerMask layerMask;
        [Space(5f)]
        [SerializeField] private bool isAutoPlaying = true;
        [Space(5f)]
        public UnityEvent<Vector3> OnMovedHit;

        private bool isPlaying;
        private Vector3 prevPosition;
        private List<RaycastHit> raycastHits;
        private List<Collider> colliderHits;
        private List<Transform> ignoreTransform;

        private void OnDrawGizmos()
        {
            if (!UseDebug)
                return;

            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawSphere(transform.position, radius);
        }
        private void OnDrawGizmosSelected()
        {
            if (!UseDebug)
                return;

            Gizmos.color = new Color(1, 0, 0, 0.8f);
            Gizmos.DrawSphere(transform.position, radius);
        }

        private void Awake()
        {
            ignoreTransform = new();
            raycastHits = new();
            colliderHits = new();
        }

        private void OnEnable()
        {
            if (isAutoPlaying)
            {
                OnMovableCast();
            }
        }
        private void OnDisable()
        {
            if (isAutoPlaying)
            {
                EndMovableCast();
            }
        }
        public void OnMovableCast()
        {
            isPlaying = true;

            ignoreTransform.Add(transform);

            prevPosition = transform.position;
        }
        public void EndMovableCast()
        {
            isPlaying = false;
        }
        public void ResetMovarbleCast()
        {
            ignoreTransform.Clear();
            isPlaying = false;
        }
        public void AddIgnoreTransform(Transform transform)
        {
            ignoreTransform.Add(transform);
        }

        private void FixedUpdate()
        {
            if (!isPlaying)
                return;

            Vector3 position = transform.position;

            if (prevPosition == transform.position)
            {
                colliderHits.Clear();

                if (SUtility.Physics.DrawOverlapSphere(position, radius, layerMask, ref colliderHits, ignoreTransform, UseDebug))
                {
                    OnMoveHit(position);
                }
            }
            else
            {
                raycastHits.Clear();

                if (SUtility.Physics.DrawSphereCastAll(prevPosition, position, radius, layerMask, ref raycastHits, ignoreTransform, UseDebug))
                {
                    OnMoveHit(raycastHits[0].point);
                }
            }


            prevPosition = position;
        }

        private void OnMoveHit(Vector3 position)
        {
            Log("On Moved Hit");

            OnMovedHit?.Invoke(position);
        }
    }
}