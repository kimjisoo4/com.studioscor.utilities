using UnityEngine.Events;
using UnityEngine;
using System.Collections;

namespace StudioScor.Utilities
{
    public interface IState
    {
        public event UnityAction<IState> OnEnteredState;
        public event UnityAction<IState> OnExitedState;

        public bool TryEnterState();
        public bool TryExitState();
        public bool CanEnterState();
        public bool CanExitState();
        public void ForceEnterState();
        public void ForceExitState(); 
    }
}