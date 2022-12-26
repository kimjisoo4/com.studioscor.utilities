using UnityEngine;
using UnityEngine.AddressableAssets;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/new AddressableSceneContainer", fileName = "AddressableSceneContainer_")]
    public class AddressableSceneContainer : BaseScriptableObject
    {
        [field: SerializeField] public AssetReference[] Scenes { get; private set; }

        public void Test()
        {
            AddressableSceneLoadManager.Instance.ForceLoadScene(this);
        }
    }
}
