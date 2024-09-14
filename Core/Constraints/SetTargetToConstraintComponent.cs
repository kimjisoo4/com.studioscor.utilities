using UnityEngine;
using UnityEngine.Animations;

namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/Constraint/Set Target To Constraint Component", order: 0)]
    public class SetTargetToConstraintComponent : BaseMonoBehaviour
    {
        [Header(" [ Set Target To Constraint Component] ")]
        [SerializeField] private Component targetConstraint;

        private IConstraint constraint;

        private void Reset()
        {
            if(gameObject.TryGetComponentInParentOrChildren(out constraint))
            {
                var target = constraint as Component;

                if(target is not null)
                {
                    this.targetConstraint = target;
                }
            }
        }

        private void OnValidate()
        {
            if (targetConstraint)
            {
                if(!targetConstraint.TryGetComponent(out constraint))
                {
                    targetConstraint = null;
                }
            }
        }

        private void Awake()
        {
            if (targetConstraint)
            {
                constraint = targetConstraint as IConstraint;
            }
            else
            {
                if(gameObject.TryGetComponentInParentOrChildren(out constraint))
                {
                    var target = constraint as Component;

                    if (target is not null)
                    {
                        this.targetConstraint = target;
                    }
                }
            }
        }

        public void SetTarget(Component component)
        {
            if (component)
                SetTarget(component.transform);
            else
                SetTarget();
        }
        public void SetTarget(GameObject target)
        {
            if (target)
                SetTarget(target.transform);
            else
                SetTarget();
        }

        public void SetTarget(Transform target = null)
        {
            int sourceCount = constraint.sourceCount;

            if (sourceCount > 1)
            {
                for(int i = 0; i < sourceCount; i++)
                {
                    constraint.RemoveSource(0);
                }
            }

            if (!target)
            {
                if(sourceCount == 1)
                {
                    constraint.RemoveSource(0);
                }

                return;
            }

            var source = new ConstraintSource();

            source.weight = 1f;
            source.sourceTransform = target;

            if (sourceCount == 0)
                constraint.AddSource(source);
            else
                constraint.SetSource(0, source);
        }
    }
}
