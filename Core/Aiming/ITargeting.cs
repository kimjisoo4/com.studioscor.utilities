using UnityEngine;


namespace StudioScor.Utilities
{
    public interface ITargeting
    {
        public bool CanTargeting { get; }
        public Transform Point { get; }
    }
}