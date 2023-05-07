#if SCOR_ENABLE_VISUALSCRIPTING

namespace StudioScor.Utilities.VisualScripting.Editor.Community
{
    public abstract class GlobalProcess
    {
        public abstract void Process();
        public abstract void OnBind();
        public abstract void OnUnbind();
        public abstract void OnInitialize();
    }
}
#endif