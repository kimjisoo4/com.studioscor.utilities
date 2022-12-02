using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Pool;

namespace StudioScor.Utilities
{
    public class FloatingDamageText : MonoBehaviour
	{
		[SerializeField] private TMP_Text _TMP;

        [SerializeField] private IObjectPool<FloatingDamageText> _FloatingDamagePool;

        public void Create(IObjectPool<FloatingDamageText> pool)
        {
            _FloatingDamagePool = pool;
        }
        public void Release()
        {
            _FloatingDamagePool.Release(this);

            gameObject.SetActive(false);
        }

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
