using UnityEngine.Events;
using UnityEngine;
using System.Collections;

namespace StudioScor.Utilities
{
    public interface IState
    {
        public event UnityAction OnEnteredState;
        public event UnityAction OnExitedState;

        public bool TryEnterState();
        public bool TryExitState();
        public bool CanEnterState();
        public bool CanExitState();
        public void ForceEnterState();
        public void ForceExitState(); 
    }
}