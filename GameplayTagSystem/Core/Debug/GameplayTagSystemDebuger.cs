using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

namespace StudioScor.GameplayTagSystem
{
    public class GameplayTagSystemDebuger : MonoBehaviour
    {
        [SerializeField] private GameplayTagComponent _GameplayTagComponent;
        [SerializeField] private GridLayoutGroup _Grid;
        [SerializeField] private GameplayTagBlockUI _Block;
        [SerializeField] private List<GameplayTagBlockUI> _Blocks;
        [SerializeField] private bool _LookBlockTag = false;

#if UNITY_EDITOR
        private void Reset()
        {
            _Grid = GetComponentInChildren<GridLayoutGroup>();
            _Block = GetComponentInChildren<GameplayTagBlockUI>();
        }
#endif
        private void OnEnable()
        {
            GameplayTagSystem_UpdateTag(_GameplayTagComponent);

            _GameplayTagComponent.OnAddedBlockTag += GameplayTagSystem_UpdateTag;
            _GameplayTagComponent.OnAddedOwnedTag += GameplayTagSystem_UpdateTag;
            _GameplayTagComponent.OnRemovedBlockTag += GameplayTagSystem_UpdateTag;
            _GameplayTagComponent.OnRemovedOwnedTag += GameplayTagSystem_UpdateTag;
        }
        private void OnDisable()
        {
            _GameplayTagComponent.OnAddedBlockTag -= GameplayTagSystem_UpdateTag;
            _GameplayTagComponent.OnAddedOwnedTag -= GameplayTagSystem_UpdateTag;
            _GameplayTagComponent.OnRemovedBlockTag -= GameplayTagSystem_UpdateTag;
            _GameplayTagComponent.OnRemovedOwnedTag -= GameplayTagSystem_UpdateTag;
        }

        public void SetGameplayTagSystem(GameplayTagComponent gameplayTagComponent)
        {
            if(_GameplayTagComponent != null)
            {
                _GameplayTagComponent.OnAddedBlockTag -= GameplayTagSystem_UpdateTag;
                _GameplayTagComponent.OnAddedOwnedTag -= GameplayTagSystem_UpdateTag;
                _GameplayTagComponent.OnRemovedBlockTag -= GameplayTagSystem_UpdateTag;
                _GameplayTagComponent.OnRemovedOwnedTag -= GameplayTagSystem_UpdateTag;
            }

            _GameplayTagComponent = gameplayTagComponent;

            if (_GameplayTagComponent != null)
            {
                _GameplayTagComponent.OnAddedBlockTag += GameplayTagSystem_UpdateTag;
                _GameplayTagComponent.OnAddedOwnedTag += GameplayTagSystem_UpdateTag;
                _GameplayTagComponent.OnRemovedBlockTag += GameplayTagSystem_UpdateTag;
                _GameplayTagComponent.OnRemovedOwnedTag += GameplayTagSystem_UpdateTag;
            }

            UpdateGameplayTagBlock();
        }

        [ContextMenu("ChangeLook")]
        public void ChangeLook()
        {
            _LookBlockTag = !_LookBlockTag;

            UpdateGameplayTagBlock();
        }

        private void UpdateGameplayTagBlock()
        {
            if(_GameplayTagComponent == null)
            {
                foreach (var block in _Blocks)
                {
                    block.gameObject.SetActive(false);
                }

                return;
            }

            int count = 0;

            IReadOnlyDictionary<GameplayTag, int> container = !_LookBlockTag ? _GameplayTagComponent.OwnedTags : _GameplayTagComponent.BlockTags;

            if (container.Count == 0)
                return;

            foreach (KeyValuePair<GameplayTag, int> tag in container)
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

        private void GameplayTagSystem_UpdateTag(GameplayTagComponent gameplayTagComponent, GameplayTag changedTag = null)
        {
            UpdateGameplayTagBlock();
        }
    }
}
