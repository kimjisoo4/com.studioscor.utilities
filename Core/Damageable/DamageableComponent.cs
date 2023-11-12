using UnityEngine;

namespace StudioScor.Utilities
{
    public class DamageableComponent : BaseMonoBehaviour, IDamageableSystem
    {
		[Header(" [ Damageable Component ] ")]
		[Header(" [ Auto Playing ] ")]
		[SerializeField] private bool autoPlaying = true;

		private FDamageInfo lastDamageInfo;
		private bool isPlaying;


		public event TakeDamageEventHandler OnTakeAnyDamage;
		public event TakeDamageEventHandler OnTakePointDamage;
		public bool IsPlaying => isPlaying;

		public FDamageInfo LastDamageInfo => lastDamageInfo;

        private void OnEnable()
        {
            if (autoPlaying)
                OnDamageable();
        }
		private void OnDisable()
        {
            if (autoPlaying)
                EndDamageable();
        }

		public void OnDamageable()
        {
			if (isPlaying)
				return;

			isPlaying = true;
		}

		public void EndDamageable()
        {
			if (!isPlaying)
				return;

			isPlaying = false;

		}
		public float ApplyDamage(float damage, DamageType damageType, GameObject damageCauser, GameObject instigator = null)
		{
			if (!isPlaying)
				return -1f;

            lastDamageInfo = new FDamageInfo(damage, damageType, damageCauser, instigator);

			Callback_TakeAnyDamage();

			return damage;
		}
		public float ApplyPointDamage(float damage, DamageType damageType, 
								Vector3 hitPoint, Vector3 hitNormal, Transform hitTransform,
								Vector3 direction, GameObject damageCauser, GameObject instigator)
        {
			if (!isPlaying)
				return -1f;

            lastDamageInfo = new FDamageInfo(damage, damageType, hitPoint, hitNormal, hitTransform, direction, damageCauser, instigator);

			Callback_TakeAnyDamage();
			Callback_TakePointDamage();

			return damage;
		}

		private void Callback_TakeAnyDamage()
		{
			Log($"Take Any Damage - [ Damage : {lastDamageInfo.Damage} | DamageCauser : {lastDamageInfo.Causer} | Instigator : {lastDamageInfo.Instigator}]");

			OnTakeAnyDamage?.Invoke(this, lastDamageInfo);
		}
		private void Callback_TakePointDamage()
        {
			Log($"Take Point Damage - [ Damage : {lastDamageInfo.Damage} | DamageCauser : {lastDamageInfo.Causer} | Instigator : {lastDamageInfo.Instigator}]");

			OnTakePointDamage?.Invoke(this, lastDamageInfo);
        }
    }

}
