using UnityEngine;
using UnityEngine.SceneManagement;


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
        [SerializeField, SEnumCondition(nameof(_Type), (int)ESceneInputFieldType.Name)] private string _Name;
        [SerializeField, SEnumCondition(nameof(_Type), (int)ESceneInputFieldType.Number)] private int _Number;
        [SerializeField, SEnumCondition(nameof(_Type), (int)ESceneInputFieldType.Target)] private Object _Target;

        public void LoadScene()
        {
            Log("Load Scene");

            switch (_Type)
            {
                case ESceneInputFieldType.Restart:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                case ESceneInputFieldType.Name:
                    SceneManager.LoadScene(_Name);
                    break;
                case ESceneInputFieldType.Number:
                    SceneManager.LoadScene(_Number);
                    break;
                case ESceneInputFieldType.Target:
                    SceneManager.LoadScene(_Target.name);
                    break;
                default:
                    break;
            }
        }

    }
}
