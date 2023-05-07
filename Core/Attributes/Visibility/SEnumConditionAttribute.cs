using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace StudioScor.Utilities
{
    public class SEnumConditionAttribute : VisibilityPropertyAttribute
    {
		public string ConditionEnum = "";
		public bool Hidden = false;

		BitArray _BitArray = new BitArray(32);
		public bool ContainsBitFlag(int enumValue)
		{
			return _BitArray.Get(enumValue);
		}

		public SEnumConditionAttribute(string conditionEnum, params int[] enumValues)
		{
			ConditionEnum = conditionEnum;
			Hidden = true;

			for (int i = 0; i < enumValues.Length; i++)
			{
				_BitArray.Set(enumValues[i], true);
			}
		}


#if UNITY_EDITOR

		private static Dictionary<string, string> cachedPaths = new Dictionary<string, string>();

		public override bool IsHidden => IsShouldDisplay;

		private bool IsEnabled = false;
		private bool IsShouldDisplay = false;
		private bool IsPreviouslyEnabled = false;

		public override void EnterEnable(Rect position, SerializedProperty property, GUIContent label)
		{
			IsEnabled = GetConditionAttributeResult(property);
			IsPreviouslyEnabled = GUI.enabled;
			IsShouldDisplay = !Hidden || IsEnabled;

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

			SerializedProperty enumProp;
			string enumPropPath = string.Empty;
			string propertyPath = property.propertyPath;

			if (!cachedPaths.TryGetValue(propertyPath, out enumPropPath))
			{
				enumPropPath = propertyPath.Replace(property.name, ConditionEnum);
				cachedPaths.Add(propertyPath, enumPropPath);
			}

			enumProp = property.serializedObject.FindProperty(enumPropPath);

			if (enumProp != null)
			{
				int currentEnum = enumProp.enumValueIndex;

				enabled = !ContainsBitFlag(currentEnum);
			}
			else
			{
				Debug.LogWarning("No matching boolean found for ConditionAttribute in object: " + ConditionEnum);
			}

			return enabled;
		}
#endif
    }
	
}