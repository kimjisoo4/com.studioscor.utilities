using UnityEngine;

namespace StudioScor.Utilities
{
	public delegate void TakeDamageEventHandler(IDamageable damageable, FDamageInfo damageInfo);
	public interface IDamageable
    {
		public float ApplyDamage(float damage, DamageType damageType, GameObject damageCauser, GameObject instigator = null);
		public float ApplyPointDamage(float damage, DamageType damageType,
								Vector3 hitPoint, Vector3 hitNormal, Transform hitTransform,
								Vector3 direction, GameObject damageCauser, GameObject instigator);

		public event TakeDamageEventHandler TakeAnyDamage;
		public event TakeDamageEventHandler TakePointDamage;
	}

}
