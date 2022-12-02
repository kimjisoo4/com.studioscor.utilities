using System.Collections.Generic;
using UnityEngine;
using StudioScor.Utilities;

namespace StudioScor.PlayerSystem
{
    public class AiFieldOfViewState : BaseStateMono
    {
        public delegate void SightHandler(AiFieldOfViewState aiFieldOfViewState, List<PawnSystem> hitPawns);

        [SerializeField] private ControllerSystem _ControllerSystem;
        [Header(" [ Setup ] ")]
        [SerializeField] private float _Interval = 0.1f;
        [SerializeField] private float _Distance;
        [SerializeField, Range(0f, 360f)] private float _Angle;
        [SerializeField] private LayerMask _LayerMask;
        [SerializeField] private EAffiliation _Affiliation = EAffiliation.Hostile;
        public ControllerSystem ControllerSystem => _ControllerSystem;
        public PawnSystem PawnSystem => ControllerSystem.Pawn;

        public event SightHandler OnSighted;

        private List<Transform> _IgnoreTransforms;
        private float _RemainInterval;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            gameObject.TryGetComponentInParentOrChildren(out _ControllerSystem);
        }
#endif
        public override bool CanEnterState()
        {
            if (!base.CanEnterState())
                return false;

            if (!PawnSystem)
                return false;

            return true;
        }

        private void OnEnable()
        {
            _IgnoreTransforms = new();
            _IgnoreTransforms.Add(PawnSystem.transform);

            _RemainInterval = 0f;
        }

        private void OnDisable()
        {
            _IgnoreTransforms = null;
        }

        private void FixedUpdate()
        {
            if (!PawnSystem)
                return;

            if (_RemainInterval >= 0f)
            {
                _RemainInterval -= Time.deltaTime;

                return;
            }

            _RemainInterval = _Interval;

            var hits = Utility.Physics.DrawConeCast(PawnSystem.transform, _Angle, _Distance, _LayerMask, _IgnoreTransforms, _UseDebug, 0.1f);

            if (hits is not null)
            {
                var affiliationTargets = new List<PawnSystem>();

                foreach (var hit in hits)
                {
                    if (hit.transform.TryGetComponent(out PawnSystem pawnSystem) && pawnSystem.IsPossessed)
                    {
                        if (ControllerSystem.CheckAffiliation(pawnSystem.Controller) == _Affiliation)
                        {
                            affiliationTargets.Add(pawnSystem);
                        }
                    }
                }

                if (affiliationTargets.Count > 0)
                {
                    OnSighted(this, affiliationTargets);
                }
            }
        }
    }
}
