using UnityEngine;

namespace StudioScor.Utilities
{
    public class ActorRerouteComponent : BaseMonoBehaviour, IRerouteActor
    {
        [Header(" [ Actor Reroute Component ] ")]
        [SerializeField] private GameObject _owner;

        private IActor _actor;
        public IActor Actor
        {   
            get
            {
                if(_actor is null)
                {
                    if (!_owner)
                    {
                        _actor = GetComponentInParent<IActor>();
                        _owner = Actor.gameObject;
                    }
                    else
                    {
                        _actor = _owner.GetComponent<IActor>();
                    }
                }
                

                return _actor;
            }
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (gameObject.TryGetComponentInParent(out IActor actor))
            {
                _owner = actor.gameObject;
            }
#endif
        }
    }
}