using UnityEngine;
using System;

namespace StudioScor.Utilities
{
    [Serializable]
    public class LocalPositionVariable : PositionVariable
    {
        [Header(" [ Local Position Variable ] ")]
        [SerializeField] private Vector3 _position;

        private LocalPositionVariable _original;

        public LocalPositionVariable() { }
        public LocalPositionVariable(Vector3 position)
        {
            _position = position;
        }

        public override IPositionVariable Clone()
        {
            var clone = new LocalPositionVariable();

            clone._original = this;

            return clone;
        }

        public override Vector3 GetValue()
        {
            Vector3 position = _original is not null ? _original._position : _position;

            return Owner.transform.TransformPoint(position);
        }
    }
}
