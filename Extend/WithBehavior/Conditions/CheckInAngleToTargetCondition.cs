#if SCOR_ENABLE_BEHAVIOR
using System;
using Unity.Behavior;
using UnityEngine;

namespace StudioScor.Utilities.UnityBehavior
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "Check In Angle Target", 
        description: "Check if Target is within a certain angle in front of Transform.", 
        story: "angle between [Self] and [Target] [Operator] [Angle] ¡Æ  ( UseDebug [UseDebugKey] )",
        category: "Conditions/StudioScor/Utilities",
        id: "utilities_checkinangletotarget")]
    public class CheckInAngleToTargetCondition : BaseCondition
    {
        [SerializeReference] public BlackboardVariable<Transform> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        [Comparison(comparisonType: ComparisonType.All)]
        [SerializeReference] public BlackboardVariable<ConditionOperator> Operator = new(ConditionOperator.LowerOrEqual);
        [SerializeReference] public BlackboardVariable<float> Angle = new(45f);
        [SerializeReference] public BlackboardVariable<EAxis> Axis = new(EAxis.Y);

        public override bool IsTrue()
        {
            var transform = Self.Value;

            if(!transform)
            {
                Log($"{nameof(Self).ToBold()} is Null");
                return false;
            }

            var target = Target.Value;

            if(!target)
            {
                Log($"{nameof(Target).ToBold()} is Null");
                return false;
            }

            var axisState = Axis.Value;
            var angle = Angle.Value;
            var operate = Operator.Value;

            Vector3 direction = target.position - transform.position;
            Vector3 axis;

            switch (axisState)
            {
                case EAxis.X:
                    axis = transform.right;
                    break;
                case EAxis.Y:
                    axis = transform.up;
                    break;
                case EAxis.Z:
                    axis = transform.forward;
                    break;
                default:
                    axis = transform.up;
                    break;
            }

            float signedAngle = Vector3.SignedAngle(transform.forward, direction, axis);
            bool result;

            switch (operate)
            {
                case ConditionOperator.Equal:
                    result = signedAngle.SafeEquals(angle);
                    break;
                case ConditionOperator.NotEqual:
                    result = !signedAngle.SafeEquals(angle);
                    break;
                case ConditionOperator.Greater:
                    result = !signedAngle.InRange(-angle, angle, false, false);
                    break;
                case ConditionOperator.Lower:
                    result = signedAngle.InRange(-angle, angle, false, false);
                    break;
                case ConditionOperator.GreaterOrEqual:
                    result = !signedAngle.InRange(-angle, angle);
                    break;
                case ConditionOperator.LowerOrEqual:
                    result = signedAngle.InRange(-angle, angle);
                    break;
                default:
                    result = false;
                    break;
            }

            Log($"{target.name.ToBold()} is {(result ? $"{operate}".ToColor(SUtility.STRING_COLOR_SUCCESS) : $"Not {operate}".ToColor(SUtility.STRING_COLOR_FAIL)).ToBold()} Angle");

            return result;
        }
    }
}

#endif