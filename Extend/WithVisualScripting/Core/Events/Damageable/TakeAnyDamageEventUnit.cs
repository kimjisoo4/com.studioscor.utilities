#if SCOR_ENABLE_VISUALSCRIPTING
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("Take Any Damage")]
    [UnitShortTitle("TakeAnyDamage")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\StudioScor\\Damageable")]
    public class TakeAnyDamageEventUnit : CustomInterfaceEventUnit<IDamageable, FDamageInfo>
    {
        public override Type MessageListenerType => typeof(DamageableMessageListener);
        protected override string HookName => DamageableWithVisualScripting.TAKE_ANY_DAMAGE;

        [DoNotSerialize]
        [PortLabel("Damage")]
        public ValueOutput Damage { get; private set; }

        [DoNotSerialize]
        [PortLabel("Damage Type")]
        public ValueOutput DamageType { get; private set; }

        [DoNotSerialize]
        [PortLabel("Damage Causer")]
        public ValueOutput DamageCauser { get; private set; }

        [DoNotSerialize]
        [PortLabel("Instigator")]
        public ValueOutput Instigator { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            Damage = ValueOutput<float>(nameof(Damage));
            DamageType = ValueOutput<DamageType>(nameof(DamageType));
            DamageCauser = ValueOutput<GameObject>(nameof(DamageCauser));
            Instigator = ValueOutput<GameObject>(nameof(Instigator));
        }

        protected override void AssignArguments(Flow flow, FDamageInfo damageInfo)
        {
            base.AssignArguments(flow, damageInfo);

            flow.SetValue(Damage, damageInfo.Damage);
            flow.SetValue(DamageType, damageInfo.Type);
            flow.SetValue(DamageCauser, damageInfo.Causer);
            flow.SetValue(Instigator, damageInfo.Instigator);
        }
    }
}

#endif