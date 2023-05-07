using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace StudioScor.Utilities
{
    public class SReadOnlyWhenPlayingAttribute : VisibilityPropertyAttribute
    {
#if UNITY_EDITOR
		public override bool IsHidden => false;

        public override void EnterEnable(Rect position, SerializedProperty property, GUIContent label)
        {
			GUI.enabled = !Application.isPlaying;
		}
        public override void ExitEnable(Rect position, SerializedProperty property, GUIContent label)
        {
			GUI.enabled = true;
		}
#endif
	}
	
}