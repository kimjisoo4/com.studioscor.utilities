using UnityEngine;
using System;
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

    public class STagSelectorAttribute : EditorGUIFieldPropertyAttribute
	{
		public bool UseDefaultTagFieldDrawer = false;

#if UNITY_EDITOR
		public override void OnEditorGUI(Rect position, SerializedProperty property, GUIContent label)
        {
			if (property.propertyType == SerializedPropertyType.String)
			{
				EditorGUI.BeginProperty(position, label, property);

				if (UseDefaultTagFieldDrawer)
				{
					property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
				}
				else
				{
					//generate the taglist + custom tags
					List<string> tagList = new List<string>();
					tagList.Add("<NoTag>");
					tagList.AddRange(UnityEditorInternal.InternalEditorUtility.tags);
					string propertyString = property.stringValue;
					int index = -1;
					if (propertyString == "")
					{
						//The tag is empty
						index = 0; //first index is the special <notag> entry
					}
					else
					{
						//check if there is an entry that matches the entry and get the index
						//we skip index 0 as that is a special custom case
						for (int i = 1; i < tagList.Count; i++)
						{
							if (tagList[i] == propertyString)
							{
								index = i;
								break;
							}
						}
					}

					//Draw the popup box with the current selected index
					index = EditorGUI.Popup(position, label.text, index, tagList.ToArray());

					//Adjust the actual string value of the property based on the selection
					if (index == 0)
					{
						property.stringValue = "";
					}
					else if (index >= 1)
					{
						property.stringValue = tagList[index];
					}
					else
					{
						property.stringValue = "";
					}
				}

				EditorGUI.EndProperty();
			}
			else
			{
				EditorGUI.PropertyField(position, property, label);
			}
		}
#endif
	}

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

	public class SReadOnlyAttribute : VisibilityPropertyAttribute
	{

#if UNITY_EDITOR

		public override bool IsHidden => false;

		public override void EnterEnable(Rect position, SerializedProperty property, GUIContent label)
        {
			GUI.enabled = false;
		}
        public override void ExitEnable(Rect position, SerializedProperty property, GUIContent label)
        {
			GUI.enabled = true;
		}
#endif
    }
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

	public abstract class DecoratorPropertyAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
		
		public abstract void OnDecorator(Rect position, SerializedProperty property, GUIContent label);
#endif
	}
	public abstract class EditorGUIFieldPropertyAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
		public abstract void OnEditorGUI(Rect position, SerializedProperty property, GUIContent label);
#endif
	}
	public abstract class VisibilityPropertyAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
		public abstract bool IsHidden { get; }

		public abstract void EnterEnable(Rect position, SerializedProperty property, GUIContent label);
		public abstract void ExitEnable(Rect position, SerializedProperty property, GUIContent label);
#endif
	}

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