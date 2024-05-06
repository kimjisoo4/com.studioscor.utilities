using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class CheckSelfActorDecorator : TaskActionDecorator
    {
        protected override bool PerformConditionCheck(GameObject target)
        {
            return Owner == target;
        }
    }
}
