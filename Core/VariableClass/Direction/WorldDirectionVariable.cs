using System;
using UnityEngine;

namespace StudioScor.Utilities
{

    [Serializable]
    public class WorldDirectionVariable : DirectionVariable
    {
        [Header(" [ Input Direction Module ] ")]
        [SerializeField] private Vector3 _direction = Vector3.forward;

        private WorldDirectionVariable _original = null;

        public override IVariable Clone()
        {
            var clone = new WorldDirectionVariable();

            clone._original = this; ;

            return clone;
        }

        public override Vector3 GetDirection()
        {
            Vector3 direction = _original is not null ? _original._direction : _direction;

            return direction;
        }
    }

}
