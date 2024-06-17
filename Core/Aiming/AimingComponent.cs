using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
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
        private bool _wasLocked;

        public bool WasLocked => _wasLocked;
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

        public event IAimingSystem.AimingSystemTargetEventHandler OnChangedTarget;
        public event IAimingSystem.AimingSystemStateEventHandler OnChangedLockedState;

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

        public void SetLockState(bool isLock)
        {
            if (isLock && Target is null)
                return;

            var prev = _wasLocked;
            _wasLocked = isLock;

            if (prev == _wasLocked)
                return;

            Invoke_OnChangedLockedState();
        }

        public override void FixedTick(float deltaTime)
        {
            if (!enabled)
                return;

            if(_wasLocked)
            {
                if(Target.CanTargeting)
                {
                    return;
                }
            }

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

        public void SetTarget(ITargeting newTarget = null)
        {
            var prevTarget = _target;
            _target = newTarget;

            if (prevTarget == _target)
                return;

            Invoke_OnChangedTarget(prevTarget);

            if (WasLocked)
            {
                if (Target is null)
                {
                    SetLockState(false);
                }
            }
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

                for(int i = 0; i < _hits.Count; i++)
                {
                    var hit = _hits[i];

                    if(hit.collider.TryGetComponent(out ITargeting targeting) && targeting.CanTargeting)
                    {
                        _aimPosition = start + direction * hit.distance;
                        SetTarget(targeting);
                        return;
                    }
                }

                _aimPosition = start + direction * _distance;
                SetTarget(null);
            }
        }
        

        protected void Invoke_OnChangedTarget(ITargeting prevTarget)
        {
            Log($"{nameof(OnChangedLockedState)} - [ {(_target is null ? "Null" : _target.Point.name)} ] ");


            if(_useUnityEvent)
            {
                Transform target = _target is null ? null : _target.Point;

                _onChangedTarget?.Invoke(target);
            }

            OnChangedTarget?.Invoke(this, _target, prevTarget);
        }

        protected void Invoke_OnChangedLockedState()
        {
            Log($"{nameof(OnChangedLockedState)} - [ {WasLocked} ] ");

            OnChangedLockedState?.Invoke(this);
        }
    }
}