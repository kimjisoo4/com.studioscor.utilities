using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System.Linq;


namespace StudioScor.Utilities
{

    [AddComponentMenu("StudioScor/Utilities/Aiming/Aiming Component", order: 0)]
    public class AimingComponent : BaseMonoBehaviour
    {
        public enum EAimingOrigin
        {
            Screen,
            Transform,
        }

        [Header(" [ Aiming Component ] ")]
        [SerializeField] private EAimingOrigin _AimingOrigin = EAimingOrigin.Screen;
        [SerializeField][SEnumCondition(nameof(_AimingOrigin), (int)EAimingOrigin.Transform)] private Transform _OriginTransform;
        [SerializeField][SEnumCondition(nameof(_AimingOrigin), (int)EAimingOrigin.Transform)] private Vector3 _Offset;
        [SerializeField] private Camera _Camera;
        [SerializeField] private float _Distance = 10f;
        [SerializeField] private float _Radius = 1f;
        [SerializeField] private LayerMask _Layer;
        [SerializeField] private TraceIgnore[] _IgnoreTraces;

        [Header(" [ Auto Playing ] ")]
        [SerializeField] private bool _AutoPlaying = true;

        [Header(" [ Events ] ")]
        [SerializeField] private UnityEvent<Transform> _OnChangedTarget;

        public event UnityAction<Transform> OnChangedTarget;

        private List<RaycastHit> _Hits = new();
        private readonly List<Transform> _IgnoreTransforms = new();

        private Vector3 _AimPosition;
        private ITargeting _Target;
        private bool _IsPlaying = false;
        private Transform _PrevHitTransform;


        public bool IsPlaying => _IsPlaying;
        public Vector3 AimPosition => _AimPosition;
        public ITargeting Target => _Target;

        private void Reset()
        {
#if UNITY_EDITOR
            _Camera = Camera.main;
            _OriginTransform = transform;
#endif
        }

        private void OnEnable()
        {
            if (_AutoPlaying)
                OnAiming();
        }
        private void OnDisable()
        {
            EndAiming();
        }

        void Start()
        {
            if (!_Camera)
                _Camera = Camera.main;
        }

        void Update()
        {
            if (!_Camera)
            {
                _Camera = Camera.main;

                return;
            }

            UpdateAiming();
        }

        #region Ignore Transforms
        public void AddIgnoreTransforms(Component component)
        {
            AddIgnoreTransforms(component.transform);
        }
        public void AddIgnoreTransforms(GameObject target)
        {
            AddIgnoreTransforms(target.transform);
        }
        public void AddIgnoreTransforms(Transform add)
        {
            _IgnoreTransforms.Add(add);
        }
        public void AddIgnoreTransforms(IEnumerable<Transform> add)
        {
            _IgnoreTransforms.AddRange(add);
        }

        public void RemoveIgnoreTransforms(Component component)
        {
            RemoveIgnoreTransforms(component.transform);
        }
        public void RemoveIgnoreTransforms(GameObject target)
        {
            RemoveIgnoreTransforms(target.transform);
        }

        public void RemoveIgnoreTransforms(Transform remove)
        {
            _IgnoreTransforms.Remove(remove);
        }
        public void RemoveIgnoreTransforms(IEnumerable<Transform> removes)
        {
            foreach (var remove in removes)
            {
                _IgnoreTransforms.Remove(remove);
            }
        }
        #endregion


        public void OnAiming()
        {
            if (_IsPlaying)
                return;

            _IsPlaying = true;
        }
        public void EndAiming()
        {
            if (!_IsPlaying)
                return;

            _IsPlaying = false;
        }
        public void UpdateAiming()
        {
            if (!_IsPlaying)
                return;

            _Hits.Clear();

            switch (_AimingOrigin)
            {
                case EAimingOrigin.Screen:
                    CalcScreenAiming();
                    break;
                case EAimingOrigin.Transform:
                    CalcTransformAiming(); ;
                    break;
                default:
                    break;
            }
        }
        #region Set Aiming Origin
        public void SetAimingOrigin(GameObject origin)
        {
            SetAimingOrigin(origin.transform);
        }
        public void SetAimingOrigin(Component component)
        {
            SetAimingOrigin(component.transform);
        }
        public void SetAimingOrigin(Transform origin)
        {
            if (!origin)
                return;

            _OriginTransform = origin;
        }
        #endregion

        public void SetTarget(Transform target = null)
        {
            if (_PrevHitTransform == target)
                return;

            _PrevHitTransform = target;

            if (_PrevHitTransform)
            {
                _PrevHitTransform.TryGetComponentInParentOrChildren(out _Target);
            }
            else
            {
                _Target = null;
            }

            Callback_OnChangedTarget();
        }

        private void CalcTransformAiming()
        {
            if (!_OriginTransform)
                return;

            Vector3 start = _OriginTransform.TransformPoint(_Offset);
            Vector3 direction = start.Direction(_Camera.transform.TransformPoint(Vector3.forward * _Distance));

            OnCast(start, direction);
        }

        private void CalcScreenAiming()
        {
            Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Ray ray = _Camera.ScreenPointToRay(center);

            OnCast(ray.origin, ray.direction);
        }

        private void OnCast(Vector3 start, Vector3 direction)
        {
            if (!SUtility.Physics.DrawSphereCastAll(start, direction, _Distance, _Radius, _Layer, ref _Hits, _IgnoreTransforms, UseDebug))
            {
                _AimPosition = start + direction * _Distance;

                SetTarget(null);
            }
            else
            {
                foreach (var ignoreTrace in _IgnoreTraces)
                {
                    ignoreTrace.Ignore(transform, ref _Hits);

                    if (_Hits.Count == 0)
                    {
                        _AimPosition = start + direction * _Distance;

                        SetTarget(null);

                        return;
                    }
                }

                SUtility.Sort.SortRaycastHitByDistance(start, ref _Hits);

                var hit = _Hits[0].transform;

                SetTarget(hit);

                _AimPosition = start + direction * _Hits[0].distance;
            }
        }
        

        protected void Callback_OnChangedTarget()
        {
            Log($"On Changed Target - [ {(_Target is null ? "Null" : _Target.Point.name)} ] ");

            Transform target = _Target is null ? null : _Target.Point;

            _OnChangedTarget?.Invoke(target);
            OnChangedTarget?.Invoke(target);
        }
    }
}