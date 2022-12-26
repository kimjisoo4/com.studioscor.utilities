using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.Utilities
{

    public class AmountFillModifier : SimpleAmountModifier
    {
        [Header(" [ Amount Text Modifier ] ")]
        [SerializeField] private Image _Fill;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            gameObject.TryGetComponentInParentOrChildren(out _Fill);
        }
#endif
        public override void UpdateValue()
        {
            _Fill.fillAmount = SimpleAmount.NormalizedValue;
        }
    }
}