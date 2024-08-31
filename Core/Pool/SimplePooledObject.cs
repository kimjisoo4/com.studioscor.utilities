using UnityEngine;
using System.Collections;
using UnityEngine.Events;


namespace StudioScor.Utilities
{
    public class SimplePooledObject : BaseMonoBehaviour
    {
        [Header(" [ Simple Pooled Object ] ")]
        [SerializeField] SimplePoolContainer _poolContainer;
        [SerializeField] private bool _releasedWhenDisable = true;
        [Space(5f)]
        [SerializeField] private UnityEvent _onCreated;
        [SerializeField] private UnityEvent _onReleased;

        private SimplePool _pool;
        private bool _isCreating;

        public event UnityAction OnCreated;
        public event UnityAction OnReleased;

        private void OnDisable()
        {
            if (!_releasedWhenDisable)
                return;

            if (_isCreating)
                return;

            if (IsApplicationQuit)
                return;

            if (_pool is null && !_poolContainer)
            {
                return;
            }

            if (gameObject.activeSelf)
                gameObject.SetActive(false);

            if(gameObject && gameObject.scene.isLoaded)
                CoroutineManager.Instance.StartCoroutine(DeleayedRelease(gameObject));
        }

        public void Create(SimplePool poolContainer)
        {
            _isCreating = true;

            Log(" Create ");

            if (poolContainer is not null)
            {
                _pool = poolContainer;

                Invoke_OnCreated();
            }
            else
            {
                Log(" Pool Container Is Null!");
            }

            gameObject.SetActive(false);

            _isCreating = false;
        }

        private IEnumerator DeleayedRelease(GameObject gameObject)
        {
            yield return null;

            if (!gameObject)
                yield break;

            Release();
        }

        public void Release()
        {
            if (_isCreating)
                return;

            Log(" Release ");

            if (_pool is not null)
            {
                _pool.Release(this);

                Invoke_OnReleased();
            }
            else if (_poolContainer)
            {
                _poolContainer.Release(this);

                Invoke_OnReleased();
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

        protected virtual void Invoke_OnCreated()
        {
            Log($"{nameof(OnCreated)}");
            
            _onCreated?.Invoke();
            OnCreated?.Invoke();
        }
        protected virtual void Invoke_OnReleased()
        {
            Log($"{nameof(OnReleased)}");

            _onReleased?.Invoke();
            OnReleased?.Invoke();
        }
    }
    
}