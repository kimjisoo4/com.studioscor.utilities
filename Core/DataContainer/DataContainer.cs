#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StudioScor.Utilities.DataContainer
{
    public abstract class DataContainer<T> : BaseScriptableObject where T : Object, IData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Data Container", GroupID = "Container")]
        [BoxGroup("Container")]
#endif
        [SerializeField] private T[] _datas;

        private readonly Dictionary<int, T> _dataLookUp = new();
        public IReadOnlyDictionary<int, T> Datas => _dataLookUp;

        protected override void OnReset()
        {
            base.OnReset();

            _dataLookUp.Clear();

            foreach (var data in _datas)
            {
                _dataLookUp.Add(data.ID, data);
            }
        }


#if ODIN_INSPECTOR
        [ButtonGroup("Container/Buttons")]
#endif
        [AddComponentMenu(nameof(FindObjectOfTypeAll), order: 1000000)]
        private void FindObjectOfTypeAll()
        {
#if UNITY_EDITOR
            Log(nameof(FindObjectOfTypeAll));
            var datas = Resources.FindObjectsOfTypeAll<T>();
            
            datas.OrderBy(so => so.name);

            _datas = datas;

            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

#if ODIN_INSPECTOR
        [ButtonGroup("Container/Buttons")]
#endif
        [AddComponentMenu(nameof(ResetAllID), order: 1000000)]
        private void ResetAllID()
        {
#if UNITY_EDITOR
            Log(nameof(ResetAllID));

            foreach (var item in _datas)
            {
                item.ResetID();
            }
#endif
        }


#if ODIN_INSPECTOR
        [ButtonGroup("Container/Buttons")]
#endif
        [AddComponentMenu(nameof(Ping), order: 1000000)]
        private void Ping()
        {
#if UNITY_EDITOR
            Log(nameof(Ping));

            UnityEditor.EditorGUIUtility.PingObject(this);
#endif
        }

    }
}
