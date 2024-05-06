using UnityEngine;

namespace StudioScor.Utilities
{
    public interface ITaskActionDecorator
    {
        public GameObject Owner { get; }
        public ITaskActionDecorator Clone();

        public void Setup(GameObject owner);
        public bool CheckCondition(GameObject target);
    }

    public class TaskActionDecorator : ITaskActionDecorator
    {
        [SerializeField] protected bool _isInverseResult = false;
        public GameObject Owner { get; protected set; }

        protected TaskActionDecorator _original;

        public virtual void Setup(GameObject owner)
        {
            Owner = owner;
        }

        public virtual ITaskActionDecorator Clone()
        {
            var clone = new TaskActionDecorator();

            clone._original = this;

            return clone;
        }

        public bool CheckCondition(GameObject target)
        {
            var isInverse = _original is null ? _isInverseResult : _original._isInverseResult;

            return isInverse ? !PerformConditionCheck(target) : PerformConditionCheck(target);
        }
        protected virtual bool PerformConditionCheck(GameObject target)
        {
            return true;
        }
    }
}
