using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KimScor.Utilities
{
    public class FloatingDamageText : MonoBehaviour
	{
		[SerializeField] private TMP_Text _TMP;

		public void OnText(float damage)
		{
			OnText(Mathf.RoundToInt(damage));
		}

		public void OnText(int damage)
		{
			_TMP.text = damage.ToString();

			gameObject.SetActive(true);
		}
	}

}
