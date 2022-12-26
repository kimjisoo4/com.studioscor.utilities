using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.BodySystem
{
   
    public class BodySystem : MonoBehaviour
    {
        #region Events
        public delegate void ChangedBodyPartHandler(BodySystem bodySystem, Body body, BodyPartComponent transform);
        #endregion

        [SerializeField] private Dictionary<Body, BodyPartComponent> _BodyParts;

        public IReadOnlyDictionary<Body, BodyPartComponent> BodyParts
        {
            get
            {
                if (_BodyParts == null)
                {
                    _BodyParts = new Dictionary<Body, BodyPartComponent>();
                }

                return _BodyParts;
            }
        }

        private bool _WasSetup = false;

        public event ChangedBodyPartHandler OnAddedBodyPart;
        public event ChangedBodyPartHandler OnRemovedBodyPart;

        private void Awake()
        {
            if (!_WasSetup)
            {
                Setup();
            }
        }
        private void Setup()
        {
            _WasSetup = true;

            if (_BodyParts == null)
            {
                _BodyParts = new Dictionary<Body, BodyPartComponent>();
            }
        }

        public bool TryGetBodyPart(Body body, out BodyPartComponent bodyPart)
        {
            return BodyParts.TryGetValue(body, out bodyPart);
        }
        public BodyPartComponent TryGetBodyPart(Body body)
        {
            if(BodyParts.TryGetValue(body, out BodyPartComponent bodyPart))
            {
                return bodyPart;
            }
            else
            {
                return null;
            }
        }

        public bool TryAddBodyPart(Body body, BodyPartComponent transform)
        {
            if (!_WasSetup)
                Setup();

            if(_BodyParts.TryAdd(body, transform))
            {
                OnAddBodyPart(body, transform);

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool TryRemoveBodyPart(Body body)
        {
            if (!_WasSetup)
                Setup();

            if (_BodyParts.TryGetValue(body, out BodyPartComponent value))
            {
                _BodyParts.Remove(body);

                OnRemoveBodyPart(body, value);

                return true;
            }
            else
            {
                return false;
            }
        }

        #region Events CallBack
        public void OnAddBodyPart(Body body, BodyPartComponent transform)
        {
            OnAddedBodyPart?.Invoke(this, body, transform);
        }
        public void OnRemoveBodyPart(Body body, BodyPartComponent transform)
        {
            OnRemovedBodyPart?.Invoke(this, body, transform);
        }
        #endregion
    }

}
