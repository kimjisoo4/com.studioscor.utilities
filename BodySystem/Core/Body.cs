using UnityEngine;

namespace KimScor.BodySystem
{
    [CreateAssetMenu(menuName = "Body/new Body", fileName = "Body_")]
    public class Body : ScriptableObject
    {
        [SerializeField] private string _Name;
        [SerializeField, TextArea] private string _Description;

        public string Name => _Name;
        public string Description => _Description;
    }

}
