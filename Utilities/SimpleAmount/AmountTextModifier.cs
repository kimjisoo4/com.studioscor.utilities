using UnityEngine;
using TMPro;

namespace StudioScor.Utilities
{
    public class AmountTextModifier : SimpleAmountModifier
    {
        [Header(" [ Amount Text Modifier ] ")]
        [SerializeField] private EAmountValueType AmountValueType;
        [SerializeField] private TMP_Text _Text;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            gameObject.TryGetComponentInParentOrChildren(out _Text);
        }
#endif

        public override void UpdateValue()
        {
            switch (AmountValueType)
            {
                case EAmountValueType.Current:
                    _Text.text = SimpleAmount.CurrentValue.ToString("F0");
                    break;
                case EAmountValueType.Max:
                    _Text.text = SimpleAmount.MaxValue.ToString("F0");
                    break;
                case EAmountValueType.Normalized:
                    _Text.text = (SimpleAmount.NormalizedValue * 100).ToString("F0");
                    break;
                default:
                    break;
            }
        }
    }

}