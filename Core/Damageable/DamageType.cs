using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName ="StudioScor/Utilities/Damageable/new DamageType", fileName = "DT_")]
	public class DamageType : BaseScriptableObject
    {
        [Header(" [ Damage Type ] ")]
        [SerializeField] private string _name;
        [SerializeField][TextArea] private string _description;

        public string Name => _name;
        public string Description => _description; 
    }
}
