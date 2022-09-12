﻿using UnityEngine;

namespace KimScor.Utilities
{
    [CreateAssetMenu(menuName = "Utilities/Variable/new Empty Variable", fileName = "Variable_")]
	public class EmptyVariable : ScriptableObject
    {
		[SerializeField] protected string _Name;
		[SerializeField, TextArea] protected string _Description;
    }

}
