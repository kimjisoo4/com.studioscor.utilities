using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public interface IState
    {
        public delegate void StateEventHandler(IState state);

        public bool TryEnterState();
        public bool TryExitState();
        public bool CanEnterState();
        public bool CanExitState();
        public void ForceEnterState();
        public void ForceExitState(); 

        public event StateEventHandler OnEnteredState;
        public event StateEventHandler OnExitedState;
    }
}