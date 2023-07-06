#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

namespace StudioScor.Utilities.VisualScripting
{
    public class DilationMessageListener : MessageListener
    {
        private readonly DilationEvent _DilationEvent = new();

        private void Awake()
        {
            if (TryGetComponent(out IDilationSystem dilation))
            {
                dilation.OnChangedDilation += Dilation_OnChangedDilation;
            }
        }
        private void OnDestroy()
        {
            if (TryGetComponent(out IDilationSystem dilation))
            {
                dilation.OnChangedDilation -= Dilation_OnChangedDilation;
            }
        }

        private void Dilation_OnChangedDilation(IDilationSystem dilation, float currentDilation, float prevDilation)
        {
            _DilationEvent.Current = currentDilation;
            _DilationEvent.Prev = prevDilation;

            EventBus.Trigger(new EventHook(DilationWithVisualScripting.ON_CHANGED_DILATION, dilation), _DilationEvent);

            _DilationEvent.Current = default;
            _DilationEvent.Prev = default;
        }
    }
}

#endif