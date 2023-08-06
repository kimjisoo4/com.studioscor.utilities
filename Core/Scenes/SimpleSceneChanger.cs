using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class SimpleSceneChanger : BaseMonoBehaviour
    {
        [Header(" [ Simple Scene Changer ]")]
        [SerializeField] private SceneLoader scene;
        [SerializeField] private bool isAutoPlaying = false;

        [SerializeField] private UnityEvent onStartedLoad;
        [SerializeField] private UnityEvent onFinishedLoad;

        public event UnityAction OnStartedLoad;
        public event UnityAction OnFinishedLoad;


        private void Start()
        {
            scene.OnStarted += Scene_OnStarted;
            scene.OnFinished += Scene_OnFinished;

            if (isAutoPlaying)
                LoadScene();
        }

        private void OnDestroy()
        {
            scene.OnStarted -= Scene_OnStarted;
            scene.OnFinished -= Scene_OnFinished;
        }

        public void LoadScene()
        {
            Log("Started Load Scene");

            scene.LoadScene();
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

            scene.UnLoadScene();
        }

        private void SimpleSceneChanger_Completed(AsyncOperation async)
        {
            async.completed -= SimpleSceneChanger_Completed;

            Callback_OnFinishedLoad();
        }

        private void Callback_OnFinishedLoad()
        {
            Log("On Finished Load Scene");

            onFinishedLoad?.Invoke();
            OnFinishedLoad?.Invoke();
        }

        private void Callback_OnStartedLoad()
        {
            Log("On Started Load Scene");

            onStartedLoad?.Invoke();
            OnStartedLoad?.Invoke();
        }
    }
}
