using UnityEngine;

namespace StudioScor.StatusSystem
{
    public enum EConsumeType
    {
        [Tooltip("절대값으로 스테이터스를 차감함")] Absolute,
        [Tooltip("최대 값을 비율로 차감함")] RatioInMax,
        [Tooltip("현재 값을 비율로 차감함")] RatioInCurret,
    }
}

