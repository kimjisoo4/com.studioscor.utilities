using UnityEngine;
using StudioScor.Utilities;

namespace StudioScor.BodySystem
{
    public class BodyPartComponent : BaseMonoBehaviour
    {
        [Header(" [ Body Part Component ] ")]
        [SerializeField] private BodySystem _BodySystem;
        [SerializeField] private Body _Body;

        public BodySystem BodySystem
        {
            get
            {
                if(_BodySystem == null)
                {
                    _BodySystem = GetComponentInParent<BodySystem>();
                }

                return _BodySystem;
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _BodySystem = GetComponentInParent<BodySystem>();
        }
#endif
        private void Awake()
        {
            if(_BodySystem == null)
                _BodySystem = GetComponentInParent<BodySystem>();
        }

        private void OnEnable()
        {
            BodySystem.TryAddBodyPart(_Body, this);
        }
        private void OnDisable()
        {
            BodySystem.TryRemoveBodyPart(_Body);
        }
    }
}
