using StudioScor.PlayerSystem.Behavior;
using System;
using Unity.Behavior;
using UnityEngine;

namespace StudioScor.Utilities.UnityBehavior
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "Check Has GameObject In Variable", 
        description: "Check target variable is not null.", 
        story: "Has GameObject In [Target] variable ( UseDebug [UseDebugKey] ) ", 
        category: "Conditions/StudioScor/Utilities", 
        id: "utilities_checkhasgameobjectinvariable")]
    public class CheckHasGameObjectInVariable : BaseCondition
    {
        [SerializeReference] public BlackboardVariable<GameObject> Target;

        public override bool IsTrue()
        {
            var target = Target.Value;

            bool result = target;

            Log($"{nameof(Target).ToBold()} is {(result ? "Not Null".ToColor(SUtility.STRING_COLOR_SUCCESS) : "Null".ToColor(SUtility.STRING_COLOR_FAIL)).ToBold()}");

            return result;
        }
    }
}