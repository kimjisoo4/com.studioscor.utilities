using UnityEngine;
using UnityEngine.SceneManagement;


namespace StudioScor.Utilities
{

    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new Load Scene Async", fileName = "SceneLoader_")]
    public class LoadSceneAsync : SceneLoader
    {
        [Header(" [ Load Scene Async ]")]
        [SerializeField] private SceneData scene;
        [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;

        public override Scene GetScene => scene.GetScene();


        private void OnValidate()
        {
#if UNITY_EDITOR
            scene.OnValidate();
#endif
        }

        protected override void OnReset()
        {
            base.OnReset();

#if UNITY_EDITOR
            scene.OnValidate();
#endif
        }


        public override void LoadScene()
        {
            IsPlaying = true;

            var async = scene.LoadScene(loadSceneMode);

            Callback_OnStarted();

            if (async is not null)
            {
                async.completed += Async_completed;
            }
            else
            {
                IsPlaying = false;

                Callback_OnFinished();
            }
        }

        private void Async_completed(AsyncOperation async)
        {
            IsPlaying = false;

            Callback_OnFinished();
        }

        public override void UnLoadScene()
        {
            scene.UnLoadScene();
        }

        
    }
}
