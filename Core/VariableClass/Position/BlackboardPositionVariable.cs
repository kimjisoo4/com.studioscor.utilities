using UnityEngine;
using System;

namespace StudioScor.Utilities
{
    [Serializable]
    public class BlackboardPositionVariable : PositionVariable
    {
        [Header(" [ Blackboard Position Variable ] ")]
        [SerializeField] private VariableBlackboardKey _blackboardKey;

        private IVariableBlackBoard _variableBlackboard;
        private BlackboardPositionVariable _original;

        public override void Setup(GameObject owner)
        {
            base.Setup(owner);

            _variableBlackboard = owner.GetComponent<IVariableBlackBoard>();

        }
        public override IPositionVariable Clone()
        {
            var clone = new BlackboardPositionVariable();

            clone._original = this;

            return clone;
        }

        public override Vector3 GetValue()
        {
            return _variableBlackboard.GetValue<Vector3>(_original is null ? _blackboardKey : _original._blackboardKey);
        }
    }
}
