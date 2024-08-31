using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Variable/new Empty Variable", fileName = "Variable_")]
	public class EmptyVariable : BaseScriptableObject
    {
		[SerializeField] protected string _name;
		[SerializeField, TextArea] protected string _description;
    }

}
