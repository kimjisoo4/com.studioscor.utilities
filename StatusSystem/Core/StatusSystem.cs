using UnityEngine;
using System.Collections.Generic;

namespace StudioScor.StatusSystem
{
    public class StatusSystem : MonoBehaviour
    {
        #region Events
        public delegate void StatusEventHandler(StatusSystem statusSystem, Status status);
        #endregion

        private Dictionary<StatusTag, Status> _Statuses = new Dictionary<StatusTag, Status>();

        private bool _WasSetup = false;
        public IReadOnlyDictionary<StatusTag, Status> Statuses
        {
            get
            {
                Setup();

                return _Statuses;
            }
        }

        public event StatusEventHandler OnAddedStatus;

        private void Awake()
        {
            Setup();
        }

        protected virtual void Setup()
        {
            if (_WasSetup)
                return;

            _WasSetup = true;

            _Statuses = new();
        }

        public Status CreateOrSetValue(StatusTag Tag, float minValue, float maxValue)
        {
            if (!Statuses.TryGetValue(Tag, out Status value))
            {
                value = new Status(Tag, minValue, maxValue);

                _Statuses.Add(Tag, value);

                OnAddStatus(value);
            }
            else
            {
                value.SetPoint(minValue, maxValue);
            }

            return value;
        }

        public Status GetValue(StatusTag Tag)
        {
            if (!Tag)
            {
                return null;
            }

            if (Statuses.TryGetValue(Tag, out Status value))
                return value;

            return null;
        }

        public Status GetOrCreateValue(StatusTag Tag, float minValue = 0f, float maxValue = 0f)
        {
            if (!Tag)
            {
                return null;
            }

            if (Statuses.TryGetValue(Tag, out Status value))
                return value;
            else
            {
                value = new Status(Tag, minValue, maxValue);

                _Statuses.Add(Tag, value);

                OnAddStatus(value);

                return value;
            }
        }
        public Status GetOrCreateValue(FInitializationStatus Status)
        {
            return GetOrCreateValue(Status.StatusTag, Status.MinValue, Status.MaxValue);
        }

        #region Callback
        protected void OnAddStatus(Status status)
        {
            OnAddedStatus?.Invoke(this, status);
        }
        #endregion
    }
}