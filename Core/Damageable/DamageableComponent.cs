using UnityEngine;

namespace StudioScor.Utilities
{
    public class DamageableComponent : BaseMonoBehaviour, IDamageable
    {
		[Header(" [ Damageable Component ] ")]
		[Header(" [ Auto Playing ] ")]
		[SerializeField] private bool _AutoPlaying = true;

		private bool _IsPlaying;

		public event TakeDamageEventHandler TakeAnyDamage;
		public event TakeDamageEventHandler TakePointDamage;
		public bool IsPlaying => _IsPlaying;

        private void OnEnable()
        {
			if (_AutoPlaying)
				OnDamageable();
        }
		private void OnDisable()
		{
			if (_AutoPlaying)
				EndDamageable();
		}

		private void OnDamageable()
        {
			if (_IsPlaying)
				return;

			_IsPlaying = true;
		}

		private void EndDamageable()
        {
			if (!_IsPlaying)
				return;

			_IsPlaying = false;

		}
		public float ApplyDamage(float damage, DamageType damageType, GameObject damageCauser, GameObject instigator = null)
		{
			if (!_IsPlaying)
				return -1f;

			FDamageInfo damageInfo = new FDamageInfo(damage, damageType, damageCauser, instigator);

			Callback_TakeAnyDamage(damageInfo);

			return damage;
		}
		public float ApplyPointDamage(float damage, DamageType damageType, 
								Vector3 hitPoint, Vector3 hitNormal, Transform hitTransform,
								Vector3 direction, GameObject damageCauser, GameObject instigator)
        {
			if (!_IsPlaying)
				return -1f;

			FDamageInfo damageInfo = new FDamageInfo(damage, damageType, hitPoint, hitNormal, hitTransform, direction, damageCauser, instigator);

			Callback_TakeAnyDamage(damageInfo);
			Callback_TakePointDamage(damageInfo);

			return damage;
		}

		private void Callback_TakeAnyDamage(FDamageInfo damageInfo)
		{
			Log($"Take Any Damage - [ Damage : {damageInfo.Damage} | DamageCauser : {damageInfo.Causer} | Instigator : {damageInfo.Instigator}]");

			TakeAnyDamage?.Invoke(this, damageInfo);
		}
		private void Callback_TakePointDamage(FDamageInfo damageInfo)
        {
			Log($"Take Point Damage - [ Damage : {damageInfo.Damage} | DamageCauser : {damageInfo.Causer} | Instigator : {damageInfo.Instigator}]");

			TakePointDamage?.Invoke(this, damageInfo);
        }
    }

}
