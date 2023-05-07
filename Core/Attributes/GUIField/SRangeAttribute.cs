using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace StudioScor.Utilities
{
    public class SRangeAttribute : EditorGUIFieldPropertyAttribute
	{
		public float Min;
		public float Max;

		public string MinProperty;
		public string MaxProperty;

		public bool UseMinProperty;
		public bool UseMaxProperty;

		public SRangeAttribute(float min, float max)
		{
			Min = min;
			Max = max;

			UseMinProperty = false;
			UseMaxProperty = false;
		}
		public SRangeAttribute(string min, float max)
		{
			MinProperty = min;
			Max = max;
			UseMinProperty = true;
			UseMaxProperty = false;
		}
		public SRangeAttribute(float min, string max)
		{
			Min = min;
			MaxProperty = max;
			UseMinProperty = false;
			UseMaxProperty = true;
		}
		public SRangeAttribute(string min, string max)
		{
			MinProperty = min;
			MaxProperty = max;
			UseMinProperty = true;
			UseMaxProperty = true;
		}

#if UNITY_EDITOR


		public override void OnEditorGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var min = Min;
			var max = Max;

			if (UseMinProperty || UseMaxProperty)
			{
				string propertyPath = property.propertyPath;
				string conditionPath;
				SerializedProperty sourcePropertyValue;

				if (UseMinProperty)
				{
					conditionPath = propertyPath.Replace(property.name, MinProperty);
					sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

					if (sourcePropertyValue != null)
					{
						if (sourcePropertyValue.propertyType == SerializedPropertyType.Integer)
						{
							min = sourcePropertyValue.intValue;
						}
						else
						{
							min = sourcePropertyValue.floatValue;
						}
					}
				}

				if (UseMaxProperty)
				{
					conditionPath = propertyPath.Replace(property.name, MaxProperty);
					sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

					if (sourcePropertyValue != null)
					{
						if (sourcePropertyValue.propertyType == SerializedPropertyType.Integer)
						{
							max = sourcePropertyValue.intValue;
						}
						else
						{
							max = sourcePropertyValue.floatValue;
						}
					}
				}
			}

			if (property.propertyType == SerializedPropertyType.Float)
			{
				if (min < max)
				{
					property.floatValue = Mathf.Clamp(property.floatValue, min, max);
				}
				else if (min > max)
				{
					property.floatValue = Mathf.Clamp(property.floatValue, max, min);
				}
				else
				{
					property.floatValue = min;
				}


				EditorGUI.Slider(position, property, min, max, label);
			}
			else if (property.propertyType == SerializedPropertyType.Integer)
			{
				if (min < max)
				{
					property.intValue = (int)Mathf.Clamp(property.floatValue, min, max);
				}
				else if (min > max)
				{
					property.intValue = (int)Mathf.Clamp(property.floatValue, max, min);
				}
				else
				{
					property.intValue = (int)min;
				}

				EditorGUI.IntSlider(position, property, (int)min, (int)max, label);
			}
			else
            {
				EditorGUI.LabelField(position, label.text, "Use Range with float or int.");
			}
		}

#endif
    }
	
}