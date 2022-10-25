using UnityEngine;

namespace KimScor.Utilities
{
    public class FadeState : BaseStateMono
    {
        [SerializeField] private SimpleFadeManager _SimpleFade;
        public SimpleFadeManager SimpleFade => _SimpleFade;
        public virtual float FadeAmount => 0f;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            gameObject.TryGetComponentInParentOrChildren(out _SimpleFade);
        }
#endif
    }

}
