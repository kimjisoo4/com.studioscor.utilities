using UnityEngine;
using UnityEngine.Events;
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
            _IgnoreTransforms.Clear();

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

    public abstract class ExplosionState : BaseStateMono
    {
        [System.Serializable]
        public class ExplosionSpec : Explosion
        {
            [Header(" [ Explosion ] ")]
            [SerializeField] private Vector3 _Offset = Vector3.zero;
            [SerializeField] private Vector3 _AngleOffset = Vector3.zero;
            [SerializeField] private float _Radius = 1f;
            [SerializeField] private LayerMask _LayerMask;
            [SerializeField] private bool _UseDebug;
            public override float Radius => _Radius;
            public override LayerMask LayerMask => _LayerMask;
            public override bool UseDebug => _UseDebug;
            public override Vector3 Offset => _Offset;
            public override Vector3 AngleOffset => _AngleOffset;
        }

        [SerializeField] private ExplosionSpec _Explosion;

        [Header(" [ ExplosionState ] ")]
        [SerializeField] private float _Duration = 0.2f;

        [Header(" [ Hit Events ] ")]
        public UnityEvent<IReadOnlyList<Collider>> OnUpdatedHits;
        public UnityEvent<IReadOnlyList<Collider>> OnFinishedHitResults;

        [Header(" [ Exit Action ] ")]
        [SerializeField] private EExitAction _ExitAction = EExitAction.Destroy;

        protected float _RemainTime;

        #region EDITOR ONLY
        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            _Explosion.OnDrawGizmos(transform, true);
#endif
        }
        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            _Explosion.OnDrawGizmos(transform, false);
#endif
        }
        #endregion

        private void Awake()
        {
            _Explosion.OnUpdatedHits += Explosion_OnUpdatedHits;
            _Explosion.OnFinishedHitResults += Explosion_OnFinishedHitResults;
        }

        private void Explosion_OnFinishedHitResults(Explosion explosion, IReadOnlyList<Collider> hits)
        {
            OnFinishedHitResults?.Invoke(hits);
        }

        private void Explosion_OnUpdatedHits(Explosion explosion, IReadOnlyList<Collider> hits)
        {
            OnUpdatedHits?.Invoke(hits);
        }

        protected virtual void OnEnable()
        {
            _Explosion.OnExplosion();

            EnterStateAction();
        }
        protected virtual void OnDisable()
        {
            _Explosion.EndExplosion();

            ExitStateAction();
        }

        public override bool CanExitState()
        {
            if (!base.CanExitState())
                return false;

            if (_RemainTime > 0)
                return false;

            return true;
        }

        private void FixedUpdate()
        {
            _Explosion.UpdateExplosion(transform.position, transform.rotation);

            _RemainTime -= Time.fixedDeltaTime;

            TryExitState();
        }

        private void EnterStateAction()
        {
            _Explosion.AddIgnoreTransform(transform);

            _RemainTime = _Duration;
        }
        private void ExitStateAction()
        {
            switch (_ExitAction)
            {
                case EExitAction.None:
                    break;
                case EExitAction.Disable:
                    gameObject.SetActive(false);
                    break;
                case EExitAction.Destroy:
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}