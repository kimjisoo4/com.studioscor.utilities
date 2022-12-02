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

        private SimplePoolContainer _PoolContainer;

        private void OnDisable()
        {
            if (_ShouldDisableOnRelease)
                Release();
        }

        public void Create(SimplePoolContainer poolContainer)
        {
            Log(" Create ");

            if (poolContainer is not null)
            {
                _PoolContainer = poolContainer;

                OnCreatedEvent?.Invoke();
            }
            else
            {
                Log(" Pool Container Is Null!");
            }
        }
        public void Release()
        {
            Log(" Release ");

            if (_PoolContainer is not null)
            {
                _PoolContainer.Release(this);

                OnReleaseEvent?.Invoke();
            }
            else
            {
                Log(" Pool Container Is Null!");
            }
            
        }
        public virtual void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
        }
    }
    
}