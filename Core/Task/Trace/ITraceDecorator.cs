using System.Linq;
using UnityEngine;

namespace StudioScor.Utilities
{
    public interface ITraceDecorator
    {
        public GameObject Owner { get; }
        public void Setup(GameObject owner);
        public ITraceDecorator Clone();
        public int CheckCondition(int hitCount, ref RaycastHit[] hitResults);
    }

    public abstract class TraceDecorator : ITraceDecorator
    {
        [SerializeField] protected bool _isInverseResult = false;
        public GameObject Owner { get; protected set; }

        protected TraceDecorator _original;

        public virtual void Setup(GameObject owner)
        {
            Owner = owner;
        }

        public virtual ITraceDecorator Clone()
        {
            var clone = OnClone() as TraceDecorator;

            clone._original = this;

            return clone;
        }
        public abstract ITraceDecorator OnClone();
        public int CheckCondition(int hitCount, ref RaycastHit[] hitResults)
        {
            var newHitResults = new RaycastHit[hitCount];
            int newHitCount = 0;

            for (int i = 0; i < hitCount; i++)
            {
                if (PerformConditionCheck(hitResults.ElementAt(i)))
                {
                    newHitResults[newHitCount] = hitResults[i];
                    newHitCount++;
                }
            }

            hitResults = newHitResults;
            return newHitCount;
        }

        protected virtual bool PerformConditionCheck(RaycastHit hitResult)
        {
            return true;
        }
    }

}
