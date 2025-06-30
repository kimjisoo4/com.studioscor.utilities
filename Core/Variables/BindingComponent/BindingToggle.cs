using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    public class BindingToggle : BindingComponent<Toggle>
    {
        [Header(" [ Binding Toggle ] ")]
        [SerializeField] private SOBoolVariable _variable;

        protected override void Awake()
        {
            base.Awake();

            Component.isOn = _variable;

            Component.onValueChanged.AddListener(Toggle_OnValueChanged);
            _variable.OnValueChanged += Variable_OnValueChanged;
        }

        private void Variable_OnValueChanged(SOVariable<bool> variable, bool currentValue, bool prevValue)
        {
            Component.isOn = currentValue;
        }

        private void Toggle_OnValueChanged(bool isOn)
        {
            _variable.Value = isOn;
        }
    }

}
