using System;
using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    public class BindingSlider : BindingComponent<Slider>
    {
        [Header(" [ Binding Slider ]")]
        [SerializeField] private SOFloatVariable _variable;

        protected override void Awake()
        {
            base.Awake();

            Component.value = _variable.Value;

            Component.onValueChanged.AddListener(Slider_OnValueChanged);
            _variable.OnValueChanged += Variable_OnChangedValue;
        }
        private void OnDestroy()
        {
            if(Component)
            {
                Component.onValueChanged.RemoveListener(Slider_OnValueChanged);
            }

            if(_variable)
            {
                _variable.OnValueChanged -= Variable_OnChangedValue;
            }
        }

        private void Slider_OnValueChanged(float newValue)
        {
            _variable.Value = newValue;
        }
        private void Variable_OnChangedValue(SOVariable<float> variable, float currentValue, float prevValue)
        {
            Component.value = currentValue;
        }
    }

}
