using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Pool;

namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/FloatingText/Floating Text Component", order: 0)]
    public class FloatingTextComponent : BaseMonoBehaviour
	{
        [Header(" [ Floating Damage Text ] ")]
		[SerializeField] private TMP_Text _TMP;
        [SerializeField] private UnityEvent _OnFloatingText;
        [SerializeField] private UnityEvent _OnReleased;

        private IObjectPool<FloatingTextComponent> _FloatingDamagePool;

        private bool _IsRelease = false;

        private void Reset()
        {
#if UNITY_EDITOR
            transform.TryGetComponentInParentOrChildren(out _TMP);
#endif
        }
        public void Create(IObjectPool<FloatingTextComponent> pool)
        {
            Log(" Create ");

            _FloatingDamagePool = pool;
        }
        public void Activate()
        {
            Log(" Activate ");

            _IsRelease = true;
        }
        public void Release()
        {
            if (!_IsRelease)
                return;

            Log(" Release ");

            _IsRelease = true;

            _FloatingDamagePool.Release(this);

            Callback_OnReleased();

            gameObject.SetActive(false);
        }

        public void OnText(float damage)
		{
            OnText(Mathf.RoundToInt(damage));
		}

		public void OnText(int damage)
		{
            Log($" On Text - [ {damage} ] ");

            _TMP.text = damage.ToString();

			gameObject.SetActive(true);

            Callback_OnFloatingText();
        }

        private void Callback_OnFloatingText()
        {
            Log("On Floating Text");

            _OnFloatingText?.Invoke();
        }
        private void Callback_OnReleased()
        {
            Log("On Released");

            _OnReleased?.Invoke();
        }
    }

}
