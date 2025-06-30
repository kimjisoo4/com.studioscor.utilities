using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace StudioScor.Utilities
{

    public abstract class SceneLoader : BaseScriptableObject
    {
        public bool IsPlaying { get; protected set; }

        public abstract void LoadScene();
        public abstract void UnloadScene();
        public abstract Scene GetScene { get; }

        public event UnityAction<SceneLoader> OnLoadStarted;
        public event UnityAction<SceneLoader> OnLoadFinished;

        protected void Callback_OnStarted()
        {
            Log(nameof(OnLoadStarted));

            OnLoadStarted?.Invoke(this);
        }
        protected void Callback_OnFinished()
        {
            Log(nameof(OnLoadStarted));

            OnLoadFinished?.Invoke(this);
        }
    }
}
