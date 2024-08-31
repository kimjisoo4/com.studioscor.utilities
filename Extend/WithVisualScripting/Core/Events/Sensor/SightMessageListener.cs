#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

namespace StudioScor.Utilities.VisualScripting
{
    public class SightMessageListener : MessageListener
    {
        private void Awake()
        {
            if (TryGetComponent(out ISightSensor sight))
            {
                sight.OnFoundSight += Sight_OnFoundSight;
                sight.OnLostedSight += Sight_OnLostedSight;
            }
        }
        private void OnDestroy()
        {
            if (TryGetComponent(out ISightSensor sight))
            {
                sight.OnFoundSight -= Sight_OnFoundSight;
                sight.OnLostedSight -= Sight_OnLostedSight;
            }
        }

        private void Sight_OnLostedSight(ISightSensor aiSencer, ISightTarget sight)
        {
            EventBus.Trigger(new EventHook(SightWithVisualScripting.ON_LOSTED_SIGHT, aiSencer), sight);
        }

        private void Sight_OnFoundSight(ISightSensor aiSencer, ISightTarget sight)
        {
            EventBus.Trigger(new EventHook(SightWithVisualScripting.ON_FOUND_SIGHT, aiSencer), sight);
        }
    }
}

#endif