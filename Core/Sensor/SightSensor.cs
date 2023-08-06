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
        [SerializeField] private Transform owner;
        [SerializeField] private bool useRigidbody = true;
        [SerializeField] private float interval = 0.1f;
        [SerializeField] private float distance = 10f;
        [SerializeField, Range(0f, 360f)] private float angle = 90f;
        [SerializeField] private LayerMask layerMask;

        [Header(" [ Auto Playing ] ")]
        [SerializeField] private bool isAutoPlaying = true;

        private List<Collider> hitResults = new();
        private readonly List<Collider> sights = new();
        private readonly List<Transform> ignoreTransforms = new();
        private bool isPlaying = false;
        private float remainTime;

        public event AiSensorEventHandler OnFoundSight;
        public event AiSensorEventHandler OnLostedSight;

        private void Reset()
        {
#if UNITY_EDITOR
            owner = transform;
#endif
        }
        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (!UseDebug || isPlaying)
                return;

            SUtility.Debug.DrawCone(owner.position, owner.rotation, distance, angle, Color.red);
#endif
        }

        private void OnEnable()
        {
            if (isAutoPlaying)
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
            if (target == owner)
                return;

            owner = target;

            if(!owner)
            {
                owner = transform;
            }
        }

        public void AddIgnoreTransform(Transform add)
        {
            ignoreTransforms.Add(add);
        }
        public void AddIgnoreTransforms(IEnumerable<Transform> adds)
        {
            ignoreTransforms.AddRange(adds);
        }
        public void RemoveIgnoreTransform(Transform remove)
        {
            ignoreTransforms.Remove(remove);
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
            if (isPlaying)
                return;

            isPlaying = true;

            hitResults.Clear();
            sights.Clear();
            ignoreTransforms.Clear();

            ignoreTransforms.Add(owner);

            remainTime = 0f;
        }
        public void EndSight()
        {
            if (!isPlaying)
                return;

            isPlaying = false;
        }

        private void FixedUpdate()
        {
            if (!isPlaying)
                return;

            float deltaTime = Time.fixedDeltaTime;

            UpdateSight(deltaTime);
        }

        public virtual void UpdateSight(float deltaTime)
        {
            if (!isPlaying)
                return;

            remainTime -= deltaTime;

            if (remainTime > 0f)
                return;

            remainTime = interval;

            Sight();
        }

        
        private void Sight()
        {
            hitResults.Clear();

            if(!SUtility.Physics.DrawConeCast(owner, angle, distance, layerMask, ref hitResults, ignoreTransforms, UseDebug, 0.1f))
            {
                if(sights.Count > 0)
                {
                    foreach (var sight in sights)
                    {
                        Callback_OnLostedSight(sight);
                    }

                    sights.Clear();
                }

                return;
            }

            if(useRigidbody)
            {
                for(int i = hitResults.LastIndex(); i >= 0; i--)
                {
                    if (!hitResults[i].attachedRigidbody)
                    {
                        hitResults.RemoveAt(i);
                    }
                }
            }

            for (int i = sights.LastIndex(); i >= 0; i--)
            {
                var sight = sights[i];

                if (!hitResults.Contains(sight))
                {
                    sights.RemoveAt(i);

                    Callback_OnLostedSight(sight);
                }
            }

            foreach (var hit in hitResults)
            {
                if (!sights.Contains(hit))
                {
                    sights.Add(hit);

                    Callback_OnFoundSight(hit);
                }
            }
        }
        

        protected virtual void Callback_OnFoundSight(Collider collider)
        {
            Log($"On Found Sight - [ {collider.name} ]");

            OnFoundSight?.Invoke(this, collider);
        }
        protected virtual void Callback_OnLostedSight(Collider collider)
        {
            Log($"On Losted Sight - [ {collider.name} ]");

            OnLostedSight?.Invoke(this, collider);
        }
    }
}