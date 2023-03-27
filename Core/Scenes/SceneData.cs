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
        [SerializeField] private ESceneInputFieldType _Type = ESceneInputFieldType.Target;

        [SerializeField, SEnumCondition(nameof(_Type), (int)ESceneInputFieldType.Name)] private string _Name;
        [SerializeField, SEnumCondition(nameof(_Type), (int)ESceneInputFieldType.Number)] private int _Number;

#if UNITY_EDITOR
        [SEnumCondition(nameof(_Type), (int)ESceneInputFieldType.Target)] public Object Target;
#endif

        [SerializeField][SEnumCondition(nameof(_Type), (int)ESceneInputFieldType.Target)] private string _TargetName;


        [Conditional("UNITY_EDITOR")]
        public void OnValidate()
        {
#if UNITY_EDITOR
            if (_Type.Equals(ESceneInputFieldType.Target))
            {
                if (Target && _TargetName != Target.name)
                    _TargetName = Target.name;
            }
#endif
        }

        public Scene GetScene()
        {
            switch (_Type)
            {
                case ESceneInputFieldType.Name:
                    return SceneManager.GetSceneByName(_Name);
                case ESceneInputFieldType.Number:
                    return SceneManager.GetSceneByBuildIndex(_Number);
                case ESceneInputFieldType.Target:
                    return SceneManager.GetSceneByName(_TargetName);
                default:
                    return default;
            }
        }
        public AsyncOperation LoadScene(LoadSceneMode mode = LoadSceneMode.Single)
        {
            switch (_Type)
            {
                case ESceneInputFieldType.Name:
                    if (SceneManager.GetSceneByName(_Name).isLoaded)
                        return null;

                    return SceneManager.LoadSceneAsync(_Name, mode);
                case ESceneInputFieldType.Number:
                    if (SceneManager.GetSceneByBuildIndex(_Number).isLoaded)
                        return null;

                    return SceneManager.LoadSceneAsync(_Number, mode);
                case ESceneInputFieldType.Target:
                    if (SceneManager.GetSceneByName(_TargetName).isLoaded)
                        return null;

                    return SceneManager.LoadSceneAsync(_TargetName, mode);
                default:
                    return default;
            }
        }
        public AsyncOperation UnLoadScene(UnloadSceneOptions options = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects)
        {
            switch (_Type)
            {
                case ESceneInputFieldType.Name:
                    if (!SceneManager.GetSceneByName(_Name).isLoaded)
                        return null;

                    return SceneManager.UnloadSceneAsync(_Name, options);

                case ESceneInputFieldType.Number:
                    if (!SceneManager.GetSceneByBuildIndex(_Number).isLoaded)
                        return null;

                    return SceneManager.UnloadSceneAsync(_Number, options);

                case ESceneInputFieldType.Target:
                    if (!SceneManager.GetSceneByName(_TargetName).isLoaded)
                        return null;

                    return SceneManager.UnloadSceneAsync(_TargetName, options);

                default:
                    return default;
            }
        }
    }
}
