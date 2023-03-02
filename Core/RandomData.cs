using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace StudioScor.Utilities
{


    public class RandomData<T> : BaseScriptableObject, ISerializationCallbackReceiver
    {
        [System.Serializable]
        public struct FRandomData
        {
            public string Name;
            public T Data;
            public float Chance;
            [SReadOnly] public float Percent;
            [SReadOnly] public float MinRange;
            [SReadOnly] public float MaxRange;

            public FRandomData(string name, T data, float chance, float percent, float minRange, float maxRange)
            {
                Name = name;
                Data = data;
                Chance = chance;
                Percent = percent;
                MinRange = minRange;
                MaxRange = maxRange;
            }
        }

        [SerializeField] private FRandomData[] _InitialData;
        [SerializeField] private List<FRandomData> _RuntimeData;
        
        private float _InitialMaxChance = 0f;
        private float _RuntimeMaxChance = 0f;

        public int RemainCount => _RuntimeData.Count;
        public bool HasData => _RuntimeData.Count > 0;


        public void OnBeforeSerialize()
        {
        }
        public void OnAfterDeserialize()
        {
            UpdateData();

            OnReset();
        }

        [ContextMenu("Update Data")]
        protected void UpdateData()
        {
            float chance = 0f;

            for (int i = 0; i < _InitialData.Length; i++)
            {
                var data = _InitialData[i];

                data.MinRange = chance;
                data.MaxRange = data.MinRange + _InitialData[i].Chance;

                _InitialData[i] = data;

                chance += data.Chance;
            }

            _InitialMaxChance = chance;

            for (int i = 0; i < _InitialData.Length; i++)
            {
                var data = _InitialData[i];

                data.Percent = data.Chance.SafeDivide(_InitialMaxChance) * 100f;

                _InitialData[i] = data;
            }
        }

        protected void UpdateRuntimeData()
        {
            Log(" Update Runtime Data ");

            float chance = 0f;

            for (int i = 0; i < _RuntimeData.Count; i++)
            {
                var data = _RuntimeData[i];

                data.MinRange = chance;
                data.MaxRange = data.MinRange + _RuntimeData[i].Chance;

                _RuntimeData[i] = data;

                chance += data.Chance;
            }

            _RuntimeMaxChance = chance;

            for (int i = 0; i < _RuntimeData.Count; i++)
            {
                var data = _RuntimeData[i];

                data.Percent = data.Chance.SafeDivide(_RuntimeMaxChance) * 100f;

                _RuntimeData[i] = data;
            }
        }

        protected override void OnReset()
        {
            _RuntimeData = _InitialData.ToList();
            _RuntimeMaxChance = _InitialMaxChance;
        }


        public bool TryGetRandomData(out T data, bool removeData = false)
        {
            Log(" Try Get Random Data ");

            bool hasData = false;
            int count = 0;

            data = default;

            if (HasData)
            {
                float rand = UnityEngine.Random.Range(0f, _RuntimeMaxChance);
                Log($" Rand Value - {rand:N1} ");

                for (int i = 0; i < _RuntimeData.Count; i++)
                {
                    Log($" Check Data Chance Value - InRange [{_RuntimeData[i].Name}] ?");

                    if (rand.InRange(_RuntimeData[i].MinRange, _RuntimeData[i].MaxRange, true, false))
                    {
                        data = _RuntimeData[i].Data;

                        count = i;
                        hasData = true;

                        Log($" Find Random Data - {_RuntimeData[i].Name} ");
                        break;
                    }
                }
            }

            if (hasData && removeData)
            {
                _RuntimeData.RemoveAt(count);

                UpdateRuntimeData();
            }
            
            return hasData;
        }
    }
}