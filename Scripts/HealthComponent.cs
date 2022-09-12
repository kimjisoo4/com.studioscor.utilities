using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimScor.Utilities
{
    public class HealthComponent : MonoBehaviour
	{
		public delegate void HealthStateHandler(HealthComponent health);
		public delegate void HealthValueChangeHandler (HealthComponent health, float currentValue, float prevValue);
		
		[SerializeField] private float _MaxHealth;
		[SerializeField] private float _Health;
	
		public float MaxHealth => _MaxHealth;
		public float Health => _Health;
		public float NormalizedHealth => _NormalizedHealth;
	
		public bool IsAlive => !_IsDie;
		public bool IsDie => _IsDie;
		public bool Full => _Full;
		
		private float _NormalizedHealth;
		private bool _IsDie;
		private bool _Full;

		public event HealthValueChangeHandler OnChangedMaxHealth;
		public event HealthValueChangeHandler OnChangedHealth;
		public event HealthStateHandler OnFulledHealth;
		public event HealthStateHandler OnDead;
	
		public void Setup(float maxHealth, float currentHealth)
        {
			_MaxHealth = maxHealth;
			_Health = currentHealth;

			_NormalizedHealth = Mathf.Clamp01(_Health / _MaxHealth);

			_IsDie = _Health > 0f;

			_Full = _NormalizedHealth >= 1f;
		}

		public void ResetHealth()
		{
			_Health = _MaxHealth;
			_NormalizedHealth = 1f;
			_IsDie = false;
			_Full = true;

			OnChangeMaxHealth();
			OnChangeHealth();
		}

		public void SetMaxHealth(float newMaxHealth)
        {
			if (MaxHealth == newMaxHealth)
				return;

			float prevValue = MaxHealth;

			_MaxHealth = newMaxHealth;

			OnChangeMaxHealth(prevValue);

			CheckHealthState();
        }
	
		public float TakeDamage(float damage)
		{
			if(_IsDie)
				return -1f;
				
			if(damage <= 0f)
				return -1f;

			float prevValue = _Health;

			_Health -= damage;

			_Health = Mathf.Clamp(_Health, 0f, MaxHealth);
		
			_NormalizedHealth = Mathf.Clamp01(_Health / _MaxHealth);
			
			OnChangeHealth(prevValue);
			
			CheckHealthState();

			return prevValue - _Health;
		}
	
		public float TakeHeal(float heal)
		{
			if(_Full)
				return -1f;
				
			if(heal <= 0f)
				return -1f;

			float prevValue = _Health;
			_Health += heal;

			_Health = Mathf.Clamp(_Health , 0, MaxHealth);
			
			_NormalizedHealth = Mathf.Clamp01(_Health / _MaxHealth);

			OnChangeHealth(prevValue);
			
			CheckHealthState();

			return _Health - prevValue;
		}

		public void TakeHealFromPercent(float percent)
        {
			TakeHeal(MaxHealth * percent);
		}
		
		private void CheckHealthState()
		{
			if(_IsDie)
			{
				if(_NormalizedHealth > 0)
				{
					_IsDie = false;
				}
			}
			else
			{
				if(_NormalizedHealth <= 0f)
				{
					_IsDie = true;
					
					OnDie();
				}
			}
			
			if(_Full)
			{
				if(NormalizedHealth < 1f)
				{
					_Full = false;
				}
			}
			else
			{
				if(NormalizedHealth >= 1f)
				{
					_Full = true;
					
					OnFullHealth();
				}
			}
		}
		
		private void OnChangeHealth(float value = 0f)
		{
			OnChangedHealth?.Invoke(this, Health, value);
		}
		private void OnChangeMaxHealth(float value = 0f)
        {
			OnChangedMaxHealth?.Invoke(this, MaxHealth, value);
		}
		private void OnDie()
		{
			OnDead?.Invoke(this);
		}
		private void OnFullHealth()
		{
			OnFulledHealth?.Invoke(this);
		}
	
	}

}
