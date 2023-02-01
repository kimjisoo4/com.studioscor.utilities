using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/new SceneContainer", fileName = "SceneContainer_")]
    public class SceneContainer : BaseScriptableObject
    {
        [field: SerializeField] public string[] Scenes { get; private set; }

        public void Test()
        {
            SceneLoadManager.Instance.ForceLoadScene(this);
        }
    }
}
