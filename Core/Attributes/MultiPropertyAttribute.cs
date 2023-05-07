using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace StudioScor.Utilities
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
	public abstract class MultiPropertyAttribute : PropertyAttribute
	{
#if UNITY_EDITOR
		public List<object> Visibilities = new List<object>();
		public List<object> Fields = new List<object>();
		public List<object> Decorators = new List<object>();

		public virtual GUIContent BuildLabel(GUIContent label)
		{
			return label;
		}
		public virtual float? GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return null;
		}
#endif
	}
	
}