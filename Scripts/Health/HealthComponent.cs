using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace KimScor.Utilities
{
	public interface IDamageable
    {
		public bool TryTakeDamage(float damage);
		public bool TryTakeHeal(float heal);
		public bool CanTakeDamage(float damage);
		public bool CanTakeHeal(float heal);

		public float OnTakeDamage(float damage);
		public float OnTakeHeal(float heal);
    }

    public class HealthComponent : MonoBehaviour, IDamageable
	{
		public enum ESetup
		{
			Awake,
			Start,
			Enable,
			Manual,
		}

		public delegate void HealthStateHandler(HealthComponent health);
		public delegate void HealthRegenStaetHandler(HealthComponent health, bool useRegeneration);
		public delegate void HealthValueChangeHandler(HealthComponent health, float currentValue, float prevValue);

		[Header(" [ Health ] ")]
		[SerializeField] private ESetup _Setup;
		[SerializeField] private float _MaxHealth = 100f;
		[SerializeField] private float _Health = 100f;

		[Header(" [ Regeneration Health ] ")]
		[SerializeField] private bool _UseRegeneration = true;
		[SerializeField] private float _RegenerationValue = 1f;

		[Header(" [ Use Floating Text ] ")]
		[SerializeField] private bool _UseFloatingText;
		[SerializeField] private Vector3 _FloatingTextOffset;

		[Header(" [ Unity Events ] ")]
		[SerializeField] private bool _UseUnityEvent = false;
		[SerializeField] private AddUnityEvents _AddUnityEvents;

		[Header(" [ Debug ] ")]
		[SerializeField] private bool _UseDebug;


		public float MaxHealth => _MaxHealth;
		public float Health => _Health;
		public float NormalizedHealth => _NormalizedHealth;
		public bool UseRegeneration => _UseRegeneration;
		public float RegenerationValue => _RegenerationValue;
		public bool IsAlive => !_IsDie;
		public bool IsDie => _IsDie;
		public bool Full => _Full;

		private float _NormalizedHealth;
		private bool _IsDie;
		private bool _Full;

		public event HealthValueChangeHandler OnChangedMaxHealth;
		public event HealthValueChangeHandler OnChangedHealth;
		public event HealthValueChangeHandler OnChangedRegenerationHealth;
		public event HealthRegenStaetHandler OnChangedRegenerationHealthState;
		public event HealthStateHandler OnFulledHealth;
		public event HealthStateHandler OnDead;
		public event HealthStateHandler OnResetedHealth;


		[System.Serializable]
		public class AddUnityEvents
        {
			public UnityEvent<HealthComponent, float, float> OnChangedMaxHealthEvent;
			public UnityEvent<HealthComponent, float, float> OnChangedHealthEvent;
			public UnityEvent<HealthComponent, float, float> OnChangedRegenerationHealthEvent;
			public UnityEvent<HealthComponent, bool> OnChangedRegenerationStateEvent;
			public UnityEvent<HealthComponent> OnFulledHeatlhEvent;
			public UnityEvent<HealthComponent> OnDeadEvent;

			private HealthComponent _HealthComponent;
			public void Setup(HealthComponent healthComponent)
			{
				if(_HealthComponent)
                {
                    _HealthComponent.OnChangedHealth -= HealthComponent_OnChangedHealth;
                    _HealthComponent.OnChangedMaxHealth -= HealthComponent_OnChangedMaxHealth;
                    _HealthComponent.OnChangedRegenerationHealth -= HealthComponent_OnChangedRegenerationHealth;
                    _HealthComponent.OnChangedRegenerationHealthState -= HealthComponent_OnChangedRegenerationHealthState;
                    _HealthComponent.OnFulledHealth -= HealthComponent_OnFulledHealth;
                    _HealthComponent.OnDead -= HealthComponent_OnDead;
				}

				_HealthComponent = healthComponent;

				if(_HealthComponent)
                {
					_HealthComponent.OnChangedHealth += HealthComponent_OnChangedHealth;
					_HealthComponent.OnChangedMaxHealth += HealthComponent_OnChangedMaxHealth;
					_HealthComponent.OnChangedRegenerationHealth += HealthComponent_OnChangedRegenerationHealth;
					_HealthComponent.OnChangedRegenerationHealthState += HealthComponent_OnChangedRegenerationHealthState;
					_HealthComponent.OnFulledHealth += HealthComponent_OnFulledHealth;
					_HealthComponent.OnDead += HealthComponent_OnDead;
				}
			}

            private void HealthComponent_OnChangedRegenerationHealth(HealthComponent health, float currentValue, float prevValue)
            {
				OnChangedRegenerationHealthEvent?.Invoke(health, currentValue, prevValue);

			}

            private void HealthComponent_OnChangedRegenerationHealthState(HealthComponent health, bool useRegeneration)
            {
				OnChangedRegenerationStateEvent?.Invoke(health, useRegeneration);

			}

            private void HealthComponent_OnDead(HealthComponent health)
            {
				OnDeadEvent?.Invoke(health);
            }

            private void HealthComponent_OnFulledHealth(HealthComponent health)
            {
				OnFulledHeatlhEvent?.Invoke(health);
            }

            private void HealthComponent_OnChangedMaxHealth(HealthComponent health, float currentValue, float prevValue)
            {
				OnChangedHealthEvent?.Invoke(health, currentValue, prevValue);
			}

            private void HealthComponent_OnChangedHealth(HealthComponent health, float currentValue, float prevValue)
            {
				OnChangedMaxHealthEvent?.Invoke(health, currentValue, prevValue);
			}
        }


#if UNITY_EDITOR

		private void Log(object message)
		{
			if (_UseDebug)
				Utilities.Debug.Log("HealthCompoenent [" + name + " ] : " + message, this);
		}
#endif


		protected virtual void OnEnable()
        {
			if (_Setup.Equals(ESetup.Enable))
				ResetHealth();

        }
        protected virtual void Awake()
        {
			if (_UseUnityEvent)
				_AddUnityEvents.Setup(this);

			if (_Setup.Equals(ESetup.Awake))
				ResetHealth();
		}
		protected virtual void Start()
        {
			if (_Setup.Equals(ESetup.Start))
				ResetHealth();
        }
        protected virtual void Update()
        {
			if (!_UseRegeneration || IsDie || Full)
				return;

			float deltaTime = Time.deltaTime;

			OnTakeHeal(_RegenerationValue * deltaTime);
        }

        public void Setup(float maxHealth, float currentHealth, float regenerationValue = 0f)
        {
			_MaxHealth = maxHealth;
			_Health = currentHealth;
			_RegenerationValue = regenerationValue;

			_NormalizedHealth = Mathf.Clamp01(_Health / _MaxHealth);

			_IsDie = _Health > 0f;
			_Full = _NormalizedHealth >= 1f;
			_UseRegeneration = regenerationValue > 0;
		}

		public void ResetHealth()
		{
			_Health = _MaxHealth;
			_NormalizedHealth = 1f;
			_IsDie = false;
			_Full = true;

			bool useRegeneration = _RegenerationValue > 0f;

			SetUseRegeneration(useRegeneration);

			OnResetHealth();
			OnChangeMaxHealth();
			OnChangeHealth();
		}

		public bool TryTakeDamage(float damage)
		{
			if (!CanTakeDamage(damage))
				return false;

			OnTakeDamage(damage);

			return true;
		}

		public bool TryTakeHeal(float heal)
		{
			if (!CanTakeHeal(heal))
				return false;

			OnTakeHeal(heal);

			return true;
		}

		public bool CanTakeDamage(float damage)
		{
			return IsAlive || damage > 0f;
		}

		public bool CanTakeHeal(float heal)
		{
			return IsAlive || heal > 0;
		}

		public float OnTakeDamage(float damage)
		{
			if (_IsDie)
				return -1f;

			if (damage <= 0f)
				return -1f;

			float prevValue = _Health;

			_Health -= damage;

			_Health = Mathf.Clamp(_Health, 0f, MaxHealth);

			_NormalizedHealth = Mathf.Clamp01(_Health / _MaxHealth);

			OnChangeHealth(prevValue);

			CheckHealthState();

			if (_UseFloatingText)
			{
				var manager = FloatingDamageManager.Instance;

				if (manager)
				{
					manager.SpawnFloatingDamage(transform.position + _FloatingTextOffset, damage);
				}
			}

			return prevValue - _Health;
		}

		public float OnTakeHeal(float heal)
		{
			if (_Full)
				return -1f;

			if (heal <= 0f)
				return -1f;

			float prevValue = _Health;
			_Health += heal;

			_Health = Mathf.Clamp(_Health, 0, MaxHealth);

			_NormalizedHealth = Mathf.Clamp01(_Health / _MaxHealth);

			OnChangeHealth(prevValue);

			CheckHealthState();

			return _Health - prevValue;
		}

        #region Regeneration
        public void SetUseRegeneration(bool useRegeneration)
        {
			if (_UseRegeneration == useRegeneration)
				return;

			_UseRegeneration = useRegeneration;

			OnChangeRegenerationState();

		}
		public void SetRegenerationValue(float newRegenerationValue)
        {
			if (_RegenerationValue == newRegenerationValue)
				return;

			float prevValue = _RegenerationValue;

			_RegenerationValue = newRegenerationValue;

			OnChangeRegenerationHealth(prevValue);
		}
        #endregion

        public void SetMaxHealth(float newMaxHealth)
        {
			if (MaxHealth == newMaxHealth)
				return;

			float prevValue = MaxHealth;

			_MaxHealth = newMaxHealth;

			OnChangeMaxHealth(prevValue);

			CheckHealthState();
        }

		public void SetHealth(float newHealth)
		{
			if (Health == newHealth)
				return;

			float prevValue = Health;

			_Health = Mathf.Clamp(newHealth, 0, _MaxHealth);

			OnChangeHealth(prevValue);
		}
		public void SetHealthFromPercent(float newHealth)
		{
			SetHealth(Health * newHealth);
		}


		public float TakeDamageFromPercent(float percent)
        {
			return OnTakeDamage(MaxHealth * percent);
        }

		public float TakeHealFromPercent(float percent)
        {
			return OnTakeHeal(MaxHealth * percent);
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
		
		private void OnChangeHealth(float prevValue = 0f)
		{
			Log("On Changed Health - Prev : " + prevValue + "|| Current : " + Health);

			OnChangedHealth?.Invoke(this, Health, prevValue);
		}
		
		private void OnChangeMaxHealth(float prevValue = 0f)
        {
			Log("On Changed Max Health - Prev : " + prevValue + "|| Current : " + MaxHealth);

			OnChangedMaxHealth?.Invoke(this, MaxHealth, prevValue);

		}
		private void OnChangeRegenerationHealth(float prevValue = 0f)
		{
			Log("On Changed Regeneration Health - Prev : " + prevValue + "|| Current : " + _RegenerationValue);

			OnChangedRegenerationHealth?.Invoke(this, _RegenerationValue, prevValue);

		}
		private void OnChangeRegenerationState()
		{
			Log("On Regeneration State -" + _UseRegeneration);

			OnChangedRegenerationHealthState?.Invoke(this, _UseRegeneration);
		}
		private void OnDie()
		{
			Log("On Dead");

			OnDead?.Invoke(this);
		}
		private void OnResetHealth()
        {
			Log("On Reset");

			OnResetedHealth?.Invoke(this);
        }
		private void OnFullHealth()
		{
			Log("On Fulled Health");

			OnFulledHealth?.Invoke(this);
		}

        
	}

}
