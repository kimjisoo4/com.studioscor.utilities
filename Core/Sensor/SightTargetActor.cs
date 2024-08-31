using UnityEngine;

namespace StudioScor.Utilities
{

    public interface ISightTarget
    {
        public delegate void SightStateEventHandler(ISightTarget sightTarget, ISightSensor sensor);

        public GameObject gameObject { get; }
        public Transform transform { get; }

        public void OnSightTarget();
        public void EndSightTarget();
        public bool CanSighting();

        public void DiscoverSensor(ISightSensor sensor);
        public void EscapeSensor(ISightSensor sensor);


        public event SightStateEventHandler OnDiscoveredSensor;
        public event SightStateEventHandler OnEscapedSensor;
    }

    public class SightTargetActor : BaseMonoBehaviour, ISightTarget
    {
        [Header(" [ Sight Target Actor ] ")]
        [SerializeField] private bool _autoPlaying = true;

        private bool _isPlaying = false;

        public event ISightTarget.SightStateEventHandler OnDiscoveredSensor;
        public event ISightTarget.SightStateEventHandler OnEscapedSensor;

        private void OnEnable()
        {
            if (_autoPlaying)
                OnSightTarget();
        }
        private void OnDisable()
        {
            EndSightTarget();
        }
        public bool CanSighting()
        {
            return _isPlaying;
        }

        public void OnSightTarget()
        {
            if (_isPlaying)
                return;

            _isPlaying = true;
        }
        public void EndSightTarget()
        {
            if (!_isPlaying)
                return;

            _isPlaying = false;
        }

        public void DiscoverSensor(ISightSensor sensor)
        {
            OnDiscoverSensor(sensor);

            Invoke_OnDiscoveredSensor(sensor);
        }

        public void EscapeSensor(ISightSensor sensor)
        {
            OnEscapeSensor(sensor);

            Invoke_OnEscapeddSensor(sensor);
        }

        protected virtual void OnDiscoverSensor(ISightSensor sensor)
        { }
        protected virtual void OnEscapeSensor(ISightSensor sensor)
        { }

    private void Invoke_OnDiscoveredSensor(ISightSensor sensor)
        {
            Log($"{OnDiscoveredSensor} - {sensor}");

            OnDiscoveredSensor?.Invoke(this, sensor);
        }
        private void Invoke_OnEscapeddSensor(ISightSensor sensor)
        {
            Log($"{OnEscapedSensor} - {sensor}");

            OnEscapedSensor?.Invoke(this, sensor);
        }
    }
}