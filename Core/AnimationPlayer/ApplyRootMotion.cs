using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IApplyRootMotion
    {
        public Vector3 DeltaPosition { get; }
        public Quaternion DeltaRotation { get; }
        public Animator Animator { get; }
        public bool UseRootMotion { get; }
        public void SetUseRootMotion(bool useRootMotion);

    }
    public class ApplyRootMotion : BaseMonoBehaviour, IApplyRootMotion
    {
        [Header(" [ Apply Root Motion ] ")]
        [SerializeField] private Animator animator;
        [SerializeField] private bool useRootMotion = true;

        public Animator Animator => animator;
        public bool UseRootMotion => useRootMotion;

        [field: SerializeField][field: SReadOnly] public Vector3 DeltaPosition { get; protected set; }
        [field: SerializeField][field: SReadOnly] public Quaternion DeltaRotation { get; protected set; }

        protected virtual void Reset()
        {
#if UNITY_EDITOR
            animator = GetComponent<Animator>();
#endif
        }
        protected virtual void Awake()
        {
            if (!animator)
                animator = GetComponent<Animator>();
        }

        public void SetUseRootMotion(bool useRootMotion)
        {
            if (this.useRootMotion == useRootMotion)
                return;

            this.useRootMotion = useRootMotion;

            if(!useRootMotion)
            {
                DeltaPosition = Vector3.zero;
                DeltaRotation = Quaternion.identity;
            }

            Log($"On Changed RootMotion State : {useRootMotion}", false, useRootMotion ? SUtility.NAME_COLOR_GREEN : SUtility.NAME_COLOR_GRAY);
        }

        private void OnAnimatorMove()
        {
            if (!UseRootMotion)
                return;

            DeltaPosition = animator.deltaPosition;
            DeltaRotation = animator.deltaRotation;

            UpdateRootMotion();
        }
        protected virtual void UpdateRootMotion()
        { }
    }
}