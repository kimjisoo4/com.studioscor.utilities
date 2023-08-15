using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/SimpleAmount/Modifier/Amount Change Color Modifier", order: 0)]
    public class AmountChangeColorModifier : SimpleAmountModifier
    {
        [Header(" [ Amount Change Color Modifier ] ")]
        [SerializeField] private Image targetImage;
        [SerializeField] private float targetValue = 0.8f;
        [SerializeField] private Color targetColor = Color.yellow;

        private bool wasActivate = false;

        protected override void Reset()
        {
#if UNITY_EDITOR
            base.Reset();

            gameObject.TryGetComponentInParentOrChildren(out targetImage);
#endif
        }

        public override void UpdateModifier()
        {
            float value = SimpleAmount.NormalizedValue;


            if(wasActivate)
            {
                if (value > targetValue)
                    wasActivate = false;
            }
            else
            {
                if (value <= targetValue)
                {
                    wasActivate = true;

                    targetImage.color = targetColor;
                }
            }
        }
    }
}