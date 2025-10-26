using System.Collections.Generic;


namespace StudioScor.Utilities
{
    public class Requester
    {
        public delegate void RequsterEventHandler(Requester requester, bool value);

        private readonly HashSet<object> _requesters = new();
        private bool _hasRequest;
        public IReadOnlyCollection<object> Requesters => _requesters;
        public bool HasRequest
        {
            get
            {
                return _hasRequest;
            }
            private set
            {
                if (_hasRequest == value)
                    return;

                _hasRequest = value;

                OnValueChanged?.Invoke(this, _hasRequest);
            }
        }

        public event RequsterEventHandler OnValueChanged;

        public void Clear()
        {
            if (_requesters.Count == 0)
                return;

            _requesters.Clear();

            HasRequest = false;
        }

        public void AddRequest(object addRequester)
        {
            if (_requesters.Contains(addRequester))
                return;

            _requesters.Add(addRequester);

            HasRequest = true;
        }
        public void RemoveRequest(object removeRequester)
        {
            if (_requesters.Contains(removeRequester))
            {
                _requesters.Remove(removeRequester);

                HasRequest = _requesters.Count > 0;
            }
        }
    }
}