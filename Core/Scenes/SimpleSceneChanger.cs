using UnityEngine;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class SimpleSceneChanger : BaseMonoBehaviour
    {
        public enum ESceneInputFieldType
        {
            Restart,
            Name,
            Number,
            Target,
        }

        [Header(" [ Simple Scene Changer ]")]
        [SerializeField] private ESceneInputFieldType _Type;
        [SerializeField][SEnumCondition(nameof(_Type), (int)ESceneInputFieldType.Name, (int)ESceneInputFieldType.Target, (int)ESceneInputFieldType.Number)] private LoadSceneMode _Mode;
        [SerializeField] private UnloadSceneOptions _UnloadOption;
        [SerializeField] private bool _IsAutoPlaying = false;

        [SerializeField, SEnumCondition(nameof(_Type), (int)ESceneInputFieldType.Name)] private string _Name;
        [SerializeField, SEnumCondition(nameof(_Type), (int)ESceneInputFieldType.Number)] private int _Number;

#if UNITY_EDITOR
        [SEnumCondition(nameof(_Type), (int)ESceneInputFieldType.Target)] public Object _Target;
#endif

        [SerializeField][SEnumCondition(nameof(_Type), (int)ESceneInputFieldType.Target)] private string _TargetName;

        [SerializeField] private UnityEvent _OnStartedLoad;
        [SerializeField] private UnityEvent _OnFinishedLoad;

        private bool IsAdditve => _Mode.Equals(LoadSceneMode.Additive);

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (_Type.Equals(ESceneInputFieldType.Target))
            {
                _TargetName = _Target.name;
            }
#endif
        }

        private void Start()
        {
            if (_IsAutoPlaying)
                LoadScene();
        }

        public void LoadScene()
        {
            Log("Started Load Scene");

            switch (_Type)
            {
                case ESceneInputFieldType.Restart:
                    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name).completed += SimpleSceneChanger_Completed;
                    break;
                case ESceneInputFieldType.Name:
                    if (IsAdditve && SceneManager.GetSceneByName(_Name).isLoaded)
                    {
                        Callback_OnFinishedLoad();

                        return;
                    }

                    SceneManager.LoadSceneAsync(_Name, _Mode).completed += SimpleSceneChanger_Completed;

                    break;
                case ESceneInputFieldType.Number:
                    if (IsAdditve && SceneManager.GetSceneByName(_Name).isLoaded)
                    {
                        Callback_OnFinishedLoad();

                        return;
                    }

                    SceneManager.LoadSceneAsync(_Number, _Mode).completed += SimpleSceneChanger_Completed;

                    break;
                case ESceneInputFieldType.Target:
                    if (IsAdditve && SceneManager.GetSceneByName(_TargetName).isLoaded)
                    {
                        Callback_OnFinishedLoad();

                        return;
                    }

                    SceneManager.LoadSceneAsync(_TargetName, _Mode).completed += SimpleSceneChanger_Completed;

                    break;
                default:
                    break;
            }

            _OnStartedLoad?.Invoke();
        }

        private void SimpleSceneChanger_Completed(AsyncOperation async)
        {
            async.completed -= SimpleSceneChanger_Completed;

            Callback_OnFinishedLoad();
        }
        private void Callback_OnFinishedLoad()
        {
            Log("Completed Load Scene");

            _OnFinishedLoad?.Invoke();
        }

        public void UnLoadScene()
        {
            Log("Un Load Scene");

            switch (_Type)
            {
                case ESceneInputFieldType.Restart:
                    SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name, _UnloadOption);
                    break;
                case ESceneInputFieldType.Name:
                    SceneManager.UnloadSceneAsync(_Name, _UnloadOption);
                    break;
                case ESceneInputFieldType.Number:
                    SceneManager.UnloadSceneAsync(_Number, _UnloadOption);
                    break;
                case ESceneInputFieldType.Target:
                    SceneManager.UnloadSceneAsync(_TargetName, _UnloadOption);
                    break;
                default:
                    break;
            }
        }
    }
}
