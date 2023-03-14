using UnityEngine;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new Load Target Scene", fileName = "SceneLoader_Target")]
    public class LoadTargetScene : SceneLoader, ISerializationCallbackReceiver
    {
        [Header(" [ Load Target Scene ] ")]
        [SerializeField] private SceneLoader _Target;

        private SceneLoader _RuntimeTarget;

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

            _RuntimeTarget = _Target;
        }
        


        public void SetTarget(SceneLoader target)
        {
            _RuntimeTarget = target;
        }

        public override void LoadScene()
        {
            _RuntimeTarget.OnStarted += Target_OnStarted;
            _RuntimeTarget.OnFinished += Target_OnFinished;

            _RuntimeTarget.LoadScene();
        }

        private void Target_OnStarted()
        {
            _RuntimeTarget.OnStarted -= Target_OnStarted;

            Callback_OnStarted();
        }
        private void Target_OnFinished()
        {
            _RuntimeTarget.OnFinished -= Target_OnFinished;

            Callback_OnFinished();
        }

        public override void UnLoadScene()
        {
            _RuntimeTarget.UnLoadScene();
        }

        
    }
}
