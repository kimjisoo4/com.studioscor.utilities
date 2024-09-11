using UnityEngine;

namespace StudioScor.Utilities
{
    public class DamageableComponent : BaseMonoBehaviour, IDamageableSystem
    {
		[Header(" [ Damageable Component ] ")]
		[Header(" Auto Playing ")]
		[SerializeField] private bool _autoPlaying = true;

		private bool _isPlaying;

		public bool IsPlaying => _isPlaying;

		public event TakeDamageEventHandler OnTakeAnyDamage;
		public event TakeDamageEventHandler OnTakePointDamage;
		public event TakeDamageEventHandler OnAfterDamage;


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

			DamageInfoData damageInfoData = DamageInfoData.Get(damage, damageType, damageCauser, instigator);

            Invoke_TakeAnyDamage(damageInfoData);
            Invoke_AfterDamage(damageInfoData);

			float appliedDamage = damageInfoData.AppliedDamage;
            damageInfoData.Release();

			return appliedDamage;

        }
		public float ApplyPointDamage(float damage, DamageType damageType, 
								Vector3 hitPoint, Vector3 hitNormal, Transform hitTransform,
								Vector3 direction, GameObject damageCauser, GameObject instigator)
        {
			if (!_isPlaying)
				return -1f;


            var damageInfoData = DamageInfoData.Get(damage, damageType, hitPoint, hitNormal, hitTransform, direction, damageCauser, instigator);

			Invoke_TakeAnyDamage(damageInfoData);
			Invoke_TakePointDamage(damageInfoData);
			Invoke_AfterDamage(damageInfoData);

			float appliedDamage = damageInfoData.Damage;
			damageInfoData.Release();

            return appliedDamage;
        }

		private void Invoke_TakeAnyDamage(DamageInfoData damageInfoData)
		{
			Log($"{nameof(OnTakeAnyDamage)} - [ Damage : {damageInfoData.Damage}  DamageCauser : {damageInfoData.Causer} | Instigator : {damageInfoData.Instigator}]");

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
