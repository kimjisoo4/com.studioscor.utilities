using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

namespace StudioScor.Utilities
{
    public abstract class Explosion : BaseClass
    {
        #region
        public delegate void ExplosionEventHandler(Explosion explosion, IReadOnlyList<Collider> hits);
        #endregion
        public abstract Vector3 Offset { get; }
        public abstract Vector3 AngleOffset { get; }
        public abstract float Radius { get; }
        public abstract LayerMask LayerMask { get; }

        private Transform _Owner;

        protected List<Transform> _IgnoreTransforms;
        protected List<Collider> _Hits;
        protected List<Collider> _HitResults;

        public Transform Owner => _Owner;
        public IReadOnlyList<Transform> IgnoreTransforms => _IgnoreTransforms;
        public IReadOnlyList<Collider> Hits => _Hits;
        public IReadOnlyList<Collider> HitResults => _HitResults;


        public event ExplosionEventHandler OnUpdatedHits;
        public event ExplosionEventHandler OnFinishedHitResults;

        public Explosion()
        {
            _IgnoreTransforms = new();
            _Hits = new();
            _HitResults = new();
        }

        [Conditional("UNITY_EDITOR")]
        public void OnDrawGizmos(Transform transform, bool isWire = false)
        {
#if UNITY_EDITOR
            if (!UseDebug || !transform)
                return;

            Color color = Color.red;

            color.a = 0.5f;

            Gizmos.color = color;

            if(isWire)
                Gizmos.DrawWireSphere(transform.position, Radius);
            else
                Gizmos.DrawSphere(transform.position, Radius);
#endif
        }


        public void SetOwner(Transform transform)
        {
            _Owner = transform;
        }

        public void AddIgnoreTransform(Transform transform)
        {
            _IgnoreTransforms.Add(transform);
        }

        public void OnExplosion()
        {
            if (_Owner)
            {
                _IgnoreTransforms.Add(_Owner);
            }
        }

        public void EndExplosion()
        {
            _IgnoreTransforms.Clear();
            _HitResults.Clear();

            _Owner = null;
        }

        public void UpdateExplosion(Vector3 position, Quaternion rotation)
        {
            _Hits.Clear();

            if (Trace(position, rotation))
            {
                OnUpdateHits();
            }
        }

        protected virtual bool Trace(Vector3 position, Quaternion rotation)
        {
            Quaternion traceRotation = rotation * Quaternion.Euler(AngleOffset);
            Vector3 tracePosition = position + (traceRotation * Offset);

            if (Utility.Physics.DrawOverlapSphere(tracePosition, Radius, LayerMask, ref _Hits, _IgnoreTransforms, UseDebug))
            {
                _HitResults.AddRange(Hits);

                return true;
            }

            return false;
        }


        #region CallBack
        protected void OnUpdateHits()
        {
            OnUpdatedHits?.Invoke(this, Hits);
        }
        protected void OnFinishHitResults()
        {
            OnFinishedHitResults?.Invoke(this, HitResults);
        }
        #endregion
    }
}