using UnityEngine;
using UnityEngine.Animations;

using StudioScor.Utilities;

namespace CuBattle
{
    [AddComponentMenu("StudioScor/Utilities/Constraint/Set Target To Constraint Component", order: 0)]
    public class SetTargetToConstraintComponent : BaseMonoBehaviour
    {
        [Header(" [ Set Target To Constraint Component] ")]
        [SerializeField] private GameObject _Target;
        private IConstraint _Constraint;

        private void Reset()
        {
            if(gameObject.TryGetComponentInParentOrChildren(out _Constraint))
            {
                var target = _Constraint as Component;

                if(target is not null)
                {
                    _Target = target.gameObject;
                }
            }
        }

        private void OnValidate()
        {
            if (_Target)
            {
                if(!_Target.TryGetComponent(out _Constraint))
                {
                    _Target = null;
                }
            }
        }

        private void Awake()
        {
            if (_Target)
            {
                _Target.TryGetComponent(out _Constraint);
            }
            else
            {
                if(gameObject.TryGetComponentInParentOrChildren(out _Constraint))
                {
                    var target = _Constraint as Component;

                    if (target is not null)
                    {
                        _Target = target.gameObject;
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
