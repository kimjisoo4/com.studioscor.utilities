using UnityEngine;
using System;
using System.Linq;

namespace StudioScor.Utilities
{
    [Serializable]
    public class TraceHitPositionVariable : PositionVariable
    {
        [Header(" [ Trace Hit Position Variable ] ")]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private ITrace _trace;

        private TraceHitPositionVariable _origina;

        public override void Setup(GameObject owner)
        {
            base.Setup(owner);

            _trace.Setup(owner);
        }

        public override IPositionVariable Clone()
        {
            var clone = new TraceHitPositionVariable();

            clone._origina = this;
            clone._trace = _trace.Clone();

            return clone;
        }

        public override Vector3 GetValue()
        {
            _trace.OnTrace();
            _trace.UpdateTrace();
            _trace.EndTrace();

            return _trace.HitResults.ElementAt(0).point;
        }
    }
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

        public override IPositionVariable Clone()
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
