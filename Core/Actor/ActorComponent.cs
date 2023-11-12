namespace StudioScor.Utilities
{
    public class ActorComponent : BaseMonoBehaviour, IActor, IRerouteActor
    {
        public IActor Actor => this;
    }
}