using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace StudioScor.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxAttribute))]
    public class MinMaxAttributeDrawer : PropertyDrawer
    {
        public VisualTreeAsset _inspectorXML;
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create a new VisualElement to be the root the property UI.
            var container = new VisualElement();

            _inspectorXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{UtilitiesPathUtility.InspectorPath}utilities-minmax-custom-attribute.uxml");

            if (_inspectorXML)
            {
                var inspectorXML = _inspectorXML.Instantiate();

                var minmaxSlider = inspectorXML.Q<MinMaxSlider>("MinMaxSlider_Slider");
                minmaxSlider.BindProperty(property);
                minmaxSlider.label = property.displayName;

                var vector = inspectorXML.Q<Vector2Field>("Vector2Field_Value");
                vector.BindProperty(property);

                MinMaxAttribute minmax = (MinMaxAttribute)attribute;

                minmaxSlider.lowLimit = minmax.Min;
                minmaxSlider.highLimit = minmax.Max;

                vector.RegisterValueChangedCallback((callback) =>
                {
                    vector.value = new Vector2(Mathf.Max(minmax.Min, vector.value.x), Mathf.Min(minmax.Max, vector.value.y));
                });
                container.Add(inspectorXML);
            }

            return container;
        }
    }
}