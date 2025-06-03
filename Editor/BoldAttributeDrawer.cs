using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace StudioScor.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(SFontStyleAttribute))]
    public class BoldAttributeDrawer : PropertyDrawer
    {
        public VisualTreeAsset _inspectorXML;
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create a new VisualElement to be the root the property UI.
            var container = new VisualElement();

            _inspectorXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{UtilitiesPathUtility.InspectorPath}utilities-bold-custom-attribute.uxml");

            if (_inspectorXML)
            {
                var inspectorXML = _inspectorXML.Instantiate();

                var propertyField = inspectorXML.Q<PropertyField>("PropertyField_Field");
                propertyField.BindProperty(property);

                SFontStyleAttribute condition = (SFontStyleAttribute)attribute;

                FontStyle fontStyle = condition.UseBold ? condition.UseItalic ? FontStyle.BoldAndItalic : FontStyle.Bold : FontStyle.Normal;

                foreach (var child in propertyField.parent.Children())
                {
                    child.style.unityFontStyleAndWeight = fontStyle;
                }

                container.Add(inspectorXML);
            }

            return container;
        }
    }
}