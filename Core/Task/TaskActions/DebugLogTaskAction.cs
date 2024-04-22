using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class DebugLogTaskAction : TaskAction
    {
        [Header(" [ Debug Log Action ] ")]
        [SerializeField] private string _message = "Debug";

        private DebugLogTaskAction _original;

        public override void Action(GameObject target)
        {
            Debug.Log($"{GetType()} :: {(_original is null ? _message : _original._message)} :: {target}");
        }

        public override ITaskAction Clone()
        {
            var clone = new DebugLogTaskAction();

            clone._original = this;

            return clone;
        }
    }
}
