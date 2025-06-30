using UnityEngine;
using UnityEngine.SceneManagement;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/SceneLoadSystem/new Scene Load System", fileName = "SceneLoadSystem", order = 0)]
    public class ScriptableSceneLoadSystem : BaseScriptableObject
    {
        public delegate void SceneLoadSystenEventHandler(ScriptableSceneLoadSystem sceneLoadSystem);
        public delegate void SceneLoadSystenStateEventHandler(ScriptableSceneLoadSystem sceneLoadSystem, bool state);

        [Header(" [ Scene Load System ] ")]
        [SerializeField] private SceneReferenceSO _loadingScene;
        [SerializeField][SReadOnly] private bool _isLoading;
        [SerializeField][SReadOnly] private SceneReferenceSO _targetScene;

        private AsyncOperation _loadAsync;

        private SceneReferenceSO _prevLoadingScene;

        private bool _isInTransition;
        public bool IsInTransition
        { 
            get
            {
                return _isInTransition;
            }
            private set
            {
                if (_isInTransition == value)
                    return;

                _isInTransition = value;
                Invoke_OnTransitionStateChanged();
            }
        }

        public SceneReferenceSO TargetScene => _targetScene;
        public float Progress => _loadAsync is not null ? _loadAsync.progress : -1f;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            private set
            {
                if (_isLoading == value)
                    return;

                _isLoading = value;

                Invoke_OnLoadingStateChanged();
            }
        }

        public event SceneLoadSystenEventHandler OnLoadStarted;
        public event SceneLoadSystenEventHandler OnLoadFinished;
        public event SceneLoadSystenEventHandler OnUnloadStarted;
        public event SceneLoadSystenEventHandler OnUnloadFinished;

        public event SceneLoadSystenStateEventHandler OnLoadingStateChanged;
        public event SceneLoadSystenStateEventHandler OnTransitionStateChanged;


        protected override void OnReset()
        {
            base.OnReset();

            _loadAsync = null;
            _targetScene = null;
            _prevLoadingScene = null;
            _isInTransition = false;
            _isLoading = false;

            OnLoadStarted = null;
            OnLoadFinished = null;
            OnUnloadStarted = null;
            OnUnloadFinished = null;
            OnLoadingStateChanged = null;
            OnTransitionStateChanged = null;
        }

        public void LoadLoadingScene(SceneReferenceSO loadingScene = null, LoadSceneMode mode = LoadSceneMode.Single, System.Action onComplete = null)
        {
            if (_isLoading)
                return;
            
            _prevLoadingScene = loadingScene ? loadingScene : _loadingScene;

            IsLoading = true;

            LoadSceneAsync(_prevLoadingScene, mode, onComplete);
        }
        public void UnloadLoadingScene(System.Action onComplete = null)
        {
            if (!_isLoading)
                return;

            UnloadSceneAsync(_prevLoadingScene, () =>
            {
                onComplete?.Invoke();

                IsLoading = false;
                _prevLoadingScene = null;
            });
        }


        public void LoadSceneAsync(SceneReferenceSO scene, LoadSceneMode mode = LoadSceneMode.Single, System.Action onComplete = null)
        {
            if (IsInTransition || scene == null || string.IsNullOrEmpty(scene.SceneName))
                return;

            Log($"{nameof(LoadSceneAsync)} - Scene : {scene.SceneName} || Mode : {mode}");

            IsInTransition = true;
            _targetScene = scene;

            Invoke_OnLoadStarted(_targetScene);
            _loadAsync = SceneManager.LoadSceneAsync(scene.SceneName, mode);
            
            _loadAsync.completed += (_) =>
            {
                IsInTransition = false;
                var prevScene = _targetScene;
                _targetScene = null;
                _loadAsync = null;

                onComplete?.Invoke();
                Invoke_OnLoadFinished(prevScene);
            };
        }

        public void UnloadSceneAsync(SceneReferenceSO scene, System.Action onComplete = null)
        {
            if (_isInTransition || scene == null || string.IsNullOrEmpty(scene.SceneName))
                return;

            Log($"{nameof(UnloadSceneAsync)}");

            if (SceneManager.GetSceneByName(scene.SceneName).isLoaded)
            {
                _targetScene = scene;
                IsInTransition = true;

                Invoke_OnUnloadStarted(_targetScene);
                _loadAsync = SceneManager.UnloadSceneAsync(_targetScene.SceneName);
                _loadAsync.completed += (_) =>
                {
                    var prevScene = _targetScene;
                    _targetScene = null;
                    _loadAsync = null;
                    IsInTransition = false;

                    onComplete?.Invoke();
                    Invoke_OnUnloadFinished(prevScene);
                };
            }
        }

        public bool IsSceneLoaded(SceneReferenceSO sceneRef)
        {
            if (sceneRef == null) 
                return false;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == sceneRef.SceneName && scene.isLoaded)
                    return true;
            }
            return false;
        }

        private void Invoke_OnLoadStarted(SceneReferenceSO scene)
        {
            Log(nameof(OnLoadStarted));

            OnLoadStarted?.Invoke(this);
        }
        private void Invoke_OnLoadFinished(SceneReferenceSO scene)
        {
            Log(nameof(OnLoadFinished));

            OnLoadFinished?.Invoke(this);
        }
        private void Invoke_OnUnloadStarted(SceneReferenceSO scene)
        {
            Log(nameof(OnUnloadStarted));

            OnUnloadStarted?.Invoke(this);
        }
        private void Invoke_OnUnloadFinished(SceneReferenceSO scene)
        {
            Log(nameof(OnUnloadFinished));

            OnUnloadFinished?.Invoke(this);
        }

        private void Invoke_OnTransitionStateChanged()
        {
            Log(nameof(OnTransitionStateChanged));

            OnTransitionStateChanged?.Invoke(this, _isInTransition);
        }
        private void Invoke_OnLoadingStateChanged()
        {
            Log(nameof(OnLoadingStateChanged));

            OnLoadingStateChanged?.Invoke(this, _isLoading);
        }
    }
}
