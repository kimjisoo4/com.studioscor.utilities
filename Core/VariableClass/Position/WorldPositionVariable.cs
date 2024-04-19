using UnityEngine;
using System;

namespace StudioScor.Utilities
{
    [Serializable]
    public class WorldPositionVariable : PositionVariable
    {
        [Header(" [ World Position Variable ] ")]
        [SerializeField] private Vector3 _position;

        private WorldPositionVariable _original;

        public WorldPositionVariable() { }
        public WorldPositionVariable(Vector3 position)
        {
            _position = position;
        }

        public override IVariable<Vector3> Clone()
        {
            var clone = new WorldPositionVariable();

            clone._original = this;

            return clone;
        }

        public override Vector3 GetValue()
        {
            return _original is not null ? _original._position : _position;
        }
    }
}
