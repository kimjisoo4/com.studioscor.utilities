using UnityEngine;
using UnityEngine.SceneManagement;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new Load Scene Group", fileName = "SceneLoader_Group_")]
    public class LoadSceneGroup : SceneLoader
    {
        [Header(" [ Load Scene Group ] ")]
        [SerializeField] private SceneLoader mainScene;
        [SerializeField] private SceneLoader[] subScenes;

        public override Scene GetScene => mainScene.GetScene;

        private int _Count;

        protected override void OnReset()
        {
            base.OnReset();
        }


        public override void LoadScene()
        {
            IsPlaying = true;

            mainScene.OnLoadFinished += MainScene_OnFinished;

            mainScene.LoadScene();
        }

        private void MainScene_OnFinished(SceneLoader scene)
        {
            scene.OnLoadFinished -= MainScene_OnFinished;

            _Count = 0;

            foreach (var subScene in subScenes)
            {
                _Count++;

                subScene.OnLoadFinished += SubScene_OnFinished;

                subScene.LoadScene();
            }
        }

        private void SubScene_OnFinished(SceneLoader scene)
        {
            scene.OnLoadFinished -= SubScene_OnFinished;

            _Count--;

            if (_Count <= 0)
            {
                IsPlaying = false;

                Callback_OnFinished();
            }
        }

        public override void UnloadScene()
        {
            foreach (var scene in subScenes)
            {
                scene.UnloadScene();
            }

            mainScene.UnloadScene();
        }
    }
}
