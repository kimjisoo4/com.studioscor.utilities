using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace StudioScor.Utilities
{

    public class SceneLoadManager : Singleton<SceneLoadManager> 
    {
        #region Events
        public delegate void SceneLoadHandler(SceneLoadManager sceneLoadManager);
        #endregion

        [Header(" [ Scene Load Manager ] ")]
        [SerializeField] private LoadingScene _LoadingScene;
        [SerializeField] private SceneLoader _StartScene;
        [SerializeField] private float _LoadDelay = 0.5f;

        private SceneLoader _CurrentScene;

        private List<AsyncOperation> _SceneInstances;
        public float LoadDelay => _LoadDelay;

        public event SceneLoadHandler OnStartedSceneLoad;
        public event SceneLoadHandler OnFinishedSceneLoad;

        private void Start()
        {
            if (_StartScene)
                ForceLoadScene(_StartScene);
        }

        public void ForceLoadScene(SceneLoader sceneLoader)
        {
            StartCoroutine(LoadSceneTask(sceneLoader));
        }

        public IEnumerator LoadSceneTask(SceneLoader sceneLoader)
        {
            yield break;
        }



        private void OnStartSceneLoad()
        {
            OnStartedSceneLoad?.Invoke(this);
        }
        private void OnFinishSceneLoad()
        {
            OnFinishedSceneLoad?.Invoke(this);
        }
    }
}
