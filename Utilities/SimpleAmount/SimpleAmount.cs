using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    public abstract class SimpleAmount : MonoBehaviour
    {
        private float _CurrentValue;
        private float _MaxValue;
        private float _NormalizedValue;

        public float CurrentValue => _CurrentValue;
        public float MaxValue => _MaxValue;
        public float NormalizedValue => _NormalizedValue;


        public void SetMaxValue(float maxValue)
        {
            _MaxValue = maxValue;

            _NormalizedValue = _CurrentValue / _MaxValue;

            UpdateAmount();
        }
        public void SetCurrentValue(float currentValue)
        {
            _CurrentValue = currentValue;

            _NormalizedValue = _CurrentValue / _MaxValue;

            UpdateAmount();
        }
        public void SetValue(float currentValue, float maxValue)
        {
            _CurrentValue = currentValue;
            _MaxValue = maxValue;

            _NormalizedValue = _CurrentValue / _MaxValue;

            UpdateAmount();
        }

        public abstract void UpdateAmount();
    }

}