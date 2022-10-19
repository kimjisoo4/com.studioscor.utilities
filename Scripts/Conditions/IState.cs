using UnityEngine.Events;

namespace KimScor.Utilities
{
    public interface IState
    {
        public event UnityAction OnStartedState;
        public event UnityAction OnFinishedState;

        public bool TryEnterState();
        public bool TryExitState();
        public bool CanEnterState();
        public bool CanExitState();
        public void OnState();
        public void UpdateState(float deltaTime);
        public void EndState(); 
    }
}