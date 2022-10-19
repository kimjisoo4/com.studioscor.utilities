using UnityEngine;
using System.Collections.Generic;

namespace KimScor.Utilities
{
    public class ApplyPoisonComponent : MonoBehaviour, IPoisonOwner
    {
        [SerializeField] private HealthComponent _HealthComponent;

        private List<IPoison> _Poisons;
        public IReadOnlyList<IPoison> Poisons => _Poisons;

        private void Awake()
        {
            _Poisons = new();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            UpdatePoisons(deltaTime);
        }

        private void UpdatePoisons(float deltaTime)
        {
            foreach (var poison in _Poisons)
            {
                poison.UpdatePoison(deltaTime);
            }
        }
        public void TakePoisonDamage(IPoison poison)
        {
            _HealthComponent.OnTakeDamage(poison.Damage);
        }

        public bool TryApplyPoison(IPoison newPoison)
        {
            if (!CanApplyPoison(newPoison))
                return false;

            ApplyPoison(newPoison);

            return true;
        }

        public bool CanApplyPoison(IPoison newPoison)
        {
            return newPoison.CanApplyPoison(this);
        }

        public void ApplyPoison(IPoison poison)
        {
            poison.ApplyPoison(this);

            _Poisons.Add(poison);
        }
    }
}