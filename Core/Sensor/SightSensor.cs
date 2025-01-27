using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public interface ISightSensor
    {
        public delegate void AiSensorEventHandler(ISightSensor sightSensor, ISightTarget sight);
        public bool IsPlaying { get; }
        public GameObject Owner { get; }
        public IReadOnlyList<ISightTarget> SightTargets { get; }

        public void SetOwner(GameObject owner);
        public void AddIgnoreTransform(Transform transform);
        public void RemoveIgnoreTransform(Transform transform);
        public void OnSight();
        public void EndSight();
        public void UpdateSight(float deltaTime);
        
        public event AiSensorEventHandler OnFoundSight;
        public event AiSensorEventHandler OnLostedSight;
    }

    public class SightSensor : BaseMonoBehaviour, ISightSensor
    {
        [Header(" [ Ai Sight Sensor ] ")]
        [SerializeField] private GameObject _owner;
        [SerializeField][Min(0f)] private float _interval = 0.1f;
        [SerializeField][Min(0f)] private float _distance = 10f;
        [SerializeField][Range(0f, 360f)] private float _angle = 90f;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private int _sightCount = 20;

        [Header(" Auto Playing ")]
        [SerializeField] private bool _isAutoPlaying = true;

        private Collider[] _hitResults;
        private readonly List<ISightTarget> _sightTargets = new();
        private readonly List<Transform> _ignoreTransforms = new();
        private bool _isPlaying = false;
        private float _remainTime;

        public GameObject Owner => _owner;
        public bool IsPlaying => _isPlaying;
        public IReadOnlyList<ISightTarget> SightTargets => _sightTargets;

        public event ISightSensor.AiSensorEventHandler OnFoundSight;
        public event ISightSensor.AiSensorEventHandler OnLostedSight;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if(!_owner)
            {
                _owner = gameObject;
            }
#endif
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (!UseDebug || IsPlaying)
                return;

            if (!_owner)
                return;

            if (_sightTargets is null || _sightTargets.Count <= 0)
            {
                Color color = Color.red;
                color.a = 0.5f;

                SUtility.Debug.DrawCone(_owner.transform.position, _owner.transform.rotation, _distance, _angle, color);
            }
            else
            {
                Color color = Color.green;
                color.a = 0.5f;

                SUtility.Debug.DrawCone(_owner.transform.position, _owner.transform.rotation, _distance, _angle, color);

                foreach (var sight in _sightTargets)
                {
                    SUtility.Debug.DrawPoint(sight.transform.position, color);
                }
            }

            
#endif
        }
        private void Awake()
        {
            if (!_owner)
                _owner = gameObject;
        }

        private void OnEnable()
        {
            if (_isAutoPlaying)
                OnSight();
        }
        private void OnDisable()
        {
            EndSight();
        }

        public void SetOwner(GameObject target)
        {
            if (target == _owner)
                return;

            _owner = target;

            if (!_owner)
            {
                _owner = target;
            }
        }
        public void SetOwner(Component component)
        {
            SetOwner(component.gameObject);
        }

        public void AddIgnoreTransform(Transform add)
        {
            _ignoreTransforms.Add(add);
        }
        public void AddIgnoreTransforms(IEnumerable<Transform> adds)
        {
            _ignoreTransforms.AddRange(adds);
        }
        public void RemoveIgnoreTransform(Transform remove)
        {
            _ignoreTransforms.Remove(remove);
        }
        public void RemoveIgnoreTransforms(IEnumerable<Transform> removes)
        {
            foreach (var remove in removes)
            {
                RemoveIgnoreTransform(remove);
            }
        }

        public void OnSight()
        {
            if (_isPlaying)
                return;

            _isPlaying = true;

            if(_hitResults is null || _hitResults.Length != _sightCount)
            {
                _hitResults = new Collider[_sightCount];
            }

            _sightTargets.Clear();
            _ignoreTransforms.Clear();

            _ignoreTransforms.Add(_owner.transform);

            _remainTime = 0f;
        }
        public void EndSight()
        {
            if (!_isPlaying)
                return;

            _isPlaying = false;
        }

        public virtual void UpdateSight(float deltaTime)
        {
            if (!_isPlaying)
                return;

            _remainTime -= deltaTime;

            if (_remainTime > 0f)
                return;

            _remainTime = _interval;

            Sight();
        }

        
        private void Sight()
        {
            var hitCount = SUtility.Physics.DrawConeCastNonAlloc(_owner.transform.position, _owner.transform.forward, _angle, _distance, _layerMask, _hitResults, UseDebug, 0.1f);

            if (hitCount == 0)
            {
                if(_sightTargets.Count > 0)
                {
                    foreach (var sight in _sightTargets)
                    {
                        Invoke_OnLostedSight(sight);
                    }

                    _sightTargets.Clear();
                }

                return;
            }
            else
            {
                for (int i = _sightTargets.LastIndex(); i >= 0; i--)
                {
                    var sight = _sightTargets[i];

                    if(!sight.CanSighting())
                    {
                        _sightTargets.RemoveAt(i);
                        
                        sight.EscapeSensor(this);

                        Invoke_OnLostedSight(sight);
                    }
                    else
                    {
                        bool isFind = false;

                        for (int j = 0; j < hitCount; j++)
                        {
                            var hit = _hitResults[j];

                            if (hit.transform == sight.transform)
                            {
                                isFind = true;
                                break;
                            }
                        }

                        if (!isFind)
                        {
                            _sightTargets.RemoveAt(i);

                            sight.EscapeSensor(this);

                            Invoke_OnLostedSight(sight);
                        }
                    }
                }

                for(int i = 0; i < hitCount; i++)
                {
                    var hit = _hitResults[i];

                    Log($"Sighting - {hit.transform}");

                    if (_ignoreTransforms.Contains(hit.transform))
                        continue;

                    if(hit.transform.TryGetComponent(out ISightTarget sightTarget) && !_sightTargets.Contains(sightTarget) && sightTarget.CanSighting())
                    {
                        _sightTargets.Add(sightTarget);

                        sightTarget.DiscoverSensor(this);

                        Callback_OnFoundSight(sightTarget);
                    }
                }
            }
        }
        

        protected virtual void Callback_OnFoundSight(ISightTarget findSightTarget)
        {
            Log($"{nameof(OnFoundSight)} - [ {findSightTarget.transform.name} ]");

            OnFoundSight?.Invoke(this, findSightTarget);
        }
        protected virtual void Invoke_OnLostedSight(ISightTarget lostSightTarget)
        {
            Log($"{nameof(OnLostedSight)} - [ {lostSightTarget.transform.name} ]");

            OnLostedSight?.Invoke(this, lostSightTarget);
        }
    }
}