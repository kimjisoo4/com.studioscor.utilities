using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.StatusSystem
{


    public partial class StatusUI_Image : StatusUIModifier
    {
        [SerializeField] private Image _Image;

        public override void StatusUpdate(Status status, float currentPoint, float prevPoint)
        {
            _Image.fillAmount = currentPoint / status.MaxPoint;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            _Image = GetComponent<Image>();
        }

#endif
    }
}