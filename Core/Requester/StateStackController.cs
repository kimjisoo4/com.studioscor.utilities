using System.Collections.Generic;

using System;

namespace StudioScor.Utilities
{
    /// <summary>
    /// 요청자 기반의 상태 우선순위 관리 스택.
    /// 가장 마지막에 추가된 상태를 우선 적용합니다.
    /// </summary>
    public class StateStackController<TEnum> where TEnum : Enum
    {
        public delegate void StateChangeEventHandler(StateStackController<TEnum> stateStackController, TEnum currentState, TEnum prevState);
        private struct StateRequest
        {
            public object requester;
            public TEnum state;

            public StateRequest(object requester, TEnum state)
            {
                this.requester = requester;
                this.state = state;
            }
        }

        private readonly List<StateRequest> _requests = new();
        private readonly TEnum _defaultState;

        public TEnum CurrentState { get; private set; }
        public event StateChangeEventHandler OnStateChanged;

        public StateStackController(TEnum defaultState)
        {
            _defaultState = defaultState;
            CurrentState = defaultState;
        }

        /// <summary>
        /// 상태 요청을 추가합니다.
        /// 동일 요청자가 이미 있는 경우 갱신됩니다.
        /// </summary>
        public void AddRequest(object requester, TEnum state)
        {
            if (requester == null)
                return;

            RemoveRequest(requester);
            _requests.Add(new StateRequest(requester, state));
            UpdateState();
        }

        /// <summary>
        /// 상태 요청을 제거합니다.
        /// </summary>
        public void RemoveRequest(object requester)
        {
            if (requester == null)
                return;

            for (int i = _requests.Count - 1; i >= 0; i--)
            {
                if (_requests[i].requester == requester)
                {
                    _requests.RemoveAt(i);
                    break;
                }
            }

            UpdateState();
        }

        private void UpdateState()
        {
            var prevState = CurrentState;

            if (_requests.Count > 0)
            {
                CurrentState = _requests[^1].state;
            }
            else
            {
                CurrentState = _defaultState;
            }

            if (!CurrentState.Equals(prevState))
            {
                RaiseOnStateChanged(prevState);
            }
        }

        /// <summary>
        /// 현재 상태를 반환합니다.
        /// </summary>
        public TEnum GetCurrentState()
        {
            return CurrentState;
        }

        /// <summary>
        /// 현재 요청 리스트를 읽기 전용으로 반환합니다.
        /// 디버깅용
        /// </summary>
        public IReadOnlyList<(object requester, TEnum state)> GetRequests()
        {
            List<(object, TEnum)> result = new();
            foreach (var req in _requests)
            {
                result.Add((req.requester, req.state));
            }
            return result;
        }

        /// <summary>
        /// 모든 요청을 초기화합니다.
        /// </summary>
        public void Clear()
        {
            _requests.Clear();

            var prevState = CurrentState;
            CurrentState = _defaultState;

            RaiseOnStateChanged(prevState);
        }

        private void RaiseOnStateChanged(TEnum prevState)
        {
            OnStateChanged?.Invoke(this, CurrentState, prevState);
        }
    }
}
