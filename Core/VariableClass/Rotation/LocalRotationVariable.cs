using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class LocalRotationVariable : RotationVariable
    {
        [Header(" [ Local Rotation Variable ] ")]
        [SerializeField] private Vector3 _offset;

        private LocalRotationVariable _original;

        public override IRotationVariable Clone()
        {
            var clone = new LocalRotationVariable();

            clone._original = this;

            return clone;
        }

        public override Quaternion GetValue()
        {
            var eulurAngle = _original is null ? _offset : _original._offset;
            var localRotation = Owner.transform.localRotation;

            return localRotation * Quaternion.Euler(eulurAngle);
        }
    }
}
