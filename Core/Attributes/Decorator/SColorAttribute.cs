using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace StudioScor.Utilities
{
    public class SColorAttribute : DecoratorPropertyAttribute
	{
		Color Color;
		public SColorAttribute(float R, float G, float B)
		{
			Color = new Color(R, G, B);
		}
		public SColorAttribute(Color color)
        {
			Color = color;
        }

#if UNITY_EDITOR
        public override void OnDecorator(Rect position, SerializedProperty property, GUIContent label)
        {
			GUI.color = Color;
		}
#endif
    }
	
}