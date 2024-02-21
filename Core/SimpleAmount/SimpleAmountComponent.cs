using System.Collections.Generic;
using UnityEngine;


namespace StudioScor.Utilities
{

    [AddComponentMenu("StudioScor/Utilities/SimpleAmount/Simple Amount Component", order: 0)]
    public class SimpleAmountComponent : BaseMonoBehaviour, ISimpleAmount
    {
        [Header(" [ Simple Amount ] ")]
        [SerializeField] private float currentValue;
        [SerializeField] private float maxValue;

        private readonly List<ISimpleAmountModifier> modifiers = new();

        private bool needUpdate;
        private float normalizedValue;
        private float prevValue;

        public float PrevValue => prevValue;
        public float CurrentValue => currentValue;
        public float MaxValue => maxValue;
        public float NormalizedValue => normalizedValue;

        public void AddModifier(ISimpleAmountModifier modifier)
        {
            if(!modifiers.Contains(modifier))
            {
                modifiers.Add(modifier);
            }
        }
        public void RemoveModifier(ISimpleAmountModifier modifier)
        {
            modifiers.Remove(modifier);
        }

        public void SetMaxValue(float maxValue)
        {
            this.maxValue = maxValue;

            normalizedValue = currentValue / this.maxValue;

            needUpdate = true;
        }
        public void SetCurrentValue(float currentValue)
        {
            prevValue = this.currentValue;
            this.currentValue = currentValue;

            normalizedValue = this.currentValue / maxValue;

            needUpdate = true;
        }
        public void SetValue(float maxValue, float currentValue)
        {
            prevValue = this.currentValue;
            this.currentValue = currentValue;
            this.maxValue = maxValue;

            normalizedValue = this.currentValue.SafeDivide(this.maxValue);

            needUpdate = true;
        }

        private void LateUpdate()
        {
            if (!needUpdate)
                return;

            needUpdate = false;

            UpdateAmount();
        }

        public void UpdateAmount()
        {
            foreach (var modifier in modifiers)
            {
                modifier.UpdateModifier();
            }
        }
    }

}