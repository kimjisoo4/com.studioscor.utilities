using UnityEngine.Events;
using UnityEngine;
using System.Collections;

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
        public void OnEnterState();
        public void OnExitState(); 
    }
}