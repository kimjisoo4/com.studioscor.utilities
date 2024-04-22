using UnityEngine;
using System;

namespace StudioScor.Utilities
{
    [Serializable]
    public class TargetPositionVariable : PositionVariable
    {
        [Header(" [ Target Position Variable ] ")]
        [SerializeReference]
#if SCOR_ENABLE_SERIALIZEREFERENCE 
        [SerializeReferenceDropdown] 
#endif
        private ITransformVariable transformVariable = new SelfTransformVariable();

        public override void Setup(GameObject owner)
        {
            base.Setup(owner);

            transformVariable.Setup(owner);
        }

        public override IPositionVariable Clone()
        {
            var clone = new TargetPositionVariable();

            clone.transformVariable = transformVariable.Clone() as ITransformVariable;

            return clone;
        }

        public override Vector3 GetValue()
        {
            return transformVariable.GetValue().position;
        }
    }
}
