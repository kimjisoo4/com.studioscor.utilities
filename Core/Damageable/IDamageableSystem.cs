using UnityEngine;

namespace StudioScor.Utilities
{
	public delegate void TakeDamageEventHandler(IDamageableSystem damageable, DamageInfoData damageInfo);
	public interface IDamageableSystem
    {
		public void OnDamageable();
		public void EndDamageable();
		public float ApplyDamage(float damage, DamageType damageType, GameObject damageCauser, GameObject instigator = null);
		public float ApplyPointDamage(float damage, DamageType damageType,
								Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider,
								Vector3 direction, GameObject damageCauser, GameObject instigator);

		public DamageInfoData LastDamageInfo { get; }

		public GameObject gameObject { get; }
		public Transform transform { get; }

		public event TakeDamageEventHandler OnTakeAnyDamage;
		public event TakeDamageEventHandler OnTakePointDamage;

		public event TakeDamageEventHandler OnAfterDamage;
	}

}
