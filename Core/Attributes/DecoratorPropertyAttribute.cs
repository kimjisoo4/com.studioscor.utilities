using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace StudioScor.Utilities
{
    public abstract class DecoratorPropertyAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
		
		public abstract void OnDecorator(Rect position, SerializedProperty property, GUIContent label);
#endif
	}
	
}