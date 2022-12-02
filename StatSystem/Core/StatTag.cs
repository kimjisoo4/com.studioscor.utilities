using UnityEngine;



namespace StudioScor.StatSystem
{
    [CreateAssetMenu(fileName ="Stat_", menuName = "Stat/new Stat")]
    public class StatTag : ScriptableObject
    {
        [Header("[Name]")]
        [SerializeField] private string _StatName;
        [Header("[Text]")]
        [SerializeField] private string _Description;

        public string StatName => _StatName;
        public string Description => _Description;

        public void SetStatTag(string newName, string description = null)
        {
            _StatName = newName;
            _Description = description;
        }
    }
}
