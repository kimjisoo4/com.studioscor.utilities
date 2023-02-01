using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class MovarbleCast : BaseMonoBehaviour
    {
        [Header(" [ Movarble ] ")]
        [SerializeField] private float _Radius;
        [SerializeField] private LayerMask _LayerMask;
        [Space(5f)]
        [SerializeField] private bool _AutoPlaying = true;
        [Space(5f)]
        public UnityEvent<Vector3> OnMovedHit;

        private bool _IsPlaying;
        private Vector3 _PrevPosition;
        private List<RaycastHit> _RaycastHits;
        private List<Collider> _ColliderHits;
        private List<Transform> _IgnoreTransform;

        private void OnDrawGizmos()
        {
            if (!UseDebug)
                return;

            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawSphere(transform.position, _Radius);
        }
        private void OnDrawGizmosSelected()
        {
            if (!UseDebug)
                return;

            Gizmos.color = new Color(1, 0, 0, 0.8f);
            Gizmos.DrawSphere(transform.position, _Radius);
        }

        private void Awake()
        {
            _IgnoreTransform = new();
            _RaycastHits = new();
            _ColliderHits = new();
        }

        private void OnEnable()
        {
            if (_AutoPlaying)
            {
                OnMovableCast();
            }
        }
        private void OnDisable()
        {
            if (_AutoPlaying)
            {
                EndMovableCast();
            }
        }
        public void OnMovableCast()
        {
            _IsPlaying = true;

            _IgnoreTransform.Add(transform);

            _PrevPosition = transform.position;
        }
        public void EndMovableCast()
        {
            _IsPlaying = false;
        }
        public void ResetMovarbleCast()
        {
            _IgnoreTransform.Clear();
            _IsPlaying = false;
        }
        public void AddIgnoreTransform(Transform transform)
        {
            _IgnoreTransform.Add(transform);
        }

        private void FixedUpdate()
        {
            if (!_IsPlaying)
                return;

            Vector3 position = transform.position;

            if (_PrevPosition == transform.position)
            {
                _ColliderHits.Clear();

                if (Utility.Physics.DrawOverlapSphere(position, _Radius, _LayerMask, ref _ColliderHits, _IgnoreTransform, UseDebug))
                {
                    OnMoveHit(position);
                }
            }
            else
            {
                _RaycastHits.Clear();

                if (Utility.Physics.DrawSphereCastAll(_PrevPosition, position, _Radius, _LayerMask, ref _RaycastHits, _IgnoreTransform, UseDebug))
                {
                    OnMoveHit(_RaycastHits[0].point);
                }
            }


            _PrevPosition = position;
        }

        private void OnMoveHit(Vector3 position)
        {
            Log("On Moved Hit");

            OnMovedHit?.Invoke(position);
        }
    }
}