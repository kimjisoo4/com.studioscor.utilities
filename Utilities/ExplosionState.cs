using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace StudioScor.Utilities
{

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