using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace StudioScor.Utilities
{
    public class SHiddenAttribute : VisibilityPropertyAttribute
	{
#if UNITY_EDITOR
		public override bool IsHidden => true;

		public override void EnterEnable(Rect position, SerializedProperty property, GUIContent label)
		{
			GUI.enabled = false;
		}

		public override void ExitEnable(Rect position, SerializedProperty property, GUIContent label)
		{
			GUI.enabled = false;
		}
#endif
	}
	
}