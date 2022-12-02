using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace StudioScor.StatSystem
{
	[System.Serializable]
	public class Stat : ISerializationCallbackReceiver
	{
		#region Events
		public delegate void ChangedValue(Stat stat, float currentValue, float prevValue);
		#endregion

		[Header("[Text]")]
		[SerializeField] private string _StatName;
		[SerializeField] private string _Description;

		[Header("[Tag]")]
        [SerializeField] private StatTag _StatTag;

		[Header("[Value]")]
		[SerializeField] private float _BaseValue;

		[SerializeField] protected float _PrevValue;
		[SerializeField] protected float _Value;

		[Header("[Modifier]")]
		[SerializeField] protected List<StatModifier> _StatModifiers;

		public event ChangedValue OnChangedValue;

		public StatTag StatTag => _StatTag;
		public string StatName => _StatName;
		public string Description => _Description;
		public float BaseValue => _BaseValue;
		public virtual float Value => _Value;
		public virtual float PrevValue => _PrevValue;

		public ReadOnlyCollection<StatModifier> StatModifiers;

		public Stat()
		{
			_StatName = null;
			_Description = null;

			_StatTag = null;

			_BaseValue = 0;

			_Value = 0;
			_PrevValue = 0;

			_StatModifiers = new List<StatModifier>();
		}
		public Stat(StatTag Tag, float Value)
        {
			_StatName = Tag.StatName;
			_Description = Tag.Description;

			_StatTag = Tag;

			_BaseValue = Value;

			_Value = Value;
			_PrevValue = 0;

			_StatModifiers = new List<StatModifier>();
		}

		public Stat(Stat stat)
        {
			SetUp(stat);
		}

		public void SetUp(Stat stat)
        {
			_StatName = stat.StatTag.name;
			_Description = stat.StatTag.Description;

			_StatTag = stat.StatTag;

			_BaseValue = stat.BaseValue;

			_Value = BaseValue;
			_PrevValue = 0;

			_StatModifiers = new List<StatModifier>();
		}

		public void SetBaseValue(float value)
        {
			_BaseValue = value;

			UpdateValue();
        }

		public virtual void AddModifier(StatModifier mod)
		{
			_StatModifiers.Add(mod);

			UpdateValue();
		}

		public virtual bool RemoveModifier(StatModifier mod)
		{
			if (_StatModifiers.Remove(mod))
			{
				UpdateValue();

				return true;
			}

			return false;
		}

		public virtual bool RemoveAllModifiersFromSource(object source)
		{
			int numRemovals = _StatModifiers.RemoveAll(mod => mod.Source == source);

			if (numRemovals > 0)
			{
				UpdateValue();

				return true;
			}
			return false;
		}

		public virtual float UpdateValue()
        {
			_PrevValue = _Value;

			_Value = CalculateFinalValue();


            if (_PrevValue != _Value)
            {
				OnChangedValue?.Invoke(this, _Value, _PrevValue);
			}

			return Value;
		}

		protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
		{
			if (a.Order < b.Order)
				return -1;
			else if (a.Order > b.Order)
				return 1;
			return 0; //if (a.Order == b.Order)
		}
		
		protected virtual float CalculateFinalValue()
		{
			float finalValue = BaseValue;
			float sumPercentAdd = 0;

			_StatModifiers.Sort(CompareModifierOrder);

			for (int i = 0; i < _StatModifiers.Count; i++)
			{
				StatModifier mod = _StatModifiers[i];

				if (mod.Type == EStatModType.Flat)
				{
					finalValue += mod.Value;
				}
				else if (mod.Type == EStatModType.PercentAdd)
				{
					sumPercentAdd += mod.Value;

					if (i + 1 >= _StatModifiers.Count || _StatModifiers[i + 1].Type != EStatModType.PercentAdd)
					{
						finalValue *= sumPercentAdd;
						sumPercentAdd = 0;
					}
				}
				else if (mod.Type == EStatModType.PercentMult)
				{
					finalValue *= mod.Value;
				}
			}

			// Workaround for float calculation errors, like displaying 12.00001 instead of 12
			return (float)Math.Round(finalValue, 4);
		}

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
			_Value = BaseValue;
			_PrevValue = 0;
		}
    }
}
