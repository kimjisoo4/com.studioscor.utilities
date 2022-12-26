using UnityEngine;
using UnityEngine.Events;


namespace StudioScor.Utilities
{
    public class SimplePooledObject : BaseMonoBehaviour
    {
        [Header(" [ Disable OnRelease ] ")]
        [SerializeField] private bool _ShouldDisableOnRelease = true;

        [Header(" [ Events ] ")]
        [Space(5f)]
        public UnityEvent OnCreatedEvent;
        public UnityEvent OnReleaseEvent;

        private SimplePool _Pool;

        private bool _IsActivate = false;

        private void OnDisable()
        {
#if UNITY_EDITOR
            if (!gameObject.scene.isLoaded)
                return;
#endif
            if (_ShouldDisableOnRelease && _IsActivate)
                Release();
        }

        public void Create(SimplePool poolContainer)
        {
            Log(" Create ");

            if (poolContainer is not null)
            {
                _Pool = poolContainer;

                OnCreatedEvent?.Invoke();
            }
            else
            {
                Log(" Pool Container Is Null!");
            }

            gameObject.SetActive(false);
        }

        public void Activate()
        {
            _IsActivate = true;
        }
        public void Release()
        {
            if (!_IsActivate)
                return;

            _IsActivate = false;
            
            Log(" Release ");

            if (_Pool is not null)
            {
                _Pool.Released(this);

                OnReleaseEvent?.Invoke();
            }
            else
            {
                Log(" Pool Container Is Null!");
            }
            
        }
        public virtual void SetParent(Transform parent, bool worldPositionStay = true)
        {
            transform.SetParent(parent, worldPositionStay);

            if(!worldPositionStay)
                ResetPositionAndRotation();
        }
        public virtual void ResetPositionAndRotation()
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        public virtual void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
        }
    }
    
}