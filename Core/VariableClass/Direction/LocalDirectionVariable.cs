using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class LocalDirectionVariable : DirectionVariable
    {
        [Header(" [ Local Direction Module ] ")]
        [SerializeField] private Vector3 _direction = Vector3.forward;

        private LocalDirectionVariable _original = null;

        public LocalDirectionVariable() { }
        public LocalDirectionVariable(Vector3 direction)
        {
            _direction = direction;
        }

        public override IVariable Clone()
        {
            var clone = new LocalDirectionVariable();

            clone._original = this;

            return clone;
        }

        public override Vector3 GetDirection()
        {
            Vector3 direction = _original is not null ? _original._direction : _direction;

            return Owner.transform.TransformDirection(direction);
        }
    }

}
