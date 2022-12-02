using UnityEngine;
using System.Collections.Generic;

namespace StudioScor.Utilities
{
    public abstract class Explosion : BaseStateMono
    {
        [Header(" [ Take Damage ] ")]
        [SerializeField] private float _Radius = 1f;
        [SerializeField] private float _Duration = 0.2f;
        [SerializeField] private LayerMask _LayerMask;

        [Header(" [ Exit Action ] ")]
        [SerializeField] private EExitAction _ExitAction = EExitAction.Destroy;

        protected Transform _Owner;
        protected object _Data;
        protected List<Transform> _IgnoreTransforms;
        protected float _RemainTime;

        #region EDITOR ONLY
        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (!_UseDebug)
                return;

            Color color = Color.red;

            color.a = 0.5f;

            Gizmos.color = color;

            Gizmos.DrawSphere(transform.position, _Radius);
#endif
        }
        #endregion

        protected virtual void OnEnable()
        {
            OnExplosion();
        }
        protected virtual void OnDisable()
        {
            EndExplosion();
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
            StayExplosion();

            _RemainTime -= Time.fixedDeltaTime;

            TryExitState();
        }

        public abstract void SetupExplosion(object data);
        private void OnExplosion()
        {
            _IgnoreTransforms = new();
            
            if(_Owner)
            {
                _IgnoreTransforms.Add(_Owner);
            }
            
            _RemainTime = _Duration;
        }
        private void EndExplosion()
        {
            _IgnoreTransforms.Clear();
            _Owner = null;

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

        private void StayExplosion()
        {
            var hits = Utility.Physics.DrawOverlapSphere(transform.position, _Radius, _LayerMask, _IgnoreTransforms, _UseDebug);

            if (hits is not null)
            {
                foreach (var hit in hits)
                {
                    if (TryHitExplosion(hit))
                    {
                        _IgnoreTransforms.Add(hit.transform);
                    }
                }
            }
        }

        protected abstract bool TryHitExplosion(Collider hit);
    }
}