using UnityEngine;
using UnityEngine.Animations;

namespace StudioScor.Utilities
{
    public class LookAtCamera : BaseStateMono
    {
        [Header(" [ Look At Camera Component ] ")]
        [SerializeField] private RotationConstraint constraint;
        [SerializeField] private Camera targetCamera;

        protected override void Reset()
        {
#if UNITY_EDITOR
            base.Reset();

            if(gameObject.TryGetComponentInParentOrChildren(out constraint))
            {
                constraint.constraintActive = true;
            }
#endif
        }

        private void OnEnable()
        {
            TryEnterState();
        }

        private void OnDisable()
        {
            TryExitState();
        }

        protected override void EnterState()
        {
            base.EnterState();

            EnterLookAtCamera();
        }

        protected override void ExitState()
        {
            base.ExitState();

            ExitLookAtCamera();
        }

        [ContextMenu("On Look At Camera", false, 1000000)]
        public void OnLookAtCamera()
        {
            ForceEnterState();
        }
        [ContextMenu("End Look At Camera", false, 1000000)]
        public void EndLookAtCamera()
        {
            ForceExitState();
        }

        protected virtual void EnterLookAtCamera()
        {
            if (!targetCamera)
                targetCamera = Camera.main;

            ConstraintSource source = new()
            {
                sourceTransform = targetCamera.transform,
                weight = 1f,
            };

            if (constraint.sourceCount > 0)
            {
                constraint.SetSource(0, source);
            }
            else
            {
                constraint.AddSource(source);
            }
        }

        protected virtual void ExitLookAtCamera()
        {
            if (constraint.sourceCount > 0)
            {
                constraint.RemoveSource(0);
            }
        }
    }

}
