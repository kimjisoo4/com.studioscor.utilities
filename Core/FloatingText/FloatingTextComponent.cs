using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Pool;

namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/FloatingText/Floating Text Component", order: 0)]
    public class FloatingTextComponent : BaseMonoBehaviour
	{
        [Header(" [ Floating Damage Text ] ")]
		[SerializeField] private TMP_Text _textArea;
        [SerializeField] private UnityEvent _onFloatingText;
        [SerializeField] private UnityEvent _onReleased;

        public event UnityAction<FloatingTextComponent> OnFlaotingText;
        public event UnityAction<FloatingTextComponent> OnReleased;

        private IObjectPool<FloatingTextComponent> _floatingDamagePool;

        private bool _isRelease = false;

        private void Reset()
        {
#if UNITY_EDITOR
            transform.TryGetComponentInParentOrChildren(out _textArea);
#endif
        }
        public void Create(IObjectPool<FloatingTextComponent> pool)
        {
            Log(" Create ");

            _floatingDamagePool = pool;
        }
        public void Activate()
        {
            Log(" Activate ");

            _isRelease = true;
        }
        public void Release()
        {
            if (!_isRelease)
                return;

            Log(" Release ");

            _isRelease = true;

            _floatingDamagePool.Release(this);

            Invoke_OnReleased();

            gameObject.SetActive(false);
        }

        public void OnText(float damage)
		{
            OnText(Mathf.RoundToInt(damage));
		}

		public void OnText(int damage)
		{
            Log($" On Text - [ {damage} ] ");

            _textArea.text = damage.ToString();

			gameObject.SetActive(true);

            Invoke_OnFloatingText();
        }

        private void Invoke_OnFloatingText()
        {
            Log($"{nameof(OnFlaotingText)}");

            _onFloatingText?.Invoke();
            OnFlaotingText?.Invoke(this);
        }
        private void Invoke_OnReleased()
        {
            Log($"{nameof(OnReleased)}");

            _onReleased?.Invoke();
            OnReleased?.Invoke(this);
        }
    }

}
