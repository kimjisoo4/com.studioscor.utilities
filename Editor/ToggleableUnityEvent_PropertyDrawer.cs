#if SCOR_ENABLE_VISUALSCRIPTING
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

                var useUnityEvent = inspectorXML.Q<Toggle>("Toggle_UseUnityEvent");
                useUnityEvent.label = $"Use {property.displayName.ToBold()}";

                var unityEvent = inspectorXML.Q<PropertyField>("PropertyField_UnityEvent");
                unityEvent.label = $"{property.displayName}";
                unityEvent.BindProperty(property.FindPropertyRelative("_unityEvent"));

                unityEvent.style.display = useUnityEvent.value ? DisplayStyle.Flex : DisplayStyle.None;
                unityEvent.visible = useUnityEvent.value;

                useUnityEvent.RegisterValueChangedCallback((use) =>
                {
                    unityEvent.style.display = use.newValue ? DisplayStyle.Flex : DisplayStyle.None;
                    unityEvent.visible = use.newValue;
                });

                container.Add(inspectorXML);
            }

            return container;
        }
    }
}
#endif