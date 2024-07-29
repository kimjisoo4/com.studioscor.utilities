using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.Utilities
{

    [AddComponentMenu("StudioScor/Utilities/SimpleAmount/Modifier/Amount Fill Modifier", order: 0)]
    public class AmountFillModifier : SimpleAmountModifier
    {
        [Header(" [ Amount Fill Modifier ] ")]
        [SerializeField] private Image _fill;

        protected override void Reset()
        {
#if UNITY_EDITOR
            base.Reset();

            gameObject.TryGetComponentInParentOrChildren(out _fill);
#endif
        }
        public override void UpdateModifier()
        {
            _fill.fillAmount = SimpleAmount.NormalizedValue;
        }
    }
}