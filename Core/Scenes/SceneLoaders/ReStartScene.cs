using UnityEngine;
using UnityEngine.SceneManagement;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new Re Start Scene", fileName = "SceneLoader_ReStart")]
    public class ReStartScene : SceneLoader
    {
        [Header(" [ Re Start Scene ]")]
        [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;
        [SerializeField] private bool _useLoadingScene = false;
        [SerializeField] private SceneData _loadingScene;
        public override Scene GetScene => SceneManager.GetActiveScene();

        private string _restartSceneName;
        private void OnValidate()
        {
#if UNITY_EDITOR
            _loadingScene.OnValidate();
#endif
        }

        protected override void OnReset()
        {
            base.OnReset();

#if UNITY_EDITOR
            _loadingScene.OnValidate();
#endif
        }


        [ContextMenu(nameof(LoadScene), false, 1000000)]
        public override void LoadScene()
        {
            IsPlaying = true;

            Callback_OnStarted();

            if(_useLoadingScene)
            {
                OnLoadLoadingScene();
            }
            else
            {
                OnLoadScene();
            }
        }

        private void OnFinishe()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;

            Callback_OnFinished();
        }

        private void OnLoadScene()
        {
            var async = SceneManager.LoadSceneAsync(_useLoadingScene ? _restartSceneName : SceneManager.GetActiveScene().name, loadSceneMode);

            if (async is not null)
            {
                async.completed += Async_completed_LoadScene;
            }
            else
            {
                if (!_useLoadingScene)
                    OnFinishe();
                else
                    UnloadLoadingScene();
            }
        }
        private void OnLoadLoadingScene()
        {
            var async = _loadingScene.LoadScene(LoadSceneMode.Additive);

            if (async is not null)
            {
                async.completed += Async_completed_LoadLoading;
            }
            else
            {
                OnLoadScene();
            }
        }
        private void UnloadLoadingScene()
        {
            var async = _loadingScene.UnLoadScene();

            if (async is not null)
            {
                async.completed += Async_completed_UnloadLoading;
            }
            else
            {
                OnFinishe();
            }
        }
        private void UnloadCurrentScene()
        {
            _restartSceneName = SceneManager.GetActiveScene().name;

            var async = SceneManager.UnloadSceneAsync(_restartSceneName);

            if (async is not null)
            {
                async.completed += Async_completed_UnloadScene;
            }
            else
            {
                OnLoadScene();
            }
        }
        

        private void Async_completed_LoadScene(AsyncOperation obj)
        {
            if (_useLoadingScene)
                OnFinishe();
            else
                UnloadLoadingScene();
        }
        private void Async_completed_UnloadScene(AsyncOperation obj)
        {
            OnLoadScene();
        }

        private void Async_completed_LoadLoading(AsyncOperation obj)
        {
            UnloadCurrentScene();
        }
        private void Async_completed_UnloadLoading(AsyncOperation obj)
        {
            OnFinishe();
        }

        public override void UnLoadScene()
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }
}
