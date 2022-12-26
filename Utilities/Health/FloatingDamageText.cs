using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Pool;

namespace StudioScor.Utilities
{
    public class FloatingDamageText : MonoBehaviour
	{
		[SerializeField] private TMP_Text _TMP;

        private IObjectPool<FloatingDamageText> _FloatingDamagePool;

        private bool _IsRelease = false;

        public void Create(IObjectPool<FloatingDamageText> pool)
        {
            _FloatingDamagePool = pool;
        }
        public void Activate()
        {
            _IsRelease = true;
        }
        public void Release()
        {
            if (!_IsRelease)
                return;

            _IsRelease = true;

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
