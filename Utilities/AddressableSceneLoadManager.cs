using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

using UnityEngine.SceneManagement;


namespace StudioScor.Utilities
{
    public class AddressableSceneLoadManager : Singleton<AddressableSceneLoadManager>
    {
        #region
        public delegate void SceneLoadHandler(AddressableSceneLoadManager sceneLoadManager);
        #endregion

        [Header(" [ Scene Load Manager ] ")]
        [SerializeField] private AssetReference _LoadingScene;
        [SerializeField] private AddressableSceneContainer _Start;

        [SerializeField] private float _LoadDelay = 0.5f;

        [SerializeField] private AddressableSceneContainer _TestScene;
        [SerializeField] private bool _LoadTest = false;
        private bool _Toggle = false;

        private AddressableSceneContainer _CurrentScenes;

        private SceneInstance _LoadingSceneInstance;
        private List<SceneInstance> _SceneInstances;

        private bool HasPrevScene => _SceneInstances is not null && _SceneInstances.Count > 0;
        public float LoadDelay => _LoadDelay;

        public event SceneLoadHandler OnStartedSceneLoad;
        public event SceneLoadHandler OnFinishedSceneLoad;
         

        private void Update()
        {
            if (_LoadTest)
            {
                _LoadTest = false;
                _Toggle = !_Toggle;

                if(_Toggle)
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

        public void ForceLoadScene(AddressableSceneContainer targetSceneContainer)
        {
            StartCoroutine(LoadSceneTask(targetSceneContainer));
        }
        public void UnLoadScene()
        {
            StartCoroutine(UnLoadAsync());
        }
        
        private IEnumerator LoadSceneTask(AddressableSceneContainer targetSceneContainer)
        {
            OnStartSceneLoad();

            Log(" On Start Delay - " + LoadDelay);
            yield return new WaitForSeconds(LoadDelay);
            Log(" On Finish Delay ");

            yield return StartCoroutine(LoadLoadingSceneAsync());
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(UnLoadAsync());
            yield return new WaitForSeconds(1f);
            _CurrentScenes = targetSceneContainer;

            yield return StartCoroutine(LoadAsync());
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(UnLoadLoadingSceneAsync());

            OnFinishSceneLoad();
        }

        private IEnumerator LoadLoadingSceneAsync()
        {
            if(!_LoadingScene.IsDone)
            {
                Log(" Not has Loading Scene ");
                yield break;
            }

            Log(" Start Loading Scene Load");

            LoadSceneMode loadMode = HasPrevScene ? LoadSceneMode.Additive : LoadSceneMode.Single;

            var aysnc = _LoadingScene.LoadSceneAsync(loadMode);

            while(aysnc.Status != AsyncOperationStatus.Succeeded)
            {
                yield return null;
            }

            _LoadingSceneInstance = aysnc.Result;

            Log(" Complate Loading Scene Load");
        }

        private IEnumerator UnLoadLoadingSceneAsync()
        {

            Log(" Start Loading Scene UnLoad");

            var async = Addressables.UnloadSceneAsync(_LoadingSceneInstance, false);

            while (async.Status != AsyncOperationStatus.Succeeded)
            {
                yield return null;
            }

            _LoadingSceneInstance = default;

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

            foreach (var sceneInstances in _SceneInstances)
            {
                var async = Addressables.UnloadSceneAsync(sceneInstances, false);

                Log(" Start UnLoad Scene - " + async.DebugName);

                while (async.Status != AsyncOperationStatus.Succeeded)
                {
                    yield return null;
                }

                Log(" Finish UnLoad Scene - " + async.DebugName);

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
                var async = scene.LoadSceneAsync(LoadSceneMode.Additive);

                while (async.Status != AsyncOperationStatus.Succeeded)
                {
                    Log(" Load Scene - " + (async.PercentComplete * 100f) + "%");

                    yield return null;
                }

                Log("Finish Load Scene - " + async.DebugName);

                _SceneInstances.Add(async.Result);

                yield return null;
            }

            Log(" Scene Load Complate" );
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
