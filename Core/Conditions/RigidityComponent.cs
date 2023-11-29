using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IRigidity
    {
        public delegate void RigidityHandler(IRigidity rigidity);

        public event RigidityHandler OnStartedRigidity;
        public event RigidityHandler OnFinishedRigidity;
        public void OnRigidity();
    }

    public class RigidityComponent : BaseMonoBehaviour, IRigidity
    {
        [Header(" [ Rigidity Component ] ")]
        [SerializeField] private bool useRigidity = true;
        [SerializeField] private float duration = 0.1f;

        private Timer timer = new();

        public event IRigidity.RigidityHandler OnStartedRigidity;
        public event IRigidity.RigidityHandler OnFinishedRigidity;

        private void Awake()
        {
            timer = new();

            timer.OnFinishedTimer += Timer_OnFinishedTimer;
        }

        private void Timer_OnFinishedTimer(Timer timer)
        {
            Invoke_OnFinishedRigidity();
        }

        public void OnRigidity()
        {
            if (!useRigidity)
                return;

            timer.OnTimer(duration);

            Invoke_OnStartedRigidity();
        }

        private void Update()
        {
            if (!timer.IsPlaying)
                return;

            float deltaTime = Time.deltaTime;

            timer.UpdateTimer(deltaTime);
        }

        #region Callback
        private void Invoke_OnStartedRigidity()
        {
            Log("On Start Rigidity");

            OnStartedRigidity?.Invoke(this);
        }
        private void Invoke_OnFinishedRigidity()
        {
            Log("On Finish Rigidity");

            OnFinishedRigidity?.Invoke(this);
        }
        #endregion
    }
}