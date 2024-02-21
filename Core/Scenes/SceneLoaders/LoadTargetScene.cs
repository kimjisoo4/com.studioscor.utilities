using UnityEngine;
using UnityEngine.SceneManagement;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new Load Target Scene", fileName = "SceneLoader_Target")]
    public class LoadTargetScene : SceneLoader, ISerializationCallbackReceiver
    {
        [Header(" [ Load Target Scene ] ")]
        [SerializeField] private SceneLoader target;
        [SerializeField][SReadOnly]private SceneLoader runtimeTarget;

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
            runtimeTarget.OnStarted += Target_OnStarted;
            runtimeTarget.OnFinished += Target_OnFinished;

            runtimeTarget.LoadScene();
        }

        private void Target_OnStarted(SceneLoader scene)
        {
            scene.OnStarted -= Target_OnStarted;

            Callback_OnStarted();
        }
        private void Target_OnFinished(SceneLoader scene)
        {
            scene.OnFinished -= Target_OnFinished;

            Callback_OnFinished();
        }

        public override void UnLoadScene()
        {
            runtimeTarget.UnLoadScene();
        }

    }
}
