#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;

namespace StudioScor.Utilities.VisualScripting
{
    public class SightMessageListener : MessageListener
    {
        private void Awake()
        {
            if (TryGetComponent(out ISight sight))
            {
                sight.OnFoundSight += Sight_OnFoundSight;
                sight.OnLostedSight += Sight_OnLostedSight;
            }
        }
        private void OnDestroy()
        {
            if (TryGetComponent(out ISight sight))
            {
                sight.OnFoundSight -= Sight_OnFoundSight;
                sight.OnLostedSight -= Sight_OnLostedSight;
            }
        }

        private void Sight_OnLostedSight(ISight aiSencer, Collider sight)
        {
            EventBus.Trigger(new EventHook(SightWithVisualScripting.ON_LOSTED_SIGHT, aiSencer), sight);
        }

        private void Sight_OnFoundSight(ISight aiSencer, Collider sight)
        {
            EventBus.Trigger(new EventHook(SightWithVisualScripting.ON_FOUND_SIGHT, aiSencer), sight);
        }
    }
}

#endif