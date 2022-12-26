using UnityEngine;

namespace StudioScor.PlayerSystem
{
    [CreateAssetMenu(menuName = "StateMachine/new BlackboardKey")]
    public class BlackBoardKey : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private EBlackBoardKeyType _Type;
        [SerializeField] private string _HasValueParameter = "Has";
        [SerializeField] private string _ValueChangeParameter = "Do";

        private int _HasValueHash;
        private int _TriggerValueChangeHash;
        public EBlackBoardKeyType Type => _Type;
        public int HasValueHash => _HasValueHash;
        public int TriggerValueChangeHash => _TriggerValueChangeHash;

        public void OnAfterDeserialize()
        {
            _HasValueHash = Animator.StringToHash(_HasValueParameter);
            _TriggerValueChangeHash = Animator.StringToHash(_ValueChangeParameter);
        }

        public void OnBeforeSerialize()
        {
        }
    }
    
}
