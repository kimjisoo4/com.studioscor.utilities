using UnityEngine;

namespace KimScor.Utilities
{
    public class FloatingDamageManager : Singleton<FloatingDamageManager>
    {
        [SerializeField] private Transform _TextContainer;
		[SerializeField] private FloatingDamageText _FloatingDamageText;

		public void SpawnFloatingDamage(Vector3 position, float damage)
        {
			var text = Instantiate(_FloatingDamageText, _TextContainer);

			text.transform.position = position;
			text.OnText(damage);
        }
    }

}
