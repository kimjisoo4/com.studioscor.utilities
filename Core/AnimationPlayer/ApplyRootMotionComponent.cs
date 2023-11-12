using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public interface IApplyRootMotion
    {
        public delegate void RootMotionStateEventHandler(IApplyRootMotion applyRootMotion);
        public Vector3 DeltaPosition { get; }
        public Quaternion DeltaRotation { get; }
        public Animator Animator { get; }
        public bool ApplyRootMotion { get; }
        public void SetUseRootMotion(bool useRootMotion);

        public event RootMotionStateEventHandler OnChangedRootMotionState;
    }

    public class ApplyRootMotionComponent : BaseMonoBehaviour, IApplyRootMotion
    {
        [Header(" [ Apply Root Motion ] ")]
        [SerializeField] private Animator _animator;

        public Animator Animator => _animator;
        public bool ApplyRootMotion => _animator.applyRootMotion;
        public Vector3 DeltaPosition => _deltaPosition;
        public Quaternion DeltaRotation => _deltaRotation;

        [Header(" Unity Event ")]
        [SerializeField] private bool useUnityEvent = false;
        [SerializeField][SCondition(nameof(useUnityEvent))] private UnityEvent<bool> _onChangedRootMotionState;

        public event IApplyRootMotion.RootMotionStateEventHandler OnChangedRootMotionState;

        protected Vector3 _deltaPosition;
        protected Quaternion _deltaRotation;


        protected virtual void Reset()
        {
#if UNITY_EDITOR
            _animator = GetComponent<Animator>();
#endif
        }
        protected virtual void Awake()
        {
            if (!_animator)
                _animator = GetComponent<Animator>();
        }

        public void SetUseRootMotion(bool useRootMotion)
        {
            if (_animator.applyRootMotion == useRootMotion)
                return;

            _animator.applyRootMotion = useRootMotion;

            if(!useRootMotion)
            {
                _deltaPosition = Vector3.zero;
                _deltaRotation = Quaternion.identity;
            }

            Invoke_OnChangedRootMotionState();
        }

        private void OnAnimatorMove()
        {
            if (!ApplyRootMotion)
                return;

            _deltaPosition = _animator.deltaPosition;
            _deltaRotation = _animator.deltaRotation;

            UpdateRootMotion();
        }
        protected virtual void UpdateRootMotion()
        { }

        protected virtual void Invoke_OnChangedRootMotionState()
        {
            Log($"On Changed RootMotion State : {Animator.applyRootMotion}", Animator.applyRootMotion ? SUtility.NAME_COLOR_GREEN : SUtility.NAME_COLOR_GRAY);

            if(useUnityEvent)
            {
                _onChangedRootMotionState?.Invoke(Animator.applyRootMotion);
            }

            OnChangedRootMotionState?.Invoke(this);
        }
    }
}