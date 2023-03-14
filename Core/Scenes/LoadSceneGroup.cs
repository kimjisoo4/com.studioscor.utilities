using UnityEngine;
using UnityEngine.SceneManagement;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new Load Scene Group", fileName = "SceneLoader_Group_")]
    public class LoadSceneGroup : SceneLoader
    {
        [Header(" [ Load Scene Group ] ")]
        [SerializeField] private SceneData _MainScene;
        [SerializeField] private SceneData[] _SubScenes;

        private int _Count;

        private void OnValidate()
        {
#if UNITY_EDITOR
            _MainScene.OnValidate();

            if(_SubScenes is not null)
            {
                foreach (var scene in _SubScenes)
                {
                    scene.OnValidate();
                }
            }
#endif
        }

        protected override void OnReset()
        {
            base.OnReset();

#if UNITY_EDITOR
            _MainScene.OnValidate();

            if (_SubScenes is not null)
            {
                foreach (var scene in _SubScenes)
                {
                    scene.OnValidate();
                }
            }
#endif
        }


        public override void LoadScene()
        {
           var async =  _MainScene.LoadScene();

            if (async is null)
                LoadSubScenes();
            else
                async.completed += Async_Completed;
        }

        public override void UnLoadScene()
        {
            foreach (var scene in _SubScenes)
            {
                scene.UnLoadScene();
            }

            _MainScene.UnLoadScene();
        }

        private void Async_Completed(AsyncOperation async)
        {
            LoadSubScenes();
        }

        private void LoadSubScenes()
        {
            OnLoadSubScenes();

            if (_Count == 0)
                Callback_OnFinished();
        }


        private void OnLoadSubScenes()
        {
            _Count = _SubScenes.Length;

            foreach (var scene in _SubScenes)
            {
                var async = scene.LoadScene(LoadSceneMode.Additive);

                if(async is null)
                {
                    _Count--;
                }
                else
                {
                    async.completed += SubSceneAsync_Completed;
                }
            }
        }

        private void SubSceneAsync_Completed(AsyncOperation obj)
        {
            _Count--;

            if (_Count == 0)
                Callback_OnFinished();
        }
    }
}
