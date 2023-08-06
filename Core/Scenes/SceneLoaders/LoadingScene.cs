using UnityEngine;
using UnityEngine.Events;

using UnityEngine.SceneManagement;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new Loading Scene", fileName = "SceneLoader_Loading")]
    public class LoadingScene : SceneLoader
    {
        [Header(" [ Loading Scene ] ")]
        [SerializeField] private SceneData loadingScene;

        public override Scene GetScene => loadingScene.GetScene();

        private int count;

        private void OnValidate()
        {
#if UNITY_EDITOR
            loadingScene.OnValidate();
#endif
        }

        public override void LoadScene()
        {
            Callback_OnStarted();

            var async = loadingScene.LoadScene(LoadSceneMode.Additive);

            if (async is null)
            {
                UnLoadOtherScenes();
            }
            else
            {
                async.completed += LoadingScene_completed;
            }
        }

        private void LoadingScene_completed(AsyncOperation async)
        {
            UnLoadOtherScenes();
        }

        public override void UnLoadScene()
        {
            loadingScene.UnLoadScene();
        }

        private void LoadingScene_OnFinished(SceneLoader scene)
        {
            scene.OnFinished -= LoadingScene_OnFinished;
        }

        private void UnLoadOtherScenes()
        {
            var scene = loadingScene.GetScene();

            SceneManager.SetActiveScene(scene);

            OnUnLoadOtherScenes();

            if (count == 0)
                OnFinishedUnloadOtherScene();
        }

        private void OnUnLoadOtherScenes()
        {
            int count = SceneManager.sceneCount;
            var activeScene = SceneManager.GetActiveScene();

            Log($" Current Active Scene - {activeScene.name}");
            Log($" Current Scene Count - {count}");

            this.count = count - 1;

            for (int i = count - 1; i >= 0; i--)
            {
                var scene = SceneManager.GetSceneAt(i);

                if (scene != activeScene)
                {
                    Log($"[ {scene.name} ] is Sub Scene.");

                    var async = SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

                    if(async is null)
                    {
                        this.count--;
                    }
                    else
                    {
                        async.completed += Async_UnLoadedOther_Completed;
                    }
                }
                else
                {
                    Log($"[ {scene.name} ] is Active Scene.");
                }
            }
        }

        private void Async_UnLoadedOther_Completed(AsyncOperation async)
        {
            count--;

            if (count == 0)
                OnFinishedUnloadOtherScene();
        }

        private void OnFinishedUnloadOtherScene()
        {
            Callback_OnFinished();
        }
    }
}
