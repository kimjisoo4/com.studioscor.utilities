using UnityEngine;
using System.Diagnostics;

namespace StudioScor.TraceSystem
{

    [System.Serializable]
    public class GroundChecker
    {
        [SerializeField] private Transform _Transform;
        [Header(" [ Trace Shape ] ")]
        [SerializeField] private ETraceShape _TraceShape;
        [SerializeField] private float _Radius = 0.25f;
        [Header("[ Ground Layer ]")]
        [SerializeField] private LayerMask _GroundLayer;
        [SerializeField] private Vector3 _GroundOffset = new Vector3(0, 0.5f, 0);
        [SerializeField] private float _CheckDistance = 1f;

        protected RaycastHit _GroundHit;
        private bool _IsGrounded;
        private float _GroundDistance;
        private bool _IsExtend = true;

        public float GroundDistance => _GroundDistance;
        public Vector3 GroundPoint => _GroundHit.point;
        public bool IsGrounded => _IsGrounded;


        private const float EXTEND_HALF = 0.5f;
        private const float EXTEND_HALF_OFFSET = 0.01f;

        public void SetTransform(Transform transform)
        {
            _Transform = transform;
        }

        public void SetExtend(bool isExtend)
        {
            _IsExtend = isExtend;
        }

        public bool CheckGround()
        {
            _GroundHit = new();

            _IsGrounded = TryRayCast();

            _GroundDistance = _GroundHit.distance - _GroundOffset.y;

            switch (_TraceShape)
            {
                case ETraceShape.Sphere:
                    _GroundDistance += _Radius;
                    break;
                default:
                    break;
            }
            


            return _IsGrounded;
        }

        private bool TryRayCast()
        {
            Vector3 start = _Transform.position + _Transform.TransformDirection(_GroundOffset);
            Vector3 direction = -_Transform.up;
            float distacne = _IsExtend ? _CheckDistance : _CheckDistance * EXTEND_HALF + EXTEND_HALF_OFFSET;

            switch (_TraceShape)
            {
                case ETraceShape.Sphere:
                    distacne -= _Radius;

                    return Physics.SphereCast(start, _Radius, direction, out _GroundHit, distacne, _GroundLayer);
                default:
                    return Physics.Raycast(start, direction, out _GroundHit, distacne, _GroundLayer);
            }
        }

        [Conditional("UNITY_EDITOR")]
        public void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (!_Transform)
                return;

            Vector3 start = _Transform.position + _Transform.TransformDirection(_GroundOffset);
            Vector3 direction = -_Transform.up;
            float distacne = _IsExtend ? _CheckDistance : _CheckDistance * EXTEND_HALF + EXTEND_HALF_OFFSET;

            RaycastHit hit;
            bool isHit;

            switch (_TraceShape)
            {
                case ETraceShape.Sphere:
                    distacne -= _Radius;
                    isHit = Physics.SphereCast(start, _Radius, direction, out hit, distacne, _GroundLayer);
                    break;
                default:
                    isHit =  Physics.Raycast(start, direction, out hit, distacne, _GroundLayer);
                    break;
            }

            if (isHit)
            {
                Gizmos.color = Color.green;

                Gizmos.DrawRay(start, direction * hit.distance);

                switch (_TraceShape)
                {
                    case ETraceShape.Sphere:
                        Gizmos.DrawWireSphere(start + direction * hit.distance, _Radius);        
                        break;
                    default:

                        break;
                }
                Gizmos.DrawSphere(hit.point, 0.1f);

                Gizmos.color = Color.red;
                Gizmos.DrawRay(direction * hit.distance, direction * distacne);

            }
            else
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(start, direction * distacne);

                switch (_TraceShape)
                {
                    case ETraceShape.Sphere:
                        Gizmos.DrawWireSphere(start + direction * distacne, _Radius);
                        break;
                    default:

                        break;
                }
            }
#endif
        }
    }
}