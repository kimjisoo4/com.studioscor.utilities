using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class SelfTransformVariable : TransformVariable
    {
        public override IVariable<Transform> Clone()
        {
            return new SelfTransformVariable();
        }

        public override Transform GetValue()
        {
            return Owner.transform;
        }
    }
}
