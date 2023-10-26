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
        [SerializeField] private EAimingOrigin aimingOrigin = EAimingOrigin.Screen;
        [SerializeField][SEnumCondition(nameof(aimingOrigin), (int)EAimingOrigin.Transform)] private Transform originTransform;
        [SerializeField][SEnumCondition(nameof(aimingOrigin), (int)EAimingOrigin.Transform)] private Vector3 offset;
        [SerializeField] private new Camera camera;
        [SerializeField] private float distance = 10f;
        [SerializeField] private float radius = 1f;
        [SerializeField] private LayerMask layer;
        [SerializeField] private IgnoreTrace[] ignoreTraces;

        [Header(" Auto Playing ")]
        [SerializeField] private bool autoPlaying = true;

        [Header(" Events ")]
        [SerializeField] private UnityEvent<Transform> onChangedTarget;


        private List<RaycastHit> hits = new();
        private readonly List<Transform> ignoreTransforms = new();

        private Vector3 aimPosition;
        private ITargeting target;
        private bool isPlaying = false;
        private Transform prevHitTransform;


        public bool IsPlaying => isPlaying;
        public Vector3 AimPosition => aimPosition;
        public ITargeting Target => target;

        public event IAimingSystem.AimingSystemEventHandler OnChangedTarget;

        private void Reset()
        {
#if UNITY_EDITOR
            camera = Camera.main;
#endif
        }

        private void OnEnable()
        {
            if (autoPlaying)
                OnAiming();
        }
        private void OnDisable()
        {
            EndAiming();
        }

        void Start()
        {
            if (!camera)
                camera = Camera.main;
        }

        void FixedUpdate()
        {
            if (!camera)
            {
                camera = Camera.main;

                return;
            }

            UpdateAiming();
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
            Log($" Add Ignore Transform {add.gameObject.name}", false, SUtility.NAME_COLOR_GREEN);

            ignoreTransforms.Add(add);
        }
        public void AddIgnoreTarget(IEnumerable<Transform> add)
        {
            ignoreTransforms.AddRange(add);
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
            Log($" Remove Ignore Transform {remove.gameObject.name}", false, SUtility.NAME_COLOR_GRAY);

            ignoreTransforms.Remove(remove);
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
            if (isPlaying)
                return;

            isPlaying = true;
        }
        public void EndAiming()
        {
            if (!isPlaying)
                return;

            isPlaying = false;
        }
        public void UpdateAiming()
        {
            if (!isPlaying)
                return;

            hits.Clear();

            switch (aimingOrigin)
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

            originTransform = origin;
        }
        #endregion

        public void SetTarget(Transform newTarget = null)
        {
            if (prevHitTransform == newTarget)
                return;

            prevHitTransform = newTarget;

            if (prevHitTransform)
            {
                prevHitTransform.TryGetComponentInParentOrChildren(out target);
            }
            else
            {
                target = null;
            }

            Callback_OnChangedTarget();
        }

        private void CalcTransformAiming()
        {
            if (!originTransform)
                return;

            Vector3 start = originTransform.TransformPoint(offset);
            Vector3 direction = start.Direction(camera.transform.TransformPoint(Vector3.forward * distance));

            OnCast(start, direction);
        }

        private void CalcScreenAiming()
        {
            Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Ray ray = camera.ScreenPointToRay(center);

            OnCast(ray.origin, ray.direction);
        }

        private void OnCast(Vector3 start, Vector3 direction)
        {
            if (!SUtility.Physics.DrawSphereCastAll(start, direction, distance, radius, layer, ref hits, ignoreTransforms, UseDebug))
            {
                aimPosition = start + direction * distance;

                SetTarget(null);
            }
            else
            {
                foreach (var ignoreTrace in ignoreTraces)
                {
                    ignoreTrace.Ignore(transform, ref hits);

                    if (hits.Count == 0)
                    {
                        aimPosition = start + direction * distance;

                        SetTarget(null);

                        return;
                    }
                }

                SUtility.Sort.SortRaycastHitByDistance(start, ref hits);

                var hit = hits[0].transform;

                SetTarget(hit);

                aimPosition = start + direction * hits[0].distance;
            }
        }
        

        protected void Callback_OnChangedTarget()
        {
            base.Log($"On Changed Target - [ {(this.target is null ? "Null" : this.target.Point.name)} ] ");

            Transform target = this.target is null ? null : this.target.Point;

            onChangedTarget?.Invoke(target);

            OnChangedTarget?.Invoke(this, target);
        }
    }
}