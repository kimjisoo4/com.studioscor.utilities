using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimScor.BodySystem
{
   
    public class BodySystem : MonoBehaviour
    {
        #region Events
        public delegate void ChangedBodyPartHandler(BodySystem bodySystem, Body body, BodyPart transform);
        #endregion

        [SerializeField] private Dictionary<Body, BodyPart> _BodyParts;

        public IReadOnlyDictionary<Body, BodyPart> BodyParts
        {
            get
            {
                if (_BodyParts == null)
                {
                    _BodyParts = new Dictionary<Body, BodyPart>();
                }

                return _BodyParts;
            }
        }

        private bool _IsInitialization = false;

        public event ChangedBodyPartHandler OnAddedBodyPart;
        public event ChangedBodyPartHandler OnRemovedBodyPart;

        private void Awake()
        {
            if (!_IsInitialization)
            {
                Setup();
            }
        }
        private void Setup()
        {
            _IsInitialization = true;

            if (_BodyParts == null)
            {
                _BodyParts = new Dictionary<Body, BodyPart>();
            }
        }

        public bool TryGetBodyPart(Body body, out BodyPart bodyPart)
        {
            return BodyParts.TryGetValue(body, out bodyPart);
        }
        public BodyPart TryGetBodyPart(Body body)
        {
            if(BodyParts.TryGetValue(body, out BodyPart bodyPart))
            {
                return bodyPart;
            }
            else
            {
                return null;
            }
        }

        public bool TryAddBodyPart(Body body, BodyPart transform)
        {
            if (!_IsInitialization)
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
            if (!_IsInitialization)
                Setup();

            if (_BodyParts.TryGetValue(body, out BodyPart value))
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
        public void OnAddBodyPart(Body body, BodyPart transform)
        {
            OnAddedBodyPart?.Invoke(this, body, transform);
        }
        public void OnRemoveBodyPart(Body body, BodyPart transform)
        {
            OnRemovedBodyPart?.Invoke(this, body, transform);
        }
        #endregion
    }

}
