using UnityEngine;
using System.Collections;
using UnityEngine.Events;


namespace StudioScor.Utilities
{

    public class SimplePooledObject : BaseMonoBehaviour
    {
        [Header(" [ Simple Pooled Object ] ")]
        [SerializeField] private bool releasedWhenDisable = true;
        [Space(5f)]
        [SerializeField] private UnityEvent onCreated;
        [SerializeField] private UnityEvent onReleased;

        private SimplePool pool;
        private bool isActivate = false;

        public bool IsActivate => isActivate;

        public event UnityAction OnCreated;
        public event UnityAction OnReleased;
        
        private void OnDisable()
        {
#if UNITY_EDITOR
            if (!gameObject.scene.isLoaded)
                return;
#endif
            if (pool is null)
                return;

            if (releasedWhenDisable)
            {
                if (transform.parent != pool.Container)
                {
                    if (gameObject.activeSelf)
                        gameObject.SetActive(false);

                    CoroutineManager.Instance.StartCoroutine(DeleayedRelease());
                }
                else
                    Release();
            }
        }

        public void Create(SimplePool poolContainer)
        {
            Log(" Create ");

            if (poolContainer is not null)
            {
                pool = poolContainer;

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
            isActivate = true;
        }

        private IEnumerator DeleayedRelease()
        {
            yield return null;

            Release();
        }

        public void Release()
        {
            if (!isActivate)
                return;

            isActivate = false;
            
            Log(" Release ");

            if (pool is not null)
            {
                pool.Release(this);

                Callback_OnReleased();
            }
            else
            {
                Log(" Pool Container Is Null!");
            }
        }

        public void ResetParent()
        {
            SetParent(pool.Container);
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
            onCreated?.Invoke();
            OnCreated?.Invoke();
        }
        protected virtual void Callback_OnReleased()
        {
            onReleased?.Invoke();
            OnReleased?.Invoke();
        }
    }
    
}