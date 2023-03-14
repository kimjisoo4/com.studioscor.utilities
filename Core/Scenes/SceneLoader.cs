using UnityEngine.Events;


namespace StudioScor.Utilities
{
    public abstract class SceneLoader : BaseScriptableObject
    {
        public abstract void LoadScene();
        public abstract void UnLoadScene();

        public event UnityAction OnStarted;
        public event UnityAction OnFinished;

        protected void Callback_OnStarted()
        {
            Log(" On Started Load Scene");

            OnStarted?.Invoke();
        }
        protected void Callback_OnFinished()
        {
            Log(" On Finished Load Scene");

            OnFinished?.Invoke();
        }
    }
}
