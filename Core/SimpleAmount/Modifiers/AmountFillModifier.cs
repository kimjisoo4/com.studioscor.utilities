using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/SimpleAmount/Modifier/Amount Fill Modifier", order: 0)]
    public class AmountFillModifier : SimpleAmountModifier
    {
        [Header(" [ Amount Text Modifier ] ")]
        [SerializeField] private Image _Fill;

        protected override void Reset()
        {
#if UNITY_EDITOR
            base.Reset();

            gameObject.TryGetComponentInParentOrChildren(out _Fill);
#endif
        }
        public override void UpdateModifier()
        {
            _Fill.fillAmount = SimpleAmount.NormalizedValue;
        }
    }
}