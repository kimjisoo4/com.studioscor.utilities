#if SCOR_ENABLE_BEHAVIOR
using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace StudioScor.Utilities.UnityBehavior
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [NodeDescription(name: "Get Random Position", story: "Set [Variable] to Get Random Position within an [Radius] m from the [Origin] ", category: "Action/StudioScor/Utilities", id: "utilities_getrandomposition")]
    public class GetRandomPosition : BaseAction
    {
        [Header(" [ Get Random Position ] ")]
        [SerializeReference] public BlackboardVariable<Vector3> Origin;
        [SerializeReference] public BlackboardVariable<bool> UseNavMesh = new(true);
        [SerializeReference] public BlackboardVariable<float> Radius = new(5f);
        [SerializeReference] public BlackboardVariable<float> RandRadius = new(2f);

        [Header(" Result ")]
        [SerializeReference] public BlackboardVariable<Vector3> Variable;

        protected override Status OnStart()
        {
            if (base.OnStart() == Status.Failure)
                return Status.Failure;

            Vector3 startPosition = Origin.Value;
            Vector2 randPosition = UnityEngine.Random.insideUnitCircle;
            float distance = Radius.Value;

            if (RandRadius.Value > 0)
            {
                distance += UnityEngine.Random.Range(0f, RandRadius.Value);
            }

            randPosition = randPosition * distance;

            Vector3 position = startPosition + new Vector3(randPosition.x, 0, randPosition.y);

            if (UseNavMesh)
            {
                if (NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                {
                    position = hit.position;
                }
                else
                {
                    return Status.Failure;
                }
            }

            Variable.Value = position;

            return Status.Success;
        }
    }
}
#endif