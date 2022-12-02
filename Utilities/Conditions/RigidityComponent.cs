using UnityEngine;
using System.Collections.Generic;


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
        [SerializeField] private bool _UseRigidity = true;
        [SerializeField] private float _Duration = 0.1f;

        private Timer _Timer = new();

        public event IRigidity.RigidityHandler OnStartedRigidity;
        public event IRigidity.RigidityHandler OnFinishedRigidity;

        private void Awake()
        {
            _Timer = new();

            _Timer.OnFinishedTimer += Timer_OnFinishedTimer;
        }

        private void Timer_OnFinishedTimer(Timer timer)
        {
            OnFinishRigidity();
        }

        public void OnRigidity()
        {
            if (!_UseRigidity)
                return;

            _Timer.OnTimer(_Duration);

            OnStartRigidity();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            _Timer.UpdateTimer(deltaTime);
        }

        #region Callback
        private void OnStartRigidity()
        {
            Log("On Start Rigidity");

            OnStartedRigidity?.Invoke(this);
        }
        private void OnFinishRigidity()
        {
            Log("On Finish Rigidity");

            OnFinishedRigidity?.Invoke(this);
        }
        #endregion
    }
}