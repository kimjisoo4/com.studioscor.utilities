using UnityEngine;

namespace KimScor.Utilities
{
    public class SubtractHealthOverTime : MonoBehaviour
    {
		[SerializeField] private HealthComponent _HealthComponent;
		[SerializeField] private SubtractFloatOverTime _SubtractFloatOverTime;
#if UNITY_EDITOR
		private void Reset()
        {
			TryGetComponent(out _HealthComponent);      
        }
#endif

        private void Update()
        {
			if (!_SubtractFloatOverTime.UseSubtract)
				return;

			if (!_HealthComponent)
				return;

			float value = _SubtractFloatOverTime.UpdateOverTime(Time.deltaTime);

			_HealthComponent.TakeDamage(value);
        }
    }

}
