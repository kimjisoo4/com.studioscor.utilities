using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace StudioScor.Utilities
{
    public class SceneLoadManager : Singleton<SceneLoadManager> 
    {
        #region
        public delegate void SceneLoadHandler(SceneLoadManager sceneLoadManager);
        #endregion

        [Header(" [ Scene Load Manager ] ")]
        [SerializeField] private string _LoadingScene;
        [SerializeField] private SceneContainer _Start;

        [SerializeField] private float _LoadDelay = 0.5f;

        [SerializeField] private SceneContainer _TestScene;
        [SerializeField] private bool _LoadTest = false;
        private bool _Toggle = false;

        private SceneContainer _CurrentScenes;

        private List<AsyncOperation> _SceneInstances;

        private bool HasPrevScene => _CurrentScenes;
        public float LoadDelay => _LoadDelay;

        public event SceneLoadHandler OnStartedSceneLoad;
        public event SceneLoadHandler OnFinishedSceneLoad;


        private void Update()
        {
            if (_LoadTest)
            {
                _LoadTest = false;
                _Toggle = !_Toggle;

                if (_Toggle)
                {
                    ForceLoadScene(_TestScene);
                }
                else
                {
                    ForceLoadScene(_Start);
                }
            }
        }
        protected override void Setup()
        {
            if (_Start)
                ForceLoadScene(_Start);
        }

        public void ForceLoadScene(SceneContainer targetSceneContainer)
        {
            StartCoroutine(LoadSceneTask(targetSceneContainer));
        }
        public void UnLoadScene()
        {
            StartCoroutine(UnLoadAsync());
        }

        private IEnumerator LoadSceneTask(SceneContainer targetSceneContainer)
        {
            OnStartSceneLoad();

            Log(" On Start Delay - " + LoadDelay);
            yield return new WaitForSeconds(LoadDelay);
            Log(" On Finish Delay ");

            yield return StartCoroutine(LoadLoadingSceneAsync());
            yield return StartCoroutine(UnLoadAsync());

            Resources.UnloadUnusedAssets();
            _CurrentScenes = targetSceneContainer;

            yield return StartCoroutine(LoadAsync());
            yield return StartCoroutine(UnLoadLoadingSceneAsync());

            OnFinishSceneLoad();
        }

        private IEnumerator LoadLoadingSceneAsync()
        {
            if (_LoadingScene.Equals(""))
            {
                Log(" Not has Loading Scene ");
                yield break;
            }

            Log(" Start Loading Scene Load");

            LoadSceneMode loadMode = HasPrevScene ? LoadSceneMode.Additive : LoadSceneMode.Single;

            var async = SceneManager.LoadSceneAsync(_LoadingScene, loadMode);

            async.allowSceneActivation = false;

            while (async.progress < 0.9f)
            {
                Log(async.progress);
                yield return null;
            }

            async.allowSceneActivation = true;

            Log(" Complate Loading Scene Load");
        }

        private IEnumerator UnLoadLoadingSceneAsync()
        {

            Log(" Start Loading Scene UnLoad");

            var async = SceneManager.UnloadSceneAsync(_LoadingScene);

            async.allowSceneActivation = false;

            while (async.progress < 0.9f)
            {
                yield return null;
            }

            async.allowSceneActivation = true;

            Log(" Complate Loading SCene UnLoad");
        }


        private IEnumerator UnLoadAsync()
        {
            if (_SceneInstances is null)
            {
                Log(" Not has UnLoad Scene ");
                yield break;
            }

            Log(" UnLoad Scene - Scene Count : " + _SceneInstances.Count);

            foreach (var scene in _CurrentScenes.Scenes)
            {
                var async = SceneManager.UnloadSceneAsync(scene);

                async.allowSceneActivation = false;

                Log(" Start UnLoad Scene - " + scene);

                while (async.progress < 0.9f)
                {
                    yield return null;
                }

                async.allowSceneActivation = true;

                Log(" Finish UnLoad Scene - " + scene);

                yield return null;
            }

            _SceneInstances = null;

            Log(" Complate Un Load Scene ");
        }

        private IEnumerator LoadAsync()
        {
            Log(" Load Scenes ");

            _SceneInstances = new();

            foreach (var scene in _CurrentScenes.Scenes)
            {
                var async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

                while (async.progress < 0.9f)
                {
                    Log(" Load Scene - " + scene + " ( " + (async.progress * 100f) + "% )");

                    yield return null;
                }

                Log("Finish Load Scene - " + scene);

                _SceneInstances.Add(async);

                yield return null;
            }

            Log(" Scene Load Complate");
        }

        private void OnStartSceneLoad()
        {
            Log(" On Started Scene Load - " + _CurrentScenes);

            OnStartedSceneLoad?.Invoke(this);
        }
        private void OnFinishSceneLoad()
        {
            Log(" On Finished Scene Load - " + _CurrentScenes);

            OnFinishedSceneLoad?.Invoke(this);
        }
    }
}
