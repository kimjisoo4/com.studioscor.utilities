using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class ActorTransformVariable : TransformVariable
    {
        private IActor _actor;

        public override void Setup(GameObject owner)
        {
            base.Setup(owner);

            _actor = owner.GetActor();
        }
        public override ITransformVariable Clone()
        {
            return new ActorTransformVariable();
        }

        public override Transform GetValue()
        {
            return _actor.transform;
        }
    }
}
