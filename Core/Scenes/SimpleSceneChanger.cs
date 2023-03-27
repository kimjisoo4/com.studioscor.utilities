using UnityEngine;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
   

    public class SimpleSceneChanger : BaseMonoBehaviour
    {
        [Header(" [ Simple Scene Changer ]")]
        [SerializeField] private SceneLoader _Scene;
        [SerializeField] private bool _IsAutoPlaying = false;

        [SerializeField] private UnityEvent _OnStartedLoad;
        [SerializeField] private UnityEvent _OnFinishedLoad;

        public event UnityAction OnStartedLoad;
        public event UnityAction OnFinishedLoad;


        private void Start()
        {
            _Scene.OnStarted += Scene_OnStarted;
            _Scene.OnFinished += Scene_OnFinished;

            if (_IsAutoPlaying)
                LoadScene();
        }

        private void OnDestroy()
        {
            _Scene.OnStarted -= Scene_OnStarted;
            _Scene.OnFinished -= Scene_OnFinished;
        }

        public void LoadScene()
        {
            Log("Started Load Scene");

            _Scene.LoadScene();
        }

        private void Scene_OnStarted(SceneLoader scene)
        {
            Callback_OnStartedLoad();
        }

        private void Scene_OnFinished(SceneLoader scene)
        {
            Callback_OnFinishedLoad();
        }

        public void UnLoadScene()
        {
            Log("Un Load Scene");

            _Scene.UnLoadScene();
        }

        private void SimpleSceneChanger_Completed(AsyncOperation async)
        {
            async.completed -= SimpleSceneChanger_Completed;

            Callback_OnFinishedLoad();
        }

        private void Callback_OnFinishedLoad()
        {
            Log("On Finished Load Scene");

            _OnFinishedLoad?.Invoke();
            OnFinishedLoad?.Invoke();
        }

        private void Callback_OnStartedLoad()
        {
            Log("On Started Load Scene");

            _OnStartedLoad?.Invoke();
            OnStartedLoad?.Invoke();
        }
    }
}
