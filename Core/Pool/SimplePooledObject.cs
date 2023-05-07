using UnityEngine;
using System.Collections;
using UnityEngine.Events;


namespace StudioScor.Utilities
{

    public class SimplePooledObject : BaseMonoBehaviour
    {
        [Header(" [ Simple Pooled Object ] ")]
        [SerializeField] private bool _ReleasedWhenDisable = true;
        [Space(5f)]
        [SerializeField] private UnityEvent _OnCreated;
        [SerializeField] private UnityEvent _OnReleased;

        public UnityAction OnCreated;
        public UnityAction OnReleased;

        private SimplePool _Pool;

        private bool _IsActivate = false;

        
        private void OnDisable()
        {
#if UNITY_EDITOR
            if (!gameObject.scene.isLoaded)
                return;
#endif
            if (_ReleasedWhenDisable)
            {
                if (transform.parent != _Pool.Container)
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
                _Pool = poolContainer;

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
            _IsActivate = true;
        }

        private IEnumerator DeleayedRelease()
        {
            yield return null;

            Release();
        }

        public void Release()
        {
            if (!_IsActivate)
                return;

            _IsActivate = false;
            
            Log(" Release ");

            if (_Pool is not null)
            {
                _Pool.Release(this);

                Callback_OnReleased();
            }
            else
            {
                Log(" Pool Container Is Null!");
            }
        }

        public void ResetParent()
        {
            SetParent(_Pool.Container);
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

        private void Callback_OnCreated()
        {
            _OnCreated?.Invoke();
            OnCreated?.Invoke();
        }
        private void Callback_OnReleased()
        {
            _OnReleased?.Invoke();
            OnReleased?.Invoke();
        }
    }
    
}