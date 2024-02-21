using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    public class HealthBarUI : MonoBehaviour
    {
        [Header(" [ HeathBar ] ")]
        [SerializeField] private HealthComponent _DefaultHealthComponent;

        [Header(" [ HeathBar UI ] ")]
        [SerializeField] private Image _Fill;

        private HealthComponent _HealthComponent;

        private void Start()
        {
            if (!_HealthComponent && _DefaultHealthComponent)
                Setup(_DefaultHealthComponent);

            UpdateUI();
        }

        public void Setup(HealthComponent healthComponenet)
        {
            if (_HealthComponent)
            {
                _HealthComponent.OnChangedHealth -= HealthComponenet_OnChangedHealth;
            }

            _HealthComponent = healthComponenet;

            if (_HealthComponent)
            {
                _HealthComponent.OnChangedHealth += HealthComponenet_OnChangedHealth;
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            if (!_Fill)
                return;

            if (_HealthComponent)
            {
                _Fill.fillAmount = _HealthComponent.NormalizedHealth;
            }
            else
            {
                _Fill.fillAmount = 0f;
            }
        }

        private void HealthComponenet_OnChangedHealth(HealthComponent health, float currentValue, float prevValue)
        {
            UpdateUI();
        }
    }

}
