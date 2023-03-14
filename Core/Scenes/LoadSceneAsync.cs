using UnityEngine;
using UnityEngine.SceneManagement;


namespace StudioScor.Utilities
{

    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new Load Scene Async", fileName = "SceneLoader_")]
    public class LoadSceneAsync : SceneLoader
    {
        [Header(" [ Load Scene Async ]")]
        [SerializeField] private SceneData _Scene;
        [SerializeField] private LoadSceneMode _Mode = LoadSceneMode.Single;

        private void OnValidate()
        {
#if UNITY_EDITOR
            _Scene.OnValidate();
#endif
        }

        protected override void OnReset()
        {
            base.OnReset();

#if UNITY_EDITOR
            _Scene.OnValidate();
#endif
        }


        public override void LoadScene()
        {
            var async = _Scene.LoadScene(_Mode);

            Callback_OnStarted();

            if (async is not null)
                async.completed += Async_completed;
            else
                Callback_OnFinished();
        }

        private void Async_completed(AsyncOperation async)
        {
            Callback_OnFinished();
        }

        public override void UnLoadScene()
        {
            _Scene.UnLoadScene();
        }

        
    }
}
