#if SCOR_ENABLE_VISUALSCRIPTING
using UnityEngine;
using Unity.VisualScripting;

namespace StudioScor.Utilities.VisualScripting.Editor.Community
{
    public sealed class RerouteProcess : GraphProcess<FlowGraph, FlowCanvas>
    {
        public override void Process(FlowGraph graph, FlowCanvas canvas)
        {
            if (FuzzyWindow.instance == null && canvas.isCreatingConnection)
            {
                var canSpawn = @event != null && @event.keyCode == KeyCode.Space && @event.rawType == EventType.KeyUp;
                if (!canSpawn) canSpawn = @event != null && @event.keyCode == KeyCode.Space && @event.rawType == EventType.KeyUp;
                if (canSpawn)
                {
                    var connnectionType = canvas.connectionSource.GetType();
                    if (connnectionType == typeof(ControlOutput))
                    {
                        var reroute = new FlowReroute();
                        canvas.AddUnit(reroute, canvas.connectionEnd + new Vector2(8, 0));
                        canvas.connectionSource.ValidlyConnectTo(reroute.Enter);
                        canvas.connectionSource = reroute.Exit;
                        return;
                    }

                    if (connnectionType == typeof(ControlInput))
                    {
                        var reroute = new FlowReroute();
                        canvas.AddUnit(reroute, canvas.connectionEnd + new Vector2(-8, 0));
                        canvas.connectionSource.ValidlyConnectTo(reroute.Exit);
                        canvas.connectionSource = reroute.Enter;
                        return;
                    }

                    if (connnectionType == typeof(ValueOutput))
                    {
                        var reroute = new ValueReroute();
                        canvas.AddUnit(reroute, canvas.connectionEnd + new Vector2(8, 0));
                        canvas.connectionSource.ValidlyConnectTo(reroute.Enter);
                        canvas.connectionSource = reroute.Exit;
                        return;
                    }

                    if (connnectionType == typeof(ValueInput))
                    {
                        var reroute = new ValueReroute();
                        canvas.AddUnit(reroute, canvas.connectionEnd + new Vector2(-8, 0));
                        canvas.connectionSource.ValidlyConnectTo(reroute.Exit);
                        canvas.connectionSource = reroute.Enter;
                        return;
                    }
                }
            }
        }
    }
}
#endif