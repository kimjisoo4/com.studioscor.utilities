using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class WorldRotationVariable : RotationVariable
    {
        [Header(" [ World Rotation Variable ] ")]
        [SerializeField] private Vector3 _rotation;

        private WorldRotationVariable _original;

        public override IRotationVariable Clone()
        {
            var clone = new WorldRotationVariable();

            clone._original = this;

            return clone;
        }

        public override Quaternion GetValue()
        {
            var eulurAngle = _original is null ? _rotation : _original._rotation;

            return Quaternion.Euler(eulurAngle);
        }
    }

}
