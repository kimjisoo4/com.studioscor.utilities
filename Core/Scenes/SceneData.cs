using UnityEngine;
using System.Diagnostics;
using UnityEngine.SceneManagement;


namespace StudioScor.Utilities
{
    [System.Serializable]
    public class SceneData : BaseClass
    {
        public enum ESceneInputFieldType
        {
            Name,
            Number,
            Target,
        }

        [Header(" [ Simple Scene Changer ]")]
        [SerializeField] private ESceneInputFieldType sceneFiledType = ESceneInputFieldType.Target;

        [SerializeField, SEnumCondition(nameof(sceneFiledType), (int)ESceneInputFieldType.Name)] private string sceneName;
        [SerializeField, SEnumCondition(nameof(sceneFiledType), (int)ESceneInputFieldType.Number)] private int sceneNumber;

#if UNITY_EDITOR
        [SEnumCondition(nameof(sceneFiledType), (int)ESceneInputFieldType.Target)] public Object Target;
#endif

        [SerializeField][SEnumCondition(nameof(sceneFiledType), (int)ESceneInputFieldType.Target)] private string targetSceneName;


        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void OnValidate()
        {
#if UNITY_EDITOR
            if (sceneFiledType.Equals(ESceneInputFieldType.Target))
            {
                if (Target && targetSceneName != Target.name)
                    targetSceneName = Target.name;
            }
#endif
        }

        public Scene GetScene()
        {
            switch (sceneFiledType)
            {
                case ESceneInputFieldType.Name:
                    return SceneManager.GetSceneByName(sceneName);
                case ESceneInputFieldType.Number:
                    return SceneManager.GetSceneByBuildIndex(sceneNumber);
                case ESceneInputFieldType.Target:
                    return SceneManager.GetSceneByName(targetSceneName);
                default:
                    return default;
            }
        }
        public AsyncOperation LoadScene(LoadSceneMode mode = LoadSceneMode.Single)
        {
            switch (sceneFiledType)
            {
                case ESceneInputFieldType.Name:
                    if (SceneManager.GetSceneByName(sceneName).isLoaded)
                        return null;

                    return SceneManager.LoadSceneAsync(sceneName, mode);
                case ESceneInputFieldType.Number:
                    if (SceneManager.GetSceneByBuildIndex(sceneNumber).isLoaded)
                        return null;

                    return SceneManager.LoadSceneAsync(sceneNumber, mode);
                case ESceneInputFieldType.Target:
                    if (SceneManager.GetSceneByName(targetSceneName).isLoaded)
                        return null;

                    return SceneManager.LoadSceneAsync(targetSceneName, mode);
                default:
                    return default;
            }
        }
        public AsyncOperation UnLoadScene(UnloadSceneOptions options = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects)
        {
            switch (sceneFiledType)
            {
                case ESceneInputFieldType.Name:
                    if (!SceneManager.GetSceneByName(sceneName).isLoaded)
                        return null;

                    return SceneManager.UnloadSceneAsync(sceneName, options);

                case ESceneInputFieldType.Number:
                    if (!SceneManager.GetSceneByBuildIndex(sceneNumber).isLoaded)
                        return null;

                    return SceneManager.UnloadSceneAsync(sceneNumber, options);

                case ESceneInputFieldType.Target:
                    if (!SceneManager.GetSceneByName(targetSceneName).isLoaded)
                        return null;

                    return SceneManager.UnloadSceneAsync(targetSceneName, options);

                default:
                    return default;
            }
        }
    }
}
