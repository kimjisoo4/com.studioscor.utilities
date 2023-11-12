using UnityEngine;

namespace StudioScor.Utilities
{
    public class ActorRerouteComponent : BaseMonoBehaviour, IRerouteActor
    {
        [Header(" [ Actor Reroute Component ] ")]
        [SerializeField] private GameObject _actor;
        public IActor Actor { get; private set; }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (gameObject.TryGetComponentInParent(out IActor actor))
            {
                _actor = actor.gameObject;
            }
#endif
        }

        private void Awake()
        {
            if(!_actor)
            {
                Actor = GetComponentInParent<IActor>();
                _actor = Actor.gameObject;
            }
            else
            {
                Actor = _actor.GetComponent<IActor>();
            }
        }
    }
}