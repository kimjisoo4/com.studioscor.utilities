using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace StudioScor.Utilities
{

    public class SimpleAmount : BaseMonoBehaviour
    {
        private float _CurrentValue;
        private float _MaxValue;
        private float _NormalizedValue;
        private float _PrevValue;

        public float PrevValue => _PrevValue;
        public float CurrentValue => _CurrentValue;
        public float MaxValue => _MaxValue;
        public float NormalizedValue => _NormalizedValue;

        private List<SimpleAmountModifier> _Modifiers;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            if (_Modifiers is null)
                _Modifiers = new();
        }

        public void AddModifier(SimpleAmountModifier modifier)
        {
            if(_Modifiers.Contains(modifier))
            {
                Log("Contains This Modifier - " + modifier);
            }

            _Modifiers.Add(modifier);
        }
        public void RemoveModifier(SimpleAmountModifier modifier)
        {
            if (!_Modifiers.Remove(modifier))
            {
                Log("Not Contains Thi Modifier - " + modifier);
            }
        }

        public void SetMaxValue(float maxValue)
        {
            _MaxValue = maxValue;

            _NormalizedValue = _CurrentValue / _MaxValue;

            UpdateAmount();
        }
        public void SetCurrentValue(float currentValue)
        {
            _PrevValue = _CurrentValue;
            _CurrentValue = currentValue;

            _NormalizedValue = _CurrentValue / _MaxValue;

            UpdateAmount();
        }
        public void SetValue(float currentValue, float maxValue)
        {
            _PrevValue = _CurrentValue;
            _CurrentValue = currentValue;
            _MaxValue = maxValue;

            _NormalizedValue = _CurrentValue / _MaxValue;

            UpdateAmount();
        }

        public void UpdateAmount()
        {
            foreach (var modifier in _Modifiers)
            {
                modifier.UpdateValue();
            }
        }
    }

}