using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class SelfPositionVariable : PositionVariable
    {
        public override IPositionVariable Clone()
        {
            return new SelfPositionVariable();
        }

        public override Vector3 GetValue()
        {
            return Owner.transform.position;
        }
    }

}
