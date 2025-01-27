using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace StudioScor.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(ConditionalAttribute))]
    public class ConditionalAttributeDrawer : PropertyDrawer
    {
        public VisualTreeAsset _inspectorXML;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create a new VisualElement to be the root the property UI.
            var container = new VisualElement();

            _inspectorXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{UtilitiesPathUtility.InspectorPath}utilities-condition-custom-attribute.uxml");

            if (_inspectorXML)
            {
                var inspectorXML = _inspectorXML.Instantiate();

                var propertyField = inspectorXML.Q<PropertyField>("PropertyField_Field");
                propertyField.BindProperty(property);

                ConditionalAttribute condition = (ConditionalAttribute)attribute;

                bool enabled = true;
                string propertyPath = property.propertyPath;
                string conditionPath = propertyPath.Replace(property.name, condition.Condition);

                SerializedProperty conditionPropertyValue = property.serializedObject.FindProperty(conditionPath);

                if (conditionPropertyValue != null && conditionPropertyValue.propertyType == SerializedPropertyType.Boolean)
                {
                    enabled = conditionPropertyValue.boolValue;
                }
                else
                {
                    container.Add(inspectorXML);
                    return container;
                }

                void UpdateVisibility()
                {
                    bool isVisible = conditionPropertyValue.propertyType == SerializedPropertyType.Boolean && conditionPropertyValue.boolValue;

                    foreach (var child in propertyField.parent.Children())
                    {
                        child.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
                    }
                }

                UpdateVisibility();


                container.TrackPropertyValue(conditionPropertyValue, _ =>
                {
                    UpdateVisibility();
                });

                container.Add(inspectorXML);
            }

            return container;
        }
    }
}