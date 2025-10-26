using System.Collections.Generic;

using System;

namespace StudioScor.Utilities
{
    /// <summary>
    /// 키별로 Flag 형태의 Enum을 저장하고, 상태가 변경되면 이벤트를 호출하는 컨트롤러
    /// </summary>
    /// <typeparam name="TFlag">[Flags] 특성이 붙은 enum 타입</typeparam>
    public class FlagStateController<TFlag> where TFlag : Enum
    {
        public delegate void FlagStateEvnetHandler(FlagStateController<TFlag> flagStateController, TFlag currentFlag, TFlag prevFlag);
        // 개별 키에 대한 Flag 값 저장
        private readonly Dictionary<object, TFlag> _flagMap = new();

        // 전체 Flag OR 연산 결과
        private TFlag _combinedFlags;

        /// <summary>
        /// 전체 상태가 변경되었을 때 호출되는 이벤트
        /// </summary>
        public event FlagStateEvnetHandler OnFlagsChanged;

        /// <summary>
        /// 현재 모든 키로부터의 OR된 Flag 값을 반환
        /// </summary>
        public TFlag CombinedFlags => _combinedFlags;

        /// <summary>
        /// 하나 이상의 Flag가 설정되어 있는지 확인
        /// </summary>
        public bool HasAnyFlag => !EqualityComparer<TFlag>.Default.Equals(_combinedFlags, default);

        /// <summary>
        /// 특정 키에 대한 Flag를 설정합니다.
        /// 이전 값과 변경점이 있을 경우만 이벤트를 호출합니다.
        /// </summary>
        public void SetFlag(object key, TFlag flag)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            // 이전 combined 상태 저장
            var previousCombined = _combinedFlags;

            // 기존 값과 동일하면 변경 없음
            if (_flagMap.TryGetValue(key, out var existing) &&
                EqualityComparer<TFlag>.Default.Equals(existing, flag))
            {
                return;
            }

            _flagMap[key] = flag;
            UpdateCombinedFlags(previousCombined);
        }

        /// <summary>
        /// 특정 키에 대한 Flag를 제거합니다.
        /// </summary>
        public void UnsetFlag(object key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (_flagMap.ContainsKey(key))
            {
                var previousCombined = _combinedFlags;

                _flagMap.Remove(key);
                UpdateCombinedFlags(previousCombined);
            }
        }

        /// <summary>
        /// 모든 키에 대한 Flag를 제거합니다.
        /// </summary>
        public void Clear()
        {
            var previousCombined = _combinedFlags;

            _flagMap.Clear();
            UpdateCombinedFlags(previousCombined);
        }

        /// <summary>
        /// 현재 CombinedFlags에 특정 Flag가 포함되어 있는지 확인
        /// </summary>
        public bool HasFlag(TFlag flag)
        {
            var combined = Convert.ToUInt64(_combinedFlags);
            var check = Convert.ToUInt64(flag);
            return (combined & check) != 0;
        }

        /// <summary>
        /// CombinedFlags에 전달된 모든 Flag가 전부 포함되어 있는지 확인합니다.
        /// </summary>
        public bool HasAllFlags(TFlag flags)
        {
            var combined = Convert.ToUInt64(_combinedFlags);
            var check = Convert.ToUInt64(flags);

            return (combined & check) == check;
        }

        /// <summary>
        /// 내부적으로 CombinedFlags를 재계산하고, 변경된 경우 이벤트를 발생시킵니다.
        /// </summary>
        private void UpdateCombinedFlags(TFlag previousCombined)
        {
            ulong result = 0;
            foreach (var kvp in _flagMap)
            {
                result |= Convert.ToUInt64(kvp.Value);
            }

            var newCombined = (TFlag)Enum.ToObject(typeof(TFlag), result);

            if (!EqualityComparer<TFlag>.Default.Equals(previousCombined, newCombined))
            {
                _combinedFlags = newCombined;
                OnFlagsChanged?.Invoke(this, _combinedFlags, previousCombined);
            }
        }
    }
}
