using UnityEngine;
using UnityEngine.SceneManagement;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new Load Scene Group", fileName = "SceneLoader_Group_")]
    public class LoadSceneGroup : SceneLoader
    {
        [Header(" [ Load Scene Group ] ")]
        [SerializeField] private SceneLoader _MainScene;
        [SerializeField] private SceneLoader[] _SubScenes;

        public override Scene GetScene => _MainScene.GetScene;

        private int _Count;

        protected override void OnReset()
        {
            base.OnReset();
        }


        public override void LoadScene()
        {
            _MainScene.OnFinished += MainScene_OnFinished;

            _MainScene.LoadScene();
        }

        private void MainScene_OnFinished(SceneLoader scene)
        {
            scene.OnFinished -= MainScene_OnFinished;

            _Count = 0;

            foreach (var subScene in _SubScenes)
            {
                _Count++;

                subScene.OnFinished += SubScene_OnFinished;

                subScene.LoadScene();
            }
        }

        private void SubScene_OnFinished(SceneLoader scene)
        {
            scene.OnFinished -= SubScene_OnFinished;

            _Count--;

            if (_Count <= 0)
                Callback_OnFinished();
        }

        public override void UnLoadScene()
        {
            foreach (var scene in _SubScenes)
            {
                scene.UnLoadScene();
            }

            _MainScene.UnLoadScene();
        }
    }
}
