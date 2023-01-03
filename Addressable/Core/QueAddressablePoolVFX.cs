#if ENABLE_ADDRESSABLES
using UnityEngine;
using StudioScor.Utilities;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace StudioScor.GameplayQueSystem
{
    [CreateAssetMenu(menuName = "StudioScor/GameplayQue/new Addressable Pool VFX", fileName = "VFX_")]
    public class QueAddressablePoolVFX : QueFX
    {
        [SerializeField] private AssetReference _AddressablePool;

        private Transform _Container;
        private SimplePool _Pool;

        public override void PlayQue(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (!_Container)
            {
                GameObject container = new GameObject(name);

                _Container = container.transform;

                if (!_AddressablePool.IsDone)
                {
                    Log(" Load Asset Async : " + _AddressablePool);

                    _AddressablePool.LoadAssetAsync<GameObject>().Completed += QueAddressablePoolVFX_Completed;

                    return;
                }
                else
                {
                    var pooledObject = (GameObject)_AddressablePool.Asset;

                    _Pool = new SimplePool(pooledObject.GetComponent<SimplePooledObject>(), _Container);
                }
            }

            var poolObject = _Pool.Get();

            poolObject.SetPositionAndRotation(position, rotation);
            poolObject.transform.localScale = scale;
            poolObject.gameObject.SetActive(true);
        }

        private void QueAddressablePoolVFX_Completed(AsyncOperationHandle<GameObject> obj)
        {
            var pooledObject = (GameObject)_AddressablePool.Asset;

            _Pool = new SimplePool(pooledObject.GetComponent<SimplePooledObject>(), _Container);
        }
    }
}
#endif