using UnityEngine;
using System.Collections;
using UnityEngine.Events;


namespace StudioScor.Utilities
{

    public class SimplePooledObject : BaseMonoBehaviour
    {
        [Header(" [ Simple Pooled Object ] ")]
        [SerializeField] private bool _releasedWhenDisable = true;
        [Space(5f)]
        [SerializeField] private UnityEvent _onCreated;
        [SerializeField] private UnityEvent _onReleased;

        private SimplePool _pool;
        private bool _isActivate = false;
        public bool IsActivate => _isActivate;

        public event UnityAction OnCreated;
        public event UnityAction OnReleased;
        
        private void OnDisable()
        {
            if (IsApplicationQuit)
                return;

            if (_pool is null)
                return;

            if (_releasedWhenDisable)
            {
                if (transform.parent != _pool.Container)
                {
                    if (gameObject.activeSelf)
                    {
                        gameObject.SetActive(false);
                    }

                    CoroutineManager.Instance.StartCoroutine(DeleayedRelease());
                }
                else
                {
                    Release();
                }
            }
        }

        public void Create(SimplePool poolContainer)
        {
            Log(" Create ");

            if (poolContainer is not null)
            {
                _pool = poolContainer;

                Callback_OnCreated();
            }
            else
            {
                Log(" Pool Container Is Null!");
            }

            gameObject.SetActive(false);
        }

        public void Activate()
        {
            _isActivate = true;
        }

        private IEnumerator DeleayedRelease()
        {
            yield return null;

            Release();
        }

        public void Release()
        {
            if (!_isActivate)
                return;

            _isActivate = false;
            
            Log(" Release ");

            if (_pool is not null)
            {
                _pool.Release(this);

                Callback_OnReleased();
            }
            else
            {
                Log(" Pool Container Is Null!");
            }
        }

        public void ResetParent()
        {
            SetParent(_pool.Container);
        }

        public virtual void SetParent(Transform parent, bool worldPositionStay = true)
        {
            transform.SetParent(parent, worldPositionStay);

            if (!worldPositionStay)
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
        public virtual void SetLocalPositionAndRotation(Vector3 localPosition, Quaternion localRotation)
        {
            transform.SetLocalPositionAndRotation(localPosition, localRotation);
        }

        protected virtual void Callback_OnCreated()
        {
            _onCreated?.Invoke();
            OnCreated?.Invoke();
        }
        protected virtual void Callback_OnReleased()
        {
            _onReleased?.Invoke();
            OnReleased?.Invoke();
        }
    }
    
}