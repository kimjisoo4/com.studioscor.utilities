#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

using StudioScor.Utilities.VisualScripting;

namespace StudioScor.Utilities.Editor.VisualScripting
{
    [Descriptor(typeof(FlowReroute))]
    public sealed class FlowRerouteDescriptor : UnitDescriptor<FlowReroute>
    {
        public FlowRerouteDescriptor(FlowReroute target) : base(target)
        {
        }

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);

            description.showLabel = false;
        }

        protected override EditorTexture DefaultIcon()
        {
            return UtilitiesPathUtilityWithVisualScripting.Load("flow_reroute");
        }

        protected override EditorTexture DefinedIcon()
        {
            return UtilitiesPathUtilityWithVisualScripting.Load("flow_reroute");
        }
    }
}
#endif