using UnityEngine;
using UnityEngine.Animations;

using StudioScor.Utilities;

namespace CuBattle
{
    public class SetTargetToConstraint<T> : BaseMonoBehaviour where T : IConstraint
    {
        [Header(" [ Set Constraints Target] ")]
        [SerializeField] private T _Constraint;

        private void Reset()
        {
            gameObject.TryGetComponentInParentOrChildren(out _Constraint);
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
            int sourceCount = _Constraint.sourceCount;

            if (sourceCount > 1)
            {
                for(int i = 0; i < sourceCount; i++)
                {
                    _Constraint.RemoveSource(0);
                }
            }

            if (!target)
            {
                if(sourceCount == 1)
                {
                    _Constraint.RemoveSource(0);
                }

                return;
            }

            var source = new ConstraintSource();

            source.weight = 1f;
            source.sourceTransform = target;

            if (sourceCount == 0)
                _Constraint.AddSource(source);
            else
                _Constraint.SetSource(0, source);
        }
    }
}
