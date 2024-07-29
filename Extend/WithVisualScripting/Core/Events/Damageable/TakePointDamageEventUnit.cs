#if SCOR_ENABLE_VISUALSCRIPTING
using UnityEngine;
using Unity.VisualScripting;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("Take Point Damage")]
    [UnitShortTitle("TakePointDamage")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\StudioScor\\Damageable")]
    public class TakePointDamageEventUnit : TakeAnyDamageEventUnit
    {
        protected override string HookName => DamageableWithVisualScripting.TAKE_POINT_DAMAGE;

        [DoNotSerialize]
        [PortLabel("Hit Point")]
        public ValueOutput HitPoint { get; private set; }

        [DoNotSerialize]
        [PortLabel("Hit Normal")]
        public ValueOutput HitNormal { get; private set; }

        [DoNotSerialize]
        [PortLabel("Hit Transform")]
        public ValueOutput HitTransform { get; private set; }

        [DoNotSerialize]
        [PortLabel("Direction")]
        public ValueOutput Direction { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            HitPoint = ValueOutput<Vector3>(nameof(HitPoint));
            HitNormal = ValueOutput<Vector3>(nameof(HitNormal));
            HitTransform = ValueOutput<Transform>(nameof(HitTransform));
            Direction = ValueOutput<Vector3>(nameof(Direction));
        }

        protected override void AssignArguments(Flow flow, DamageInfoData damageInfo)
        {
            base.AssignArguments(flow, damageInfo);

            flow.SetValue(HitPoint, damageInfo.HitPoint);
            flow.SetValue(HitNormal, damageInfo.HitNormal);
            flow.SetValue(HitTransform, damageInfo.HitCollider);
            flow.SetValue(Direction, damageInfo.Direction);
        }
    }
}

#endif