using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System.Linq;
namespace StudioScor.Utilities
{

    [AddComponentMenu("StudioScor/Utilities/Aiming/Aiming Component", order: 0)]
    public class AimingComponent : BaseMonoBehaviour, IAimingSystem
    {
        public enum EAimingOrigin
        {
            Screen,
            Transform,
        }

        [Header(" [ Aiming Component ] ")]
        [SerializeField] private EAimingOrigin _aimingOrigin = EAimingOrigin.Screen;
        [SerializeField][SEnumCondition(nameof(_aimingOrigin), (int)EAimingOrigin.Transform)] private Transform _originTransform;
        [SerializeField][SEnumCondition(nameof(_aimingOrigin), (int)EAimingOrigin.Transform)] private Vector3 _offset;
        [SerializeField][SEnumCondition(nameof(_aimingOrigin), (int)EAimingOrigin.Screen)] private Camera _camera;
        [SerializeField] private float _distance = 10f;
        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _layer;
        [SerializeField] private IgnoreTrace[] _ignoreTraces;

        [Header(" Events ")]
        [SerializeField] private bool _useUnityEvent = false;
        [SerializeField] private UnityEvent<Transform> _onChangedTarget;


        private List<RaycastHit> _hits = new();
        private readonly List<Transform> _ignoreTransforms = new();

        private Vector3 _aimPosition;
        private ITargeting _target;
        private Transform _prevHitTransform;

        public Vector3 AimPosition
        {
            get
            {
                if (_target is null)
                    return _aimPosition;
                else
                    return _target.Point.position;
            }
        }

        public ITargeting Target => _target;

        public event IAimingSystem.AimingSystemEventHandler OnChangedTarget;

        private void Reset()
        {
#if UNITY_EDITOR
            _camera = Camera.main;
#endif
        }

        void Start()
        {
            if (!_camera)
                _camera = Camera.main;
        }


        #region Ignore Transforms
        public void AddIgnoreTarget(Component component)
        {
            AddIgnoreTarget(component.transform);
        }
        public void AddIgnoreTarget(GameObject target)
        {
            AddIgnoreTarget(target.transform);
        }
        public void AddIgnoreTarget(Transform add)
        {
            Log($" Add Ignore Transform {add.gameObject.name}", SUtility.NAME_COLOR_GREEN);

            _ignoreTransforms.Add(add);
        }
        public void AddIgnoreTarget(IEnumerable<Transform> add)
        {
            _ignoreTransforms.AddRange(add);
        }

        public void RemoveIgnoreTarget(Component component)
        {
            RemoveIgnoreTarget(component.transform);
        }
        public void RemoveIgnoreTarget(GameObject target)
        {
            RemoveIgnoreTarget(target.transform);
        }

        public void RemoveIgnoreTarget(Transform remove)
        {
            Log($" Remove Ignore Transform {remove.gameObject.name}", SUtility.NAME_COLOR_GRAY);

            _ignoreTransforms.Remove(remove);
        }
        public void RemoveIgnoreTarget(IEnumerable<Transform> removes)
        {
            foreach (var remove in removes)
            {
                RemoveIgnoreTarget(remove);
            }
        }
        #endregion


        public void OnAiming()
        {
            if (enabled)
                return;

            enabled = true;
        }
        public void EndAiming()
        {
            if (!enabled)
                return;

            enabled = false;
        }

        public override void FixedTick(float deltaTime)
        {
            if (!enabled)
                return;

            base.FixedTick(deltaTime);

            if(!_camera)
            {
                _camera = Camera.main;
            }    

            _hits.Clear();

            switch (_aimingOrigin)
            {
                case EAimingOrigin.Screen:
                    CalcScreenAiming();
                    break;
                case EAimingOrigin.Transform:
                    CalcTransformAiming();
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

            _originTransform = origin;
        }
        #endregion

        public void SetTarget(Transform newTarget = null)
        {
            if (_prevHitTransform == newTarget)
                return;

            _prevHitTransform = newTarget;

            if (_prevHitTransform)
            {
                _prevHitTransform.TryGetComponentInParentOrChildren(out _target);
            }
            else
            {
                _target = null;
            }

            Invoke_OnChangedTarget();
        }

        private void CalcTransformAiming()
        {
            if (!_originTransform)
                return;

            Vector3 start = _originTransform.TransformPoint(_offset);
            Vector3 direction = start.Direction(_camera.transform.TransformPoint(Vector3.forward * _distance));

            OnCast(start, direction);
        }

        private void CalcScreenAiming()
        {
            Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Ray ray = _camera.ScreenPointToRay(center);

            OnCast(ray.origin, ray.direction);
        }

        private void OnCast(Vector3 start, Vector3 direction)
        {
            if (!SUtility.Physics.DrawSphereCastAll(start, direction, _distance, _radius, _layer, ref _hits, _ignoreTransforms, UseDebug))
            {
                _aimPosition = start + direction * _distance;

                SetTarget(null);
            }
            else
            {
                foreach (var ignoreTrace in _ignoreTraces)
                {
                    ignoreTrace.Ignore(transform, ref _hits);

                    if (_hits.Count == 0)
                    {
                        _aimPosition = start + direction * _distance;

                        SetTarget(null);

                        return;
                    }
                }

                SUtility.Sort.SortRaycastHitByDistance(start, ref _hits);

                var hit = _hits[0].transform;

                SetTarget(hit);

                _aimPosition = start + direction * _hits[0].distance;
            }
        }
        

        protected void Invoke_OnChangedTarget()
        {
            base.Log($"On Changed Target - [ {(_target is null ? "Null" : _target.Point.name)} ] ");

            Transform target = _target is null ? null : _target.Point;

            if(_useUnityEvent)
            {
                _onChangedTarget?.Invoke(target);
            }

            OnChangedTarget?.Invoke(this, target);
        }
    }
}