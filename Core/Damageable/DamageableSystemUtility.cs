using UnityEngine;

namespace StudioScor.Utilities
{
    public static class DamageableSystemUtility
	{
		public static IDamageableSystem GetDamageableSystem(this GameObject gameObject)
		{
			return gameObject.GetComponent<IDamageableSystem>();
		}
		public static IDamageableSystem GetDamageableSystem(this Component component)
		{
			return component.gameObject.GetDamageableSystem();
        }

        public static bool TryGetDamageableSystem(this GameObject gameObject, out IDamageableSystem damageableSystem)
		{
			return gameObject.TryGetComponent(out damageableSystem);
		}
        public static bool TryGetDamageableSystem(this Component component, out IDamageableSystem damageableSystem)
        {
			return component.gameObject.TryGetDamageableSystem(out damageableSystem);
        }

		public static float ApplyDamage(this GameObject gameObject, float damage, DamageType damageType, GameObject damageCauser, GameObject instigator)
		{
			if(gameObject.TryGetDamageableSystem(out IDamageableSystem damageableSystem))
			{
				return damageableSystem.ApplyDamage(damage, damageType, damageCauser, instigator);
			}
			else
			{
				return -1f;
			}
		}
		public static float ApplyDamage(this Component component, float damage, DamageType damageType, GameObject damageCauser, GameObject instigator)
		{
			return component.gameObject.ApplyDamage(damage, damageType, damageCauser, instigator);
        }

        public static float ApplyPointDamage(this GameObject gameObject, float damage, DamageType damageType,
											 Vector3 hitPoint, Vector3 hitNormal, Transform hitTransform,
											 Vector3 direction, GameObject damageCauser, GameObject instigator)
		{
            if (gameObject.TryGetDamageableSystem(out IDamageableSystem damageableSystem))
            {
                return damageableSystem.ApplyPointDamage(damage, damageType, hitPoint, hitNormal, hitTransform, direction, damageCauser, instigator);
            }
            else
            {
                return -1f;
            }
        }
        public static float ApplyPointDamage(this Component component, float damage, DamageType damageType,
                                             Vector3 hitPoint, Vector3 hitNormal, Transform hitTransform,
                                             Vector3 direction, GameObject damageCauser, GameObject instigator)
		{
			return component.gameObject.ApplyPointDamage(damage, damageType, hitPoint, hitNormal, hitTransform, direction, damageCauser, instigator);
        }

    }

}
