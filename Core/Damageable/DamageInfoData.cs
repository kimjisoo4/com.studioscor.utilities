using UnityEngine;
using UnityEngine.Pool;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class DamageInfoData
	{
		public float Damage { get; protected set; }
		public DamageType Type { get; protected set; }
        public Vector3 HitPoint { get; protected set; }
        public Vector3 HitNormal { get; protected set; }
        public Transform HitTransform { get; protected set; }
        public Vector3 AttackDirection { get; protected set; }
        public GameObject Causer { get; protected set; }
        public GameObject Instigator { get; protected set; }
        public float AppliedDamage { get; set; }


        private static ObjectPool<DamageInfoData> _pool;

        public DamageInfoData() { }
        
        public static DamageInfoData Get(float damage, DamageType type, Vector3 hitPoint, Vector3 hitNormal, Transform hitTransform, Vector3 direction, GameObject causer, GameObject instigator)
        {
            if(_pool is null)
            {
                _pool = new ObjectPool<DamageInfoData>(Create);
            }

            var data = _pool.Get();

            data.Damage = damage;
            data.AppliedDamage = damage;
            data.Type = type;
            data.HitPoint = hitPoint;
            data.HitNormal = hitNormal;
            data.HitTransform = hitTransform;
            data.AttackDirection = direction;
            data.Causer = causer;
            data.Instigator = instigator;

            return data;
        }
        public static DamageInfoData Get(float damage, DamageType type, GameObject causer, GameObject instigator)
        {
            return Get(damage, type, default, default, null, default, causer, instigator);
        }

        public void Release()
        {
            _pool.Release(this);
        }

        private static DamageInfoData Create()
        {
#if UNITY_EDITOR
            if(_pool.CountActive > 50)
            {
                Debug.LogWarning($"[{nameof(DamageInfoData)}] Damage Info data needs to be released.");
            }
#endif

            return new DamageInfoData();
        }

        public override string ToString()
        {
            return $"[{nameof(DamageInfoData)}] Damage - {Damage} :: DamageType - {Type} :: Hit Point - {HitPoint:f2} :: Hit Normal - {HitNormal:f2} :: Hit Transform - {HitTransform} :: AttackDirection - {AttackDirection:f2} :: DamageCauser - {Causer} :: Instigator - {Instigator}";
        }
    }

}
