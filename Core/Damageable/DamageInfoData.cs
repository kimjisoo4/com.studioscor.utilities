using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class DamageInfoData
	{
		public float Damage { get; protected set; }
		public DamageType Type { get; protected set; }

        public Vector3 HitPoint { get; protected set; }
        public Vector3 HitNormal { get; protected set; }
        public Collider HitCollider { get; protected set; }
        public Vector3 Direction { get; protected set; }

        public GameObject Causer { get; protected set; }
        public GameObject Instigator { get; protected set; }

        public float AppliedDamage { get; set; }

        public void Setup(float damage, DamageType type, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider,
                        Vector3 direction, GameObject causer, GameObject instigator)
        {
            Damage = damage;
            AppliedDamage = Damage;
            Type = type;
            HitPoint = hitPoint;
            HitNormal = hitNormal;
            HitCollider = hitCollider;
            Direction = direction;
            Causer = causer;
            Instigator = instigator;
        }

        public void Setup(float damage, DamageType type, GameObject causer, GameObject instigator)
        {
            Setup(damage, type, default, default, null, default, causer, instigator);
        }
        public DamageInfoData()
        {

        }
        public DamageInfoData(float damage, DamageType type, GameObject causer, GameObject instigator)
        {
            Setup(damage, type, causer, instigator);
        }

        public DamageInfoData(float damage, DamageType type, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider,
						Vector3 direction, GameObject causer, GameObject instigator)
        {
            Setup(damage, type, hitPoint, hitNormal, hitCollider, direction, causer, instigator);
        }
    }

}
