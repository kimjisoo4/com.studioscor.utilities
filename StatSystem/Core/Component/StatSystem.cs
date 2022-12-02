using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace StudioScor.StatSystem
{
    public class StatSystem : MonoBehaviour
    {
        #region Events
        public delegate void StatEventHandler(StatSystem statSystem, Stat stat);
        #endregion
        
        [SerializeField] private InitializationStats[] _initializationStats;
        private bool _WasSetting = false;

        private Dictionary<StatTag, Stat> _Stats = new Dictionary<StatTag, Stat>();

        public IReadOnlyDictionary<StatTag, Stat> Stats
        {
            get
            {
                if (!_WasSetting)
                    SetupStatSystem();

                return _Stats;
            }
        }

        public event StatEventHandler OnAddedStat;


        private void Awake()
        {
            if (!_WasSetting)
                SetupStatSystem();
        }

        protected virtual void SetupStatSystem()
        {
            _WasSetting = true;

            _Stats = new Dictionary<StatTag, Stat>();

            if (_initializationStats.Length > 0)
            {
                foreach (InitializationStats initializationStats in _initializationStats)
                {
                    OnInitializationStat(initializationStats);
                }
            }
        }

        public void OnInitializationStat(InitializationStats stats)
        {
            foreach (FInitializationStat initializationStat in stats.Stats)
            {
                InitializationStat(initializationStat);
            }
        }
        public void OnInitializationStat(FInitializationStat[] stats)
        {
            foreach (FInitializationStat initializationStat in stats)
            {
                InitializationStat(initializationStat);
            }
        }
        public Stat OnInitializationStat(FInitializationStat stat)
        {
            return InitializationStat(stat);
        }

        private Stat InitializationStat(FInitializationStat initializationStat)
        {
            if (!Stats.TryGetValue(initializationStat.StatTag, out Stat stat))
            {   
                stat = new Stat(initializationStat.StatTag, initializationStat.Value);

                _Stats.Add(initializationStat.StatTag, stat);

                OnAddStat(stat);
            }
            else
            {
                stat.SetBaseValue(initializationStat.Value);
            }

            return stat;
        }
        

        public bool TryGetValue(StatTag Tag, out Stat stat)
        {
            if (!Tag)
            {
                stat = null;

                return false;
            }

            return Stats.TryGetValue(Tag, out stat);
        }
        public bool TryGetValue(string tag, out Stat stat)
        {
            if (tag.Equals(null))
            {
                stat = null;
                
                return false;
            }

            foreach (Stat containStat in Stats.Values)
            {
                if (containStat.StatName == tag)
                {
                    stat = containStat;

                    return true;
                }
            }

            stat = null;

            return false;
        }

        public Stat SetOrCreateValue(StatTag tag, float value = 0f)
        {
            if(TryGetValue(tag, out Stat stat))
            {
                stat.SetBaseValue(value);

                return stat;
            }
            else
            {
                stat = new Stat(tag, value);

                _Stats.Add(tag, stat);

                OnAddStat(stat);

                return stat;
            }
        }

        public Stat SetOrCreateValue(string tag, float value = 0f)
        {
            if (TryGetValue(tag, out Stat stat))
            {
                stat.SetBaseValue(value);

                return stat;
            }
            else
            {
                var newStatTag = Instantiate(new StatTag());

                newStatTag.SetStatTag(tag);

                stat = new Stat(newStatTag, value);

                return stat;
            }
        }

        public Stat GetOrCreateValue(StatTag Tag, float value = 0f)
        {
            if (TryGetValue(tag, out Stat stat))
            {
                stat.SetBaseValue(value);

                return stat;
            }
            else
            {
                stat = new Stat(Tag, value);

                _Stats.Add(Tag, stat);

                OnAddStat(stat);

                return stat;
            }
        }

        public Stat GetOrCreateValue(string tag, float value = 0f)
        {
            if(tag.Equals(null))
            {
                return null;
            }

            foreach (Stat stat in Stats.Values)
            {
                if (stat.StatName == tag) 
                {
                    return stat;
                }
            }

            var newStatTag = Instantiate(new StatTag());
            
            newStatTag.SetStatTag(tag);

            var newStat = new Stat(newStatTag, value);

            return newStat;
        }

        #region CallBack
        protected void OnAddStat(Stat stat)
        {
            OnAddedStat?.Invoke(this, stat);
        }

        #endregion
    }
}
