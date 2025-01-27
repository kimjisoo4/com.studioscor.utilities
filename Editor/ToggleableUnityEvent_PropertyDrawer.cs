using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace StudioScor.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(ToggleableUnityEvent))]
    public class ToggleableUnityEvent_PropertyDrawer : PropertyDrawer
    {
        public VisualTreeAsset _inspectorXML;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create a new VisualElement to be the root the property UI.
            var container = new VisualElement();

            _inspectorXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{UtilitiesPathUtility.InspectorPath}utilities-toggleable-unityevent-custom-inspector.uxml");

            if (_inspectorXML)
            {
                var inspectorXML = _inspectorXML.Instantiate();

                var useUnityEventProperty = property.FindPropertyRelative("_useUnityEvent");

                var useUnityEventField = inspectorXML.Q<PropertyField>("PropertyField_UseUnityEvent");
                useUnityEventField.label = $"Use {property.displayName.ToBold()}";

                bool useUnityEvent = useUnityEventProperty.boolValue;

                var unityEvent = inspectorXML.Q<PropertyField>("PropertyField_UnityEvent");
                unityEvent.label = $"{property.displayName}";
                unityEvent.style.display = useUnityEvent ? DisplayStyle.Flex : DisplayStyle.None;
                unityEvent.visible = useUnityEvent;

                useUnityEventField.RegisterValueChangeCallback((useUnity) =>
                {
                    bool useUnityEvent = useUnityEventProperty.boolValue;

                    unityEvent.style.display = useUnityEvent ? DisplayStyle.Flex : DisplayStyle.None;
                    unityEvent.visible = useUnityEvent;

                });

                container.Add(inspectorXML);
            }

            return container;
        }
    }
}