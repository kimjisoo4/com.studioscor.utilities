using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class TaskInstanceActorOwnerVaraible : GameObjectVariable
    {
        private ITaskInstanceActor taskInstanceActor;

        public override void Setup(GameObject owner)
        {
            base.Setup(owner);

            taskInstanceActor = owner.GetComponent<ITaskInstanceActor>();
        }
        public override IGameObjectVariable Clone()
        {
            var clone = new TaskInstanceActorOwnerVaraible();

            return clone;
        }

        public override GameObject GetValue()
        {
            return taskInstanceActor.Owner;
        }
    }
}
