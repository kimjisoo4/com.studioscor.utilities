#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

using StudioScor.Utilities.VisualScripting;

namespace StudioScor.Utilities.Editor.VisualScripting
{
    [Descriptor(typeof(ValueReroute))]
    public sealed class ValueRerouteDescriptor : UnitDescriptor<ValueReroute>
    {
        public ValueRerouteDescriptor(ValueReroute target) : base(target)
        {
        }

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);

            description.showLabel = false;
        }

        protected override EditorTexture DefaultIcon()
        {
            return UtilitiesPathUtilityWithVisualScripting.Load("value_reroute");
        }

        protected override EditorTexture DefinedIcon()
        {
            return UtilitiesPathUtilityWithVisualScripting.Load("value_reroute");
        }
    }
}
#endif