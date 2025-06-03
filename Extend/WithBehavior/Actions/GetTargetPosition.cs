#if SCOR_ENABLE_BEHAVIOR
using System;
using Unity.Behavior;
using UnityEngine;

namespace StudioScor.Utilities.UnityBehavior
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [NodeDescription(name: "Get Target Position", story: "Set [Variable] to [Target] position.", category: "Action/StudioScor/Utilities", id: "utilities_gettargetposition")]
    public class GetTargetPosition : BaseAction
    {
        [Header(" [ Get Random Position ] ")]
        [SerializeReference] public BlackboardVariable<GameObject> Target;
        [SerializeReference] public BlackboardVariable<Vector3> Variable;

        protected override Status OnStart()
        {
            if (base.OnStart() == Status.Failure)
                return Status.Failure;

            if (!Target.Value)
                return Status.Failure;

            Variable.Value = Target.Value.transform.position;

            return Status.Success;
        }
    }
}
#endif