using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public delegate void AiSensorEventHandler(ISight sightSensor, Collider sight);

    public interface ISight
    {
        public event AiSensorEventHandler OnFoundSight;
        public event AiSensorEventHandler OnLostedSight;
    }
    public class SightSensor : BaseMonoBehaviour, ISight
    {
        [Header(" [ Ai Sight Sensor ] ")]
        [SerializeField] private Transform _Owner;
        [SerializeField] private bool _UseRigidbody = true;
        [SerializeField] private float _Interval = 0.1f;
        [SerializeField] private float _Distance;
        [SerializeField, Range(0f, 360f)] private float _Angle;
        [SerializeField] private LayerMask _LayerMask;

        [Header(" [ Auto Playing ] ")]
        [SerializeField] private bool _AutoPlaying = true;

        private List<Collider> _HitResults = new();
        private readonly List<Collider> _Sights = new();
        private readonly List<Transform> _IgnoreTransforms = new();
        private bool _IsPlaying = false;
        private float _RemainTime;

        public event AiSensorEventHandler OnFoundSight;
        public event AiSensorEventHandler OnLostedSight;

        private void Reset()
        {
#if UNITY_EDITOR
            _Owner = transform;
#endif
        }
        private void OnEnable()
        {
            if (_AutoPlaying)
                OnSight();
        }

        private void OnDisable()
        {
            EndSight();
        }


        public void SetOwner(GameObject target)
        {
            SetOwner(target.transform);
        }
        public void SetOwner(Component component)
        {
            SetOwner(component.transform);
        }
        public void SetOwner(Transform target)
        {
            if (target == _Owner)
                return;

            _Owner = target;

            if(!_Owner)
            {
                _Owner = transform;
            }
        }

        public void AddIgnoreTransform(Transform add)
        {
            _IgnoreTransforms.Add(add);
        }
        public void AddIgnoreTransforms(IEnumerable<Transform> adds)
        {
            _IgnoreTransforms.AddRange(adds);
        }
        public void RemoveIgnoreTransform(Transform remove)
        {
            _IgnoreTransforms.Remove(remove);
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
            if (_IsPlaying)
                return;

            _IsPlaying = true;

            _HitResults.Clear();
            _Sights.Clear();
            _IgnoreTransforms.Clear();

            _IgnoreTransforms.Add(_Owner);

            _RemainTime = 0f;
        }
        public void EndSight()
        {
            if (!_IsPlaying)
                return;

            _IsPlaying = false;
        }

        private void FixedUpdate()
        {
            if (!_IsPlaying)
                return;

            float deltaTime = Time.fixedDeltaTime;

            UpdateSight(deltaTime);
        }

        public virtual void UpdateSight(float deltaTime)
        {
            if (!_IsPlaying)
                return;

            _RemainTime -= deltaTime;

            if (_RemainTime > 0f)
                return;

            _RemainTime = _Interval;

            Sight();
        }

        private void OnDrawGizmosSelected()
        {
            if (!UseDebug || _IsPlaying)
                return;

            SUtility.Debug.DrawCone(_Owner.position, _Owner.rotation, _Distance, _Angle, Color.red);
        }
        private void Sight()
        {
            _HitResults.Clear();

            if(!SUtility.Physics.DrawConeCast(_Owner, _Angle, _Distance, _LayerMask, ref _HitResults, _IgnoreTransforms, UseDebug, 0.1f))
            {
                if(_Sights.Count > 0)
                {
                    foreach (var sight in _Sights)
                    {
                        Callback_OnLostedSight(sight);
                    }

                    _Sights.Clear();
                }

                return;
            }

            if(_UseRigidbody)
            {
                for(int i = _HitResults.LastIndex(); i >= 0; i--)
                {
                    if (!_HitResults[i].attachedRigidbody)
                    {
                        _HitResults.RemoveAt(i);
                    }
                }
            }

            for (int i = _Sights.LastIndex(); i >= 0; i--)
            {
                var sight = _Sights[i];

                if (!_HitResults.Contains(sight))
                {
                    _Sights.RemoveAt(i);

                    Callback_OnLostedSight(sight);
                }
            }

            foreach (var hit in _HitResults)
            {
                if (!_Sights.Contains(hit))
                {
                    _Sights.Add(hit);

                    Callback_OnFoundSight(hit);
                }
            }
        }
        

        protected void Callback_OnFoundSight(Collider collider)
        {
            Log($"On Found Sight - [ {collider.name} ]");

            OnFoundSight?.Invoke(this, collider);
        }
        protected void Callback_OnLostedSight(Collider collider)
        {
            Log($"On Losted Sight - [ {collider.name} ]");

            OnLostedSight?.Invoke(this, collider);
        }
    }
}