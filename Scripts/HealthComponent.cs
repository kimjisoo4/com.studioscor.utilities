using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimScor.Utilities
{
	
	public class HealthComponent : MonoBehaviour
	{
		public delegate void HealthStateHandler(HealthComponent health);
		public delegate void HealthValueChangeHandler (HealthComponent health, float changeValue);
		
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
		
		public event HealthValueChangeHandler OnTakeDamaged;
		public event HealthValueChangeHandler OnTakeHealed;
		public event HealthStateHandler OnFulledHealth;
		public event HealthStateHandler OnDead;
	
		public void ResetHealthBar()
		{
			_Health = _MaxHealth;
			_NormalizedHealth = 1f;
			_Full = true;
		}
	
		public void TakeDamage(float damage)
		{
			if(_IsDie)
				return;
				
			if(damage <= 0f)
				return;
				
			_Health -= damage;
		
			Mathf.Clamp(_Health, 0f, MaxHealth);
		
			_NormalizedHealth = Mathf.Clamp01(_Health / _MaxHealth);
			
			OnTakeDamage(damage);
			
			CheckHealthState();
		}
	
		public void TakeHeal(float heal)
		{
			if(_Full)
				return;
				
			if(heal <= 0f)
				return;
			
			_Health += heal;
			
			Mathf.Clamp(_Health , 0, MaxHealth);
			
			_NormalizedHealth = Mathf.Clamp01(_Health / _MaxHealth);
			
			OnTakeHeal(heal);
			
			CheckHealthState();
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
		
		private void OnTakeDamage(float value)
		{
			OnTakeDamaged?.Invoke(this, value);
		}
		private void OnTakeHeal(float value)
		{
			OnTakeHealed?.Invoke(this, value);	
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
