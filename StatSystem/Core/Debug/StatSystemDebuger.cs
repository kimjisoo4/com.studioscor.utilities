using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

namespace StudioScor.StatSystem
{
    public class StatSystemDebuger : MonoBehaviour
    {
        [SerializeField] private StatSystem _StatSystem;
        [SerializeField] private GridLayoutGroup _Grid;
        [SerializeField] private StatBlockUI _Block;
        [SerializeField] private List<StatBlockUI> _Blocks;

#if UNITY_EDITOR
        private void Reset()
        {
            _Grid = GetComponentInChildren<GridLayoutGroup>();
            _Block = GetComponentInChildren<StatBlockUI>();
        }
#endif
        private void OnEnable()
        {
            StatSystem_OnAddedStat(_StatSystem);

            _StatSystem.OnAddedStat += StatSystem_OnAddedStat;
        }
        private void OnDisable()
        {
#if UNITY_EDITOR
            if (!gameObject.scene.isLoaded) return;
#endif
            _StatSystem.OnAddedStat -= StatSystem_OnAddedStat;
        }

        public void SetStatSystem(StatSystem statSystem)
        {
            if (_StatSystem != null)
            {
                _StatSystem.OnAddedStat -= StatSystem_OnAddedStat;
            }

            _StatSystem = statSystem;

            if (_StatSystem != null)
            {
                _StatSystem.OnAddedStat += StatSystem_OnAddedStat;
            }

            UpdateStatBlock();
        }

        private void StatSystem_OnAddedStat(StatSystem statSystem, Stat stat = null)
        {
            UpdateStatBlock();
        }
        public void UpdateStatBlock()
        {
            if (_StatSystem == null)
            {
                foreach (var block in _Blocks)
                {
                    block.gameObject.SetActive(false);
                }

                return;
            }

            int count = 0;

            IReadOnlyDictionary<StatTag, Stat> container = _StatSystem.Stats;

            if (container.Count == 0)
                return;

            foreach (KeyValuePair<StatTag, Stat> tag in container)
            {
                if (_Blocks.Count > count)
                {
                    _Blocks[count].SetText(tag);

                    _Blocks[count].gameObject.SetActive(true);
                }
                else
                {
                    var newBlock = Instantiate(_Block, _Grid.transform);

                    newBlock.SetText(tag);

                    newBlock.gameObject.SetActive(true);

                    _Blocks.Add(newBlock);
                }

                count++;
            }

            for (int i = count; i < _Blocks.Count; i++)
            {
                _Blocks[i].gameObject.SetActive(false);
            }
        }
       
    }
}
