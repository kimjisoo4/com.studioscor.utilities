using UnityEngine;
using System.Collections.Generic;

namespace StudioScor.StatSystem
{
    [System.Serializable]
    public struct FInitializationStat
    {
#if UNITY_EDITOR
        public string StatName;
#endif
        public StatTag StatTag;
        public float Value;
    }

    [CreateAssetMenu(fileName = "InitializationStats_", menuName = "Stat/Initialization")]
	public class InitializationStats : ScriptableObject
    {
        [Header("[Stat]")]
        [SerializeField] private FInitializationStat[] _Stats;
        public IReadOnlyCollection<FInitializationStat> Stats => _Stats;

#if UNITY_EDITOR
        private void OnValidate()
        {
            for(int i = 0; i < Stats.Count; i++)
            {
                if (_Stats[i].StatTag == null)
                    return;

                _Stats[i].StatName = _Stats[i].StatTag.StatName + " [" + _Stats[i].Value + "]";
            }
        }
#endif
    }
}
