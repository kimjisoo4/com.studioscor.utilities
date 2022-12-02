using UnityEngine;

namespace StudioScor.TraceSystem
{
    public class GroundCheckerComponent : MonoBehaviour
    {
        [SerializeField] private GroundChecker _GroundChecker;

        public float GroundDistance => _GroundChecker.GroundDistance;
        public Vector3 GroundPoint => _GroundChecker.GroundPoint;
        public bool IsGrounded => _GroundChecker.IsGrounded;

        public bool CheckGround()
        {
            return _GroundChecker.CheckGround();
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _GroundChecker.SetTransform(transform);
        }
        private void OnDrawGizmosSelected()
        {
            _GroundChecker.OnDrawGizmosSelected();
        }
#endif
    }
}