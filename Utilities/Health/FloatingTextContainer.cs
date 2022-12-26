using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/New FloatingTextContainer", fileName = "FloatingTextContainer_")]
    public class FloatingTextContainer : BaseScriptableObject
    {
        [Header(" [ Floating Text Container ] ")]
        [SerializeField] private FloatingTextCanvas _FloatingText;
        [SerializeField] private FloatingDamageText _FloatingDamageText;
        [SerializeField] private int _Capacity = 10;
        [SerializeField] private int _MaxSize = 100;

        private FloatingTextCanvas _InstFloatingText;

        public void SpawnFloatingDamage(Vector3 position, float damage)
        {
            if (!_InstFloatingText)
            {
                SetupPool();
            }

            _InstFloatingText.SpawnFloatingDamage(position, damage);
        }

        private void SetupPool()
        {
            _InstFloatingText = Instantiate(_FloatingText);

            _InstFloatingText.SetupFloatingText(_FloatingDamageText, _Capacity, _MaxSize);

            Log("Setup Pool");
        }
    }

}
