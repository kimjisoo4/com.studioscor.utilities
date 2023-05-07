using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


namespace StudioScor.Utilities
{

    [AddComponentMenu("StudioScor/Utilities/SimpleAmount/Simple Amount Component", order: 0)]
    public class SimpleAmountComponent : BaseMonoBehaviour, ISimpleAmount
    {
        [Header(" [ Simple Amount ] ")]
        [SerializeField] private float _CurrentValue;
        [SerializeField] private float _MaxValue;

        private readonly List<ISimpleAmountModifier> _Modifiers = new();

        private bool _NeedUpdate;
        private float _NormalizedValue;
        private float _PrevValue;

        public float PrevValue => _PrevValue;
        public float CurrentValue => _CurrentValue;
        public float MaxValue => _MaxValue;
        public float NormalizedValue => _NormalizedValue;

        public void AddModifier(ISimpleAmountModifier modifier)
        {
            if(!_Modifiers.Contains(modifier))
            {
                _Modifiers.Add(modifier);
            }
        }
        public void RemoveModifier(ISimpleAmountModifier modifier)
        {
            _Modifiers.Remove(modifier);
        }

        public void SetMaxValue(float maxValue)
        {
            _MaxValue = maxValue;

            _NormalizedValue = _CurrentValue / _MaxValue;

            _NeedUpdate = true;
        }
        public void SetCurrentValue(float currentValue)
        {
            _PrevValue = _CurrentValue;
            _CurrentValue = currentValue;

            _NormalizedValue = _CurrentValue / _MaxValue;

            _NeedUpdate = true;
        }
        public void SetValue(float currentValue, float maxValue)
        {
            _PrevValue = _CurrentValue;
            _CurrentValue = currentValue;
            _MaxValue = maxValue;

            _NormalizedValue = _CurrentValue.SafeDivide(_MaxValue);

            _NeedUpdate = true;
        }

        private void LateUpdate()
        {
            if (!_NeedUpdate)
                return;

            _NeedUpdate = false;

            UpdateAmount();
        }

        public void UpdateAmount()
        {
            foreach (var modifier in _Modifiers)
            {
                modifier.UpdateModifier();
            }
        }
    }

}