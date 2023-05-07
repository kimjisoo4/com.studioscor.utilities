using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace StudioScor.Utilities
{
    public abstract class EditorGUIFieldPropertyAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
		public abstract void OnEditorGUI(Rect position, SerializedProperty property, GUIContent label);
#endif
	}
	
}