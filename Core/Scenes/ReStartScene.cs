using UnityEngine;
using UnityEngine.SceneManagement;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new Re Start Scene", fileName = "SceneLoader_ReStart")]
    public class ReStartScene : SceneLoader
    {
        [Header(" [ Re Start Scene ]")]
        [SerializeField] private LoadSceneMode _Mode = LoadSceneMode.Single;

        public override Scene GetScene => SceneManager.GetActiveScene();
        public override void LoadScene()
        {
            var async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, _Mode);

            Callback_OnStarted();

            if (async is not null)
                async.completed += Async_completed;
            else
                Callback_OnFinished();
        }

        private void Async_completed(AsyncOperation obj)
        {
            Callback_OnFinished();
        }

        public override void UnLoadScene()
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }
}
