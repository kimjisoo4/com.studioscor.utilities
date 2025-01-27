using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Variable/new Empty Variable", fileName = "Variable_")]
	public class EmptyVariable : BaseScriptableObject
    {
        [SerializeField] protected string _id;
		[SerializeField, TextArea] protected string _description;


        [ContextMenu(nameof(NameToID))]
        protected virtual void NameToID()
        {
            _id = name;
        }
    }

}
