using UnityEngine;
using UnityEngine.Animations;

using StudioScor.Utilities;

namespace CuBattle
{
    public class SetTargetToConstraint<T> : BaseMonoBehaviour where T : IConstraint
    {
        [Header(" [ Set Constraints Target] ")]
        [SerializeField] private T _Constraint;
        public void SetTarget(GameObject target)
        {
            SetTarget(target.transform);
        }

        public void SetTarget(Transform target)
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
