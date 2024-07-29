using UnityEngine;
using TMPro;

namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/SimpleAmount/Modifier/Amount Text Modifier", order: 0)]
    public class AmountTextModifier : SimpleAmountModifier
    {
        [Header(" [ Amount Text Modifier ] ")]
        [SerializeField] private EAmountValueType AmountValueType;
        [SerializeField] private TMP_Text _Text;

        protected override void Reset()
        {
#if UNITY_EDITOR
            base.Reset();

            gameObject.TryGetComponentInParentOrChildren(out _Text);
#endif
        }

        public override void UpdateModifier()
        {
            switch (AmountValueType)
            {
                case EAmountValueType.Current:
                    _Text.text = SimpleAmount.CurrentValue.ToString("F0");
                    break;
                case EAmountValueType.Max:
                    _Text.text = SimpleAmount.MaxValue.ToString("F0");
                    break;
                case EAmountValueType.CurrentAndMax:
                    _Text.text = $"{SimpleAmount.CurrentValue:F0}/{SimpleAmount.MaxValue:F0}";
                    break;
                case EAmountValueType.Normalized:
                    _Text.text = (SimpleAmount.NormalizedValue * 100).ToString("P0");
                    break;
                default:
                    break;
            }
        }
    }

}