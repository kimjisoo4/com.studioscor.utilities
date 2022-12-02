using UnityEngine;

namespace StudioScor.PlayerSystem
{
    [CreateAssetMenu(menuName = "StateMachine/new BlackboardKey")]
    public class BlackBoardKey : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private EBlackBoardKeyType _Type;
        [SerializeField] private bool _UseParameter = false;
        [SerializeField] private string _Parameter = "";

        private int _Hash;
        public EBlackBoardKeyType Type => _Type;
        public int Hash => _Hash;
        public bool UseParameter => _UseParameter;

        public void OnAfterDeserialize()
        {
            _Hash = Animator.StringToHash(_Parameter);
        }

        public void OnBeforeSerialize()
        {
        }
    }
    
}
