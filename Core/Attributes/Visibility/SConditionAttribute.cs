using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace StudioScor.Utilities
{
    public class SConditionAttribute : VisibilityPropertyAttribute
	{
		public string Condition = "";
		public bool UseHide = false;
		public bool Inverse = false;
		public SConditionAttribute(string conditionBoolean)
		{
			Condition = conditionBoolean;
			UseHide = true;
		}

		public SConditionAttribute(string conditionBoolean, bool hideInInspector = true)
		{
			Condition = conditionBoolean;
			UseHide = hideInInspector;
			Inverse = false;
		}

		public SConditionAttribute(string conditionBoolean, bool hideInInspector = true, bool negative = false)
		{
			Condition = conditionBoolean;
			UseHide = hideInInspector;
			Inverse = negative;
		}

#if UNITY_EDITOR

		public override bool IsHidden => IsShouldDisplay;

		private bool IsEnabled = false;
		private bool IsShouldDisplay = false;
		private bool IsPreviouslyEnabled = false;

		public override void EnterEnable(Rect position, SerializedProperty property, GUIContent label)
		{
			IsEnabled = GetConditionAttributeResult(property);

			IsPreviouslyEnabled = GUI.enabled;
			IsShouldDisplay = !UseHide || IsEnabled;

			if (IsShouldDisplay)
			{
				GUI.enabled = IsEnabled;
			}
		}

		public override void ExitEnable(Rect position, SerializedProperty property, GUIContent label)
		{
			if (IsShouldDisplay)
			{
				GUI.enabled = IsPreviouslyEnabled;
			}
		}

        private bool GetConditionAttributeResult(SerializedProperty property)
		{
			bool enabled = true;

			string propertyPath = property.propertyPath;

			string conditionPath = propertyPath.Replace(property.name, Condition);

			SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

			if (sourcePropertyValue != null)
			{
				enabled = !sourcePropertyValue.boolValue;
			}
			else
			{
				var test = property.serializedObject.FindProperty(Condition);

				if(test != null)
                {
					enabled = test.boolValue;
				}
                else
                {
					Debug.LogWarning("No matching boolean found for ConditionAttribute in object: " + Condition);
				}
			}
			

			if (Inverse)
			{
				enabled = !enabled;
			}

			return enabled;
		}
#endif
    }
	
}