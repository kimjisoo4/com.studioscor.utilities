using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace StudioScor.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(UnitAttribute))]
    public class UnitAttributeDrawer : PropertyDrawer
    {
        public VisualTreeAsset _inspectorXML;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            _inspectorXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{UtilitiesPathUtility.InspectorPath}utilities-unit-custom-attribute.uxml");

            if (_inspectorXML)
            {
                var inspectorXML = _inspectorXML.Instantiate();

                var propertyField = inspectorXML.Q<PropertyField>("PropertyField_Field");
                propertyField.BindProperty(property);

                UnitAttribute unitAttribute = (UnitAttribute)attribute;
                var unitLabel = inspectorXML.Q<Label>("Label_Unit");
                unitLabel.text = unitAttribute.Unit;

                unitLabel.enabledSelf = propertyField.enabledSelf;
                unitLabel.style.display = propertyField.style.display;
                unitLabel.style.visibility = propertyField.style.visibility;

                container.Add(inspectorXML);
            }

            return container;
        }
    }
}