using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace StudioScor.MovementSystem
{
    [System.Serializable]
    public struct FSingleTargetMovement
    {
        public FSingleTargetMovement(float distance, AnimationCurve curve)
        {
            Distance = distance;
            Curve = curve;
        }

        public float Distance;
        public AnimationCurve Curve;
    }

    [System.Serializable]
    public struct FTargetMovement
    {
        [Header("[ Setting ]")]
        public bool UseMovementX;
#if ODIN_INSPECTOR
        [ShowIf("UseMovementX")]
#endif
        public float DistanceX;
#if ODIN_INSPECTOR
        [ShowIf("UseMovementX")]
#endif
        public AnimationCurve CurveX;

        [Space(5)]
        public bool UseMovementY;
#if ODIN_INSPECTOR
        [ShowIf("UseMovementY")]
#endif
        public float DistanceY;
#if ODIN_INSPECTOR
        [ShowIf("UseMovementY")]
#endif
        public AnimationCurve CurveY;

        [Space(5)]
        public bool UseMovementZ;
#if ODIN_INSPECTOR
        [ShowIf("UseMovementZ")]
#endif
        public float DistanceZ;
#if ODIN_INSPECTOR
        [ShowIf("UseMovementZ")]
#endif
        public AnimationCurve CurveZ;

        public FTargetMovement(bool useMovementX, float distanceX, AnimationCurve curveX,
                               bool useMovementY, float distanceY, AnimationCurve curveY,
                               bool useMovementZ, float distanceZ, AnimationCurve curveZ)
        {
            UseMovementX = useMovementX;
            DistanceX = distanceX;
            CurveX = curveX;
            UseMovementY = useMovementY;
            DistanceY = distanceY;
            CurveY = curveY;
            UseMovementZ = useMovementZ;
            DistanceZ = distanceZ;
            CurveZ = curveZ;
        }
    }

}