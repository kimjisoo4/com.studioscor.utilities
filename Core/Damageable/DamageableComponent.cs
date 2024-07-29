using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace StudioScor.Utilities
{
    public class DamageableComponent : BaseMonoBehaviour, IDamageableSystem
    {
		[Header(" [ Damageable Component ] ")]
		[Header(" [ Auto Playing ] ")]
		[SerializeField] private bool _autoPlaying = true;

		private DamageInfoData _lastDamageInfo;
		private bool _isPlaying;
		private readonly Queue<DamageInfoData> _damageInfoDataPool = new();

		public event TakeDamageEventHandler OnTakeAnyDamage;
		public event TakeDamageEventHandler OnTakePointDamage;
		public event TakeDamageEventHandler OnAfterDamage;
		public bool IsPlaying => _isPlaying;
		public DamageInfoData LastDamageInfo => _lastDamageInfo;


        private void OnEnable()
        {
            if (_autoPlaying)
                OnDamageable();
        }
		private void OnDisable()
        {
            if (_autoPlaying)
                EndDamageable();
        }

		public void OnDamageable()
        {
			if (_isPlaying)
				return;

			_isPlaying = true;
		}

		public void EndDamageable()
        {
			if (!_isPlaying)
				return;

			_isPlaying = false;

		}
		public float ApplyDamage(float damage, DamageType damageType, GameObject damageCauser, GameObject instigator = null)
		{
			if (!_isPlaying)
				return -1f;

			DamageInfoData damageInfoData;

			if(_damageInfoDataPool.Count == 0)
			{
                damageInfoData = new DamageInfoData();
            }
			else
			{
				damageInfoData = _damageInfoDataPool.Dequeue();

            }

			damageInfoData.Setup(damage, damageType, damageCauser, instigator);
            _lastDamageInfo = damageInfoData;

            Invoke_TakeAnyDamage(damageInfoData);
            Invoke_AfterDamage(damageInfoData);

            _damageInfoDataPool.Enqueue(damageInfoData);

            return damageInfoData.AppliedDamage;
		}
		public float ApplyPointDamage(float damage, DamageType damageType, 
								Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider,
								Vector3 direction, GameObject damageCauser, GameObject instigator)
        {
			if (!_isPlaying)
				return -1f;

            DamageInfoData damageInfoData;

            if (_damageInfoDataPool.Count == 0)
            {
                damageInfoData = new DamageInfoData();
            }
            else
            {
                damageInfoData = _damageInfoDataPool.Dequeue();

            }

            damageInfoData.Setup(damage, damageType, hitPoint, hitNormal, hitCollider, direction, damageCauser, instigator);
            _lastDamageInfo = damageInfoData;

			Invoke_TakeAnyDamage(damageInfoData);
			Invoke_TakePointDamage(damageInfoData);
			Invoke_AfterDamage(damageInfoData);
            _damageInfoDataPool.Enqueue(damageInfoData);

            return damageInfoData.AppliedDamage;
        }

		private void Invoke_TakeAnyDamage(DamageInfoData damageInfoData)
		{
			Log($"{nameof(OnTakeAnyDamage)} - [ Damage : {damageInfoData.Damage} | DamageCauser : {damageInfoData.Causer} | Instigator : {damageInfoData.Instigator}]");

			OnTakeAnyDamage?.Invoke(this, damageInfoData);
		}
		private void Invoke_TakePointDamage(DamageInfoData damageInfoData)
		{
			Log($"{nameof(OnTakePointDamage)} - [ Damage : {damageInfoData.Damage} | DamageCauser : {damageInfoData.Causer} | Instigator : {damageInfoData.Instigator}]");

			OnTakePointDamage?.Invoke(this, damageInfoData);
		}
        private void Invoke_AfterDamage(DamageInfoData damageInfoData)
        {
            Log($"{nameof(OnAfterDamage)} - [ Applied Damage : {damageInfoData.AppliedDamage} | DamageCauser : {damageInfoData.Causer} | Instigator : {damageInfoData.Instigator}]");

            OnAfterDamage?.Invoke(this, damageInfoData);
        }
    }

}
