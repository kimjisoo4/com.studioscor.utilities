using UnityEngine;
using System.Linq;
using System.Collections.Generic;


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

        [SerializeField] private FRandomData[] initialData;
        [SerializeField][SReadOnly] private List<FRandomData> runtimeData;
        
        private float initialMaxChance = 0f;
        private float runtimeMaxChance = 0f;

        public int RemainCount => runtimeData.Count;
        public bool HasData => runtimeData.Count > 0;


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

            for (int i = 0; i < initialData.Length; i++)
            {
                var data = initialData[i];

                data.MinRange = chance;
                data.MaxRange = data.MinRange + initialData[i].Chance;

                initialData[i] = data;

                chance += data.Chance;
            }

            initialMaxChance = chance;

            for (int i = 0; i < initialData.Length; i++)
            {
                var data = initialData[i];

                data.Percent = data.Chance.SafeDivide(initialMaxChance) * 100f;

                initialData[i] = data;
            }
        }

        protected void UpdateRuntimeData()
        {
            Log(" Update Runtime Data ");

            float chance = 0f;

            for (int i = 0; i < runtimeData.Count; i++)
            {
                var data = runtimeData[i];

                data.MinRange = chance;
                data.MaxRange = data.MinRange + runtimeData[i].Chance;

                runtimeData[i] = data;

                chance += data.Chance;
            }

            runtimeMaxChance = chance;

            for (int i = 0; i < runtimeData.Count; i++)
            {
                var data = runtimeData[i];

                data.Percent = data.Chance.SafeDivide(runtimeMaxChance) * 100f;

                runtimeData[i] = data;
            }
        }

        protected override void OnReset()
        {
            runtimeData = initialData.ToList();
            runtimeMaxChance = initialMaxChance;
        }

        public void TryReturnData(T returnData)
        {
            foreach (var data in runtimeData)
            {
                if (data.Data.Equals(returnData))
                {
                    return;
                }
            }


            foreach (var data in initialData)
            {
                if (data.Data.Equals(returnData))
                {
                    runtimeData.Add(data);

                    UpdateRuntimeData();

                    return;
                }
            }
        }

        public void RemoveData(T removeData)
        {
            if (!HasData)
                return;

            for(int i = runtimeData.LastIndex(); i >= 0; i--)
            {
                if (runtimeData[i].Data.Equals(removeData))
                {
                    runtimeData.RemoveAt(i);

                    UpdateRuntimeData();

                    return;
                }
            }
        }

        public bool TryGetRandomData(out T data)
        {
            Log(" Try Get Random Data ");

            bool hasData = false;

            data = default;

            if (HasData)
            {
                float rand = UnityEngine.Random.Range(0f, runtimeMaxChance);
                Log($" Rand Value - {rand:N1} ");

                for (int i = 0; i < runtimeData.Count; i++)
                {
                    Log($" Check Data Chance Value - InRange [{runtimeData[i].Name}] ?");

                    if (rand.InRange(runtimeData[i].MinRange, runtimeData[i].MaxRange, true, false))
                    {
                        data = runtimeData[i].Data;

                        hasData = true;

                        Log($" Find Random Data - {runtimeData[i].Name} ");
                        break;
                    }
                }
            }

            return hasData;
        }
    }
}