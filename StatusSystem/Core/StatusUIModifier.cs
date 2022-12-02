using UnityEngine;

namespace StudioScor.StatusSystem
{
    public abstract class StatusUIModifier : MonoBehaviour
    {
        [SerializeField] private StatusUI _StatusUI;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            _StatusUI = GetComponentInParent<StatusUI>();
        }
#endif

        private void OnEnable()
        {
            if(_StatusUI == null)
            {
                _StatusUI = GetComponentInParent<StatusUI>();
            }

            if (_StatusUI != null)
                _StatusUI.AddModifier(this);
        }
        private void OnDisable()
        {
            if (_StatusUI != null)
                _StatusUI.RemoveModifire(this);
        }

        public abstract void StatusUpdate(Status status, float currentPoint, float prevPoint);
    }
}