using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.PlayerSystem
{
    public class BlackBoard : MonoBehaviour
    {
        public delegate void ChangeBlackBoardHandler(BlackBoard blackBoard, BlackBoardKey blackboardKey, object value);

        [SerializeField] private Animator _Animator;

        [SerializeField] private Dictionary<BlackBoardKey, object> _Values;
        [SerializeField] private List<BlackBoardKey> _InitKeys;

        public event ChangeBlackBoardHandler OnChangedBlackboardValue;

        public Animator Animator => _Animator;
        public IReadOnlyDictionary<BlackBoardKey, object> Values
        {
            get
            {
                if (_Values is null)
                {
                    Setup();
                }

                return _Values;
            }
        }

        private bool _WasSetup = false;

#if UNITY_EDITOR
        private void Reset()
        {
            TryGetComponent(out _Animator);
        }
#endif

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            if (_WasSetup)
                return;

            _WasSetup = true;

            ResetBlackBoard();
        }

        public void ResetBlackBoard()
        {
            _Values = new();

            foreach (var key in _InitKeys)
            {
                _Values.Add(key, default);
            }
        }

        public bool HasBlackBoardValue(BlackBoardKey blackBoardKey)
        {
            if (!Values.TryGetValue(blackBoardKey, out object value))
                return false;

            return value != default;
        }

        public bool TryGetBlackBoardValue(BlackBoardKey blackBoardKey, out object value)
        {
            return Values.TryGetValue(blackBoardKey, out value);
        }

        public void ClearBlackBoardValue(BlackBoardKey blackBoardKey)
        {
            if (Values.ContainsKey(blackBoardKey))
            {
                _Values[blackBoardKey] = default;

                if (blackBoardKey.UseParameter)
                {
                    Animator.SetBool(blackBoardKey.Hash, false);
                }

                OnChangeBlackBoardValue(blackBoardKey);
            }
        }
        public void SetBlackBoardValue(BlackBoardKey blackBoardKey, object newValue)
        {
            if (Values.ContainsKey(blackBoardKey))
            {
                if (Values[blackBoardKey] != newValue)
                {
                    _Values[blackBoardKey] = newValue;

                    if (blackBoardKey.UseParameter)
                    {
                        Animator.SetBool(blackBoardKey.Hash, true);
                    }

                    OnChangeBlackBoardValue(blackBoardKey, newValue);
                }
            }
        }
        public BlackBoardKey GetBlackBoardKey(string name)
        {
            foreach (var key in Values.Keys)
            {
                if (key.name.Equals(name))
                {
                    return key;
                }
            }

            return null;
        }
        #region Blackboard type String
        public bool HasBlackBoardValue(string keyName)
        {
            return HasBlackBoardValue(GetBlackBoardKey(keyName));
        }
        public bool TryGetBlackBoardValue(string blackBoardKey, out object value)
        {
            return TryGetBlackBoardValue(GetBlackBoardKey(blackBoardKey), out value);
        }
        public void ClearBlackBoardValue(string keyName)
        {
            var key = GetBlackBoardKey(keyName);

            ClearBlackBoardValue(key);
        }
        public void SetBlackBoardValue(string keyName, object newValue)
        {
            var key = GetBlackBoardKey(keyName);

            SetBlackBoardValue(key, newValue);
        }
        #endregion


        #region CallBack
        protected void OnChangeBlackBoardValue(BlackBoardKey key, object newValue = default)
        {
            OnChangedBlackboardValue?.Invoke(this, key, newValue);
        }
        #endregion
    }
}
