using System.Linq;
using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/SceneLoadSystem/new Scene Reference", fileName = "Scene_", order = 10)]
    public class SceneReferenceSO : BaseScriptableObject
    {
        [Header(" [ Scene Data ] ")]
        [SerializeField] private string _sceneName;
#if UNITY_EDITOR
        [SerializeField] private Object _scene;
#endif
        public string SceneName => _sceneName;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!_scene)
                return;

            if (_scene is not UnityEditor.SceneAsset)
            {
                Debug.LogWarning($"{_scene.name} 는 {nameof(UnityEditor.SceneAsset)}이 아닙니다.", this);
                _scene = null;
                return;
            }

            if (!UnityEditor.EditorBuildSettings.scenes.Any(s => s.path.Contains(_scene.name)))
            {
                Debug.LogWarning($"{_scene.name} 씬이 Build Settings에 포함되어 있지 않습니다.", _scene);
            }

            if (_sceneName != _scene.name)
            {
                _sceneName = _scene.name;
            }
#endif
        }
    }
}
