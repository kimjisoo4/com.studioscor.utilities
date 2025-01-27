using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName ="StudioScor/Utilities/Damageable/new DamageType", fileName = "DT_")]
	public class DamageType : BaseScriptableObject
    {
        [Header(" [ Damage Type ] ")]
        [SerializeField] private string _id;

        public string ID => _id;

        [ContextMenu(nameof(NameToID), false, 1000000)]
        private void NameToID()
        {
            _id = name;
        }
    }
}
