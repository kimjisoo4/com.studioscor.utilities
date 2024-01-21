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
        [SerializeField] private Animator _Animator;

        public Animator Animator => _Animator;
        public bool ApplyRootMotion => _Animator.applyRootMotion;
        public Vector3 DeltaPosition => _DeltaPosition;
        public Quaternion DeltaRotation => _DeltaRotation;

        [Header(" Unity Event ")]
        [SerializeField] private bool _UseUnityEvent = false;
        [SerializeField][SCondition(nameof(_UseUnityEvent))] private UnityEvent<bool> _OnChangedRootMotionState;

        public event IApplyRootMotion.RootMotionStateEventHandler OnChangedRootMotionState;

        protected Vector3 _DeltaPosition;
        protected Quaternion _DeltaRotation;


        protected virtual void Reset()
        {
#if UNITY_EDITOR
            _Animator = GetComponent<Animator>();
#endif
        }
        protected virtual void Awake()
        {
            if (!_Animator)
                _Animator = GetComponent<Animator>();
        }

        public void SetUseRootMotion(bool useRootMotion)
        {
            if (_Animator.applyRootMotion == useRootMotion)
                return;

            _Animator.applyRootMotion = useRootMotion;

            if(!useRootMotion)
            {
                _DeltaPosition = Vector3.zero;
                _DeltaRotation = Quaternion.identity;
            }

            Invoke_OnChangedRootMotionState();
        }

        private void OnAnimatorMove()
        {
            if (!ApplyRootMotion)
                return;

            _DeltaPosition = _Animator.deltaPosition;
            _DeltaRotation = _Animator.deltaRotation;

            UpdateRootMotion();
        }
        protected virtual void UpdateRootMotion()
        { }

        protected virtual void Invoke_OnChangedRootMotionState()
        {
            Log($"On Changed RootMotion State : {Animator.applyRootMotion}", Animator.applyRootMotion ? SUtility.NAME_COLOR_GREEN : SUtility.NAME_COLOR_GRAY);

            if(_UseUnityEvent)
            {
                _OnChangedRootMotionState?.Invoke(Animator.applyRootMotion);
            }

            OnChangedRootMotionState?.Invoke(this);
        }
    }
}