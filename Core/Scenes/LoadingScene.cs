using UnityEngine;
using UnityEngine.Events;

using UnityEngine.SceneManagement;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new Loading Scene", fileName = "SceneLoader_Loading")]
    public class LoadingScene : SceneLoader
    {
        [Header(" [ Loading Scene ] ")]
        [SerializeField] private SceneData _LoadingScene;

        private int _Count;


        private void OnValidate()
        {
#if UNITY_EDITOR
            _LoadingScene.OnValidate();
#endif
        }

        public override void LoadScene()
        {
            var async = _LoadingScene.LoadScene(LoadSceneMode.Additive);

            Callback_OnStarted();

            if(async is not null)
            {
                async.completed += LoadingSceneAsync_Completed;
            }
            else
            {
                UnLoadOtherScenes();
            }
        }
        public override void UnLoadScene()
        {
            _LoadingScene.UnLoadScene();
        }


        private void LoadingSceneAsync_Completed(AsyncOperation async)
        {
            UnLoadOtherScenes();
        }

        private void UnLoadOtherScenes()
        {
            var scene = _LoadingScene.GetScene();

            SceneManager.SetActiveScene(scene);

            OnUnLoadOtherScenes();

            if (_Count == 0)
            {
                Resources.UnloadUnusedAssets();

                Callback_OnFinished();
            }
        }

        private void OnUnLoadOtherScenes()
        {
            int count = SceneManager.sceneCount;
            var activeScene = SceneManager.GetActiveScene();

            Log($" Current Active Scene - {activeScene.name}");
            Log($" Current Scene Count - {count}");

            _Count = count - 1;

            for (int i = count - 1; i >= 0; i--)
            {
                var scene = SceneManager.GetSceneAt(i);

                if (scene != activeScene)
                {
                    Log($"[ {scene.name} ] is Sub Scene.");

                    var async = SceneManager.UnloadSceneAsync(scene);

                    if(async is null)
                    {
                        _Count--;
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
            _Count--;

            if (_Count == 0)
            {
                Resources.UnloadUnusedAssets();

                Callback_OnFinished();
            }
        }
    }
}
