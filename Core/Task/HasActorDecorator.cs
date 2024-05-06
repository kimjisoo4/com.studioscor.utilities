using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class HasActorDecorator : TaskActionDecorator
    {
        protected override bool PerformConditionCheck(GameObject target)
        {
            return target.TryGetActor(out _);
        }
    }
}
