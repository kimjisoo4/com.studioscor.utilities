using UnityEngine;

namespace StudioScor.Utilities
{
    public class SimpleFadeState : BaseStateMono
    {
        [Header(" [ Simple Fade State ] ")]
        [SerializeField] private SimpleFadeComponent _SimpleFade;
        protected SimpleFadeComponent SimpleFade => _SimpleFade;

        protected override void Reset()
        {
            base.Reset();

            gameObject.TryGetComponentInParentOrChildren(out _SimpleFade);
        }
    }

}
