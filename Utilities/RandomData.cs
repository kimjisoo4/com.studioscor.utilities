using UnityEngine;
using System.Linq;

namespace StudioScor.Utilities
{
    public class RandomData<T> : ScriptableObject, ISerializationCallbackReceiver
    {
        [System.Serializable]
        public struct FRandomData
        {
#if UNITY_EDITOR
            [SerializeField] private string _Name;
#endif
            public T Data;
            public float Chance;

            public float ChanceMinRange;
            public float ChanceMaxRange;

            public float CurrentPercent;

            public bool InRange(float value)
            {
                return value >= ChanceMinRange && value < ChanceMaxRange;
            }
            public void SetChanceRange(ref float prevChance)
            {
                ChanceMinRange = prevChance;

                prevChance += Chance;

                ChanceMaxRange = prevChance;
            }
            public void SetCurrentPercent(float maxChance)
            {
                if (maxChance == 0)
                    return;

                CurrentPercent = Chance / maxChance;
            }
        }

        [SerializeField] private FRandomData[] _Datas;
        private float _MaxChance = 0f;

        public T GetRandomData()
        {
            if (_Datas.Length == 0)
                return default;

            float rand = Random.Range(0f, _MaxChance);

            for (int i = 0; i < _Datas.Length - 1; i++)
            {
                if (_Datas[i].InRange(rand))
                {
                    return _Datas[i].Data;
                }
            }

            return _Datas.Last().Data;
        }

        public void OnBeforeSerialize()
        {
        }
        public void OnAfterDeserialize()
        {
            float chance = 0f;

            for (int i = 0; i < _Datas.Length; i++)
            {
                _Datas[i].SetChanceRange(ref chance);
            }

            _MaxChance = chance;

            for (int i = 0; i < _Datas.Length; i++)
            {
                _Datas[i].SetCurrentPercent(_MaxChance);
            }
        }
    }
}