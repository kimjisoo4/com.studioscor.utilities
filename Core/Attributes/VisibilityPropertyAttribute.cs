using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace StudioScor.Utilities
{
    public abstract class VisibilityPropertyAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
		public abstract bool IsHidden { get; }

		public abstract void EnterEnable(Rect position, SerializedProperty property, GUIContent label);
		public abstract void ExitEnable(Rect position, SerializedProperty property, GUIContent label);
#endif
	}
	
}