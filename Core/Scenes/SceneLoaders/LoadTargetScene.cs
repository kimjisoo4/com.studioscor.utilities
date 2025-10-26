using UnityEngine;
using UnityEngine.SceneManagement;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new Load Target Scene", fileName = "SceneLoader_Target")]
    public class LoadTargetScene : SceneLoader, ISerializationCallbackReceiver
    {
        [Header(" [ Load Target Scene ] ")]
        [SerializeField] private SceneLoader target;
        [SerializeField] private SceneLoader runtimeTarget;

        public override Scene GetScene => runtimeTarget.GetScene;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            OnReset();
        }

        protected override void OnReset()
        {
            base.OnReset();

            runtimeTarget = target;
        }
        


        public void SetTarget(SceneLoader target)
        {
            runtimeTarget = target;
        }

        public override void LoadScene()
        {
            IsPlaying = true;

            runtimeTarget.OnLoadStarted += Target_OnStarted;
            runtimeTarget.OnLoadFinished += Target_OnFinished;

            runtimeTarget.LoadScene();
        }

        private void Target_OnStarted(SceneLoader scene)
        {
            scene.OnLoadStarted -= Target_OnStarted;

            Callback_OnStarted();
        }
        private void Target_OnFinished(SceneLoader scene)
        {
            scene.OnLoadFinished -= Target_OnFinished;

            IsPlaying = false;

            Callback_OnFinished();
        }

        public override void UnloadScene()
        {
            runtimeTarget.UnloadScene();
        }

    }
}
