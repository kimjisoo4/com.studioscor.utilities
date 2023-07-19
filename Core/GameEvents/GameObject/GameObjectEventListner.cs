using UnityEngine;

namespace StudioScor.Utilities
{
    public class GameObjectEventListner : GenericGameEventListner<GameObject>
    {
        public GameObjectEventListner(GenericGameEvent<GameObject> gameEvent) : base(gameEvent)
        {
        }
    }

}
