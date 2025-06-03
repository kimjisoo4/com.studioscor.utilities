using StudioScor.Utilities.Netcode.GameObjects.VisualScripting;
using Unity.Netcode;
using Unity.VisualScripting;

[UnitTitle("Trigger Custom Rpc Event")]
[UnitShortTitle("TriggerCustomRpcEvent")]
[TypeIcon(typeof(CustomEvent))]
[UnitCategory("Events\\StudioScor\\NetcodeForGameObject")]
public class TriggerCustomRpcEventUnit : Unit
{
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput Enter;

    [DoNotSerialize]
    [PortLabelHidden]
    [NullMeansSelf]
    public ValueInput Target;

    [DoNotSerialize]
    [PortLabelHidden]
    public ValueInput EventName { get; private set; }

    [Serialize]
    [Inspectable]
    [UnitHeaderInspectable]
    public SendTo SendTo { get; set; } = SendTo.Server;

    [DoNotSerialize]
    [PortLabelHidden]
    public ValueInput RpcParam { get; private set; }

    [DoNotSerialize]
    [PortLabelHidden]
    public ControlOutput Exit;

    protected override void Definition()
    {
        Enter = ControlInput(nameof(Enter), OnEnter);
        Exit = ControlOutput(nameof(Exit));

        Target = ValueInput<NetworkObject>(nameof(Target), null).NullMeansSelf();
        EventName = ValueInput<string>(nameof(EventName), null);

        if(SendTo == SendTo.SpecifiedInParams)
        {
            RpcParam = ValueInput<RpcParams>(nameof(RpcParam));
        }
    }

    private ControlOutput OnEnter(Flow flow)
    {
        var target = flow.GetValue<NetworkObject>(Target);

        if(target.gameObject.TryGetComponent(out NetworkMessageListener networkMessageListener))
        {
            var eventName = flow.GetValue<string>(EventName);

            if (SendTo != SendTo.SpecifiedInParams)
            {
                networkMessageListener.TriggerRpcEvent(eventName, SendTo);
            }
            else
            {
                var rpcParam = flow.GetValue<RpcParams>(RpcParam);

                networkMessageListener.TriggerRpcEvent(eventName, SendTo, rpcParam);
            }
        }

        return Exit;
    }
}
