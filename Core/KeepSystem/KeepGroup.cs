using UnityEngine;
using System.Collections.Generic;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/KeepSystem/new Keep Group", fileName = "Keep_Group_")]
    public class KeepGroup : KeepData
    {
        [SerializeField] private List<KeepData> _Datas;

        public override void ResetData()
        {
            foreach (var data in _Datas)
            {
                data.ResetData();
            }
        }
        public override void Save(GameObject gameObject)
        {
            foreach (var data in _Datas)
            {
                data.Save(gameObject);
            }    
        }
        public override void Load(GameObject gameObject)
        {
            foreach (var data in _Datas)
            {
                data.Load(gameObject);
            }
        }
    }
}
