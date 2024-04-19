using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class SelfPositionVariable : PositionVariable
    {
        public override IVariable<Vector3> Clone()
        {
            return new SelfPositionVariable();
        }

        public override Vector3 GetValue()
        {
            return Owner.transform.position;
        }
    }

}
