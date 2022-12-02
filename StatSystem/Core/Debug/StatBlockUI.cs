using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace StudioScor.StatSystem
{
    public class StatBlockUI : MonoBehaviour
    {
        [SerializeField] private Text Name;
        [SerializeField] private Text Count;

        private Stat _Stat;

        public void SetText(KeyValuePair<StatTag, Stat> stat)
        {
            if (_Stat is not null)
            {
                _Stat.OnChangedValue -= Stat_OnChangedValue;
            }

            _Stat = stat.Value;

            Name.text = _Stat.StatName;
            Count.text = Mathf.RoundToInt(_Stat.Value).ToString();

            _Stat.OnChangedValue += Stat_OnChangedValue;
        }

        private void Stat_OnChangedValue(Stat stat, float currentValue, float prevValue)
        {
            Name.text = _Stat.StatName;
            Count.text = Mathf.RoundToInt(_Stat.Value).ToString();
        }
    }
}
