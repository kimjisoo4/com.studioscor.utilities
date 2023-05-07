using UnityEngine;

namespace StudioScor.Utilities
{
    public struct FDamageInfo
	{
		public float Damage;
		public DamageType Type;

		public Vector3 HitPoint;
		public Vector3 HitNormal;
		public Transform HitTransform;
		public Vector3 Direction;

		public GameObject Causer;
		public GameObject Instigator;

        public FDamageInfo(float damage, DamageType type, GameObject causer, GameObject instigator) : this()
        {
            Damage = damage;
            Type = type;
            Causer = causer;
            Instigator = instigator;
        }

        public FDamageInfo(float damage, DamageType type, Vector3 hitPoint, Vector3 hitNormal, Transform hitTransform,
						Vector3 direction, GameObject causer, GameObject instigator)
        {
            Damage = damage;
            Type = type;
            HitPoint = hitPoint;
            HitNormal = hitNormal;
            HitTransform = hitTransform;
            Causer = causer;
            Instigator = instigator;
            Direction = direction;
        }
    }

}
