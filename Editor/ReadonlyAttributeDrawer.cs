using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace StudioScor.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(SReadonlyAttribute))]
    public class ReadonlyAttributeDrawer : PropertyDrawer
    {
        public VisualTreeAsset _inspectorXML;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create a new VisualElement to be the root the property UI.
            var container = new VisualElement();

            _inspectorXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{UtilitiesPathUtility.InspectorPath}utilities-readonly-custom-attribute.uxml");

            if (_inspectorXML)
            {
                var inspectorXML = _inspectorXML.Instantiate();

                var propertyField = inspectorXML.Q<PropertyField>("PropertyField_Field");
                propertyField.BindProperty(property);

                SReadonlyAttribute readonlyAttribte = (SReadonlyAttribute)attribute;

                if (readonlyAttribte.ReadonlyWhenPlaying)
                {
                    if (Application.isPlaying)
                    {
                        propertyField.enabledSelf = false;
                    }
                    else
                    {
                        propertyField.enabledSelf = true;
                    }
                }
                else
                {
                    propertyField.enabledSelf = false;
                }

                container.Add(inspectorXML);
            }

            return container;
        }
    }
}