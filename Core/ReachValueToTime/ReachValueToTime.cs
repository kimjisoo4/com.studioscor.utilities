using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class ReachValueToTime
    {
        [Header(" [ Reach Value To Time] ")]
        [SerializeField] private float distance = 5f;
        [SerializeField] private AnimationCurve curve;

        private bool isPlaying = false;
        private float prevDistance = 0f;

        public float Distance => distance;
        public AnimationCurve Curve => curve;
        public bool IsPlaying => isPlaying;

        private bool needUpdate = false;
        private float tempDistance;
        private AnimationCurve tempCurve;

        public ReachValueToTime()
        {

        }
        public ReachValueToTime(float distance, AnimationCurve curve)
        {
            this.distance = distance;
            this.curve = curve;
        }

        public void Copy(ReachValueToTime reachValueToTime)
        {
            distance = reachValueToTime.distance;
            curve = reachValueToTime.curve;
            prevDistance = reachValueToTime.prevDistance;
        }
        public void SetDistance(float distance)
        {
            if (IsPlaying)
            {
                tempDistance = distance;

                needUpdate = true;
            }
            else
            {
                this.distance = distance;
            }
        }
        public void SetCurve(AnimationCurve curve)
        {
            if (IsPlaying)
            {
                tempCurve = curve;

                needUpdate = true;
            }
            else
            {
                this.curve = curve;
            }
        }


        public void OnMovement(float distance, AnimationCurve curve)
        {
            if (isPlaying || distance.SafeEquals(0f))
                return;

            this.distance = distance;
            this.curve = curve;
            prevDistance = 0f;

            isPlaying = true;
        }
        public void OnMovement(float distance)
        {
            if (isPlaying || distance.SafeEquals(0f))
                return;

            this.distance = distance; 
            prevDistance = 0f;

            isPlaying = true;
        }

        public void OnMovement()
        {
            if (isPlaying || distance.SafeEquals(0f))
                return;

            prevDistance = 0f;

            isPlaying = true;
        }

        public void EndMovement()
        {
            if (!IsPlaying)
                return;

            isPlaying = false;

            if(needUpdate)
            {
                distance = tempDistance;
                curve = tempCurve;
            }
        }

        public float UpdateMovement(float normalizeTime)
        {
            if (!isPlaying)
                return 0f;

            float distance = Curve.Evaluate(normalizeTime) * Distance;

            float currentDistance = distance - prevDistance;

            prevDistance = distance;

            if (normalizeTime >= 1f)
            {
                EndMovement();
            }

            return currentDistance;
        }
    }
}