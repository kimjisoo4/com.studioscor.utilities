using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace StudioScor.Utilities
{
    public abstract class SceneLoader : BaseScriptableObject
    {
        public abstract void LoadScene();
        public abstract void UnLoadScene();

        public event UnityAction<SceneLoader> OnStarted;
        public event UnityAction<SceneLoader> OnFinished;
        public abstract Scene GetScene { get; }

        protected void Callback_OnStarted()
        {
            Log(" On Started Load Scene");

            OnStarted?.Invoke(this);
        }
        protected void Callback_OnFinished()
        {
            Log(" On Finished Load Scene");

            OnFinished?.Invoke(this);
        }
    }
}
