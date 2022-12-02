using UnityEngine;
using UnityEngine.UI;

namespace StudioScor.StatusSystem
{
    public class StatusUI_Text : StatusUIModifier
    {
        [SerializeField] private Text _Text;

        public override void StatusUpdate(Status status, float currentPoint, float prevPoint)
        {
            _Text.text = Mathf.Floor(currentPoint) + " / " + status.MaxPoint;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            _Text = GetComponent<Text>();
        }
#endif
    }
}