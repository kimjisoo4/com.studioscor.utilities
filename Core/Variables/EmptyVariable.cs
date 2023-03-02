using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "Utilities/Variable/new Empty Variable", fileName = "Variable_")]
	public class EmptyVariable : BaseScriptableObject
    {
		[SerializeField] protected string _Name;
		[SerializeField, TextArea] protected string _Description;
    }

}
