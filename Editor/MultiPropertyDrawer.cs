using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace StudioScor.Utilities.Editor
{

    [CustomPropertyDrawer(typeof(MultiPropertyAttribute), true)]
    public class MultiPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            MultiPropertyAttribute multiAttribute = attribute as MultiPropertyAttribute;

            float height = base.GetPropertyHeight(property, label);
            VisibilityPropertyAttribute vis;

            foreach (object visibility in multiAttribute.Visibilities)
            {
                vis = visibility as VisibilityPropertyAttribute;

                if (vis != null)
                {
                    if (vis.IsHidden)
                    {
                        return -EditorGUIUtility.standardVerticalSpacing;
                    }
                }
            }

            foreach (object atr in multiAttribute.Fields)
            {
                if (atr as MultiPropertyAttribute != null)
                {
                    //build label here too?
                    var tempheight = ((MultiPropertyAttribute)atr).GetPropertyHeight(property, label);

                    if (tempheight.HasValue)
                    {
                        return tempheight.Value; 
                    }
                }
            }

            return height;
        }
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MultiPropertyAttribute multiAttribute = attribute as MultiPropertyAttribute;
            // First get the attribute since it contains the range for the slider
            if (multiAttribute.Fields == null || multiAttribute.Fields.Count == 0)
            {
                multiAttribute.Fields = fieldInfo.GetCustomAttributes(typeof(EditorGUIFieldPropertyAttribute), false).OrderBy(s => ((PropertyAttribute)s).order).ToList();
            }

            if (multiAttribute.Visibilities == null || multiAttribute.Visibilities.Count == 0)
            {
                multiAttribute.Visibilities = fieldInfo.GetCustomAttributes(typeof(VisibilityPropertyAttribute), false).OrderBy(s => ((PropertyAttribute)s).order).ToList();
            }

            if (multiAttribute.Decorators == null || multiAttribute.Decorators.Count == 0)
            {
                multiAttribute.Decorators = fieldInfo.GetCustomAttributes(typeof(DecoratorPropertyAttribute), false).OrderBy(s => ((PropertyAttribute)s).order).ToList();
            }

            var origColor = UnityEngine.GUI.color;
            var Label = label;

            VisibilityPropertyAttribute visibilityProperty;
            EditorGUIFieldPropertyAttribute editorGUIFieldProperty;
            DecoratorPropertyAttribute decoratorProperty;

            bool isHidden = false;

            foreach (object visibility in multiAttribute.Visibilities)
            {
                visibilityProperty = visibility as VisibilityPropertyAttribute;

                if (visibilityProperty != null)
                {
                    visibilityProperty.EnterEnable(position, property, Label);

                    isHidden = visibilityProperty.IsHidden;

                    break;
                }
            }

            if(!isHidden)
            {
                if(multiAttribute.Fields.Count > 0)
                {
                    foreach (object atr in multiAttribute.Fields)
                    {
                        editorGUIFieldProperty = atr as EditorGUIFieldPropertyAttribute;

                        if (editorGUIFieldProperty != null)
                        {
                            editorGUIFieldProperty.OnEditorGUI(position, property, Label);

                            break;
                        }
                    }
                }
                else
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
                
                foreach (object atr in multiAttribute.Decorators)
                {
                    decoratorProperty = atr as DecoratorPropertyAttribute;

                    if (decoratorProperty != null)
                    {
                        Label = decoratorProperty.BuildLabel(Label);

                        decoratorProperty.OnDecorator(position, property, Label);
                    }
                }
            }

            foreach (object visivilitit in multiAttribute.Visibilities)
            {
                visibilityProperty = visivilitit as VisibilityPropertyAttribute;

                if (visibilityProperty != null)
                {
                    visibilityProperty.ExitEnable(position, property, Label);

                    break;
                }
            }

            UnityEngine.GUI.color = origColor;
        }
    }
}