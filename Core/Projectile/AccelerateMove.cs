using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class AccelerateMove
    {
        [field: Header(" [ Accelerate Move ]")]
        [field: SerializeField] public float StartSpeed { get; set; } = 10f;
        [field: SerializeField] public float TargetSpeed { get; set; } = 5f;
        [field: SerializeField] public float AccelerateSpeed { get; set; } = 20f;
        [field: SerializeField] public float DecelerateSpeed { get; set; } = 20f;
        [field: SerializeField]public bool IsStopped { get; set; } = false;

        [SerializeField][SReadOnly] private bool isPlaying = false;
        [SerializeField][SReadOnly] private float speed = 0f;

        public bool IsPlaying => isPlaying;
        public float Speed => speed;

        public void OnAccelerate()
        {
            if (isPlaying)
                return;

            speed = StartSpeed;

            isPlaying = true;
            IsStopped = false;
        }
        public void OnAccelerate(float startSpeed)
        {
            if (isPlaying)
                return;

            speed = startSpeed;

            isPlaying = true;
            IsStopped = false;
        }

        public void UpdateAccelerate(float deltaTime)
        {
            if (!isPlaying)
                return;

            if(IsStopped)
            {
                speed = Mathf.MoveTowards(speed, 0f, DecelerateSpeed * deltaTime);
            }
            else
            {
                if (speed < TargetSpeed)
                {
                    speed = Mathf.MoveTowards(speed, TargetSpeed, AccelerateSpeed * deltaTime);
                }
                else
                {
                    speed = Mathf.MoveTowards(speed, TargetSpeed, DecelerateSpeed * deltaTime);
                }
            }
        }

        public void EndAccelerate()
        {
            if (!isPlaying)
                return;

            isPlaying = false;
            IsStopped = false;

            speed = 0f;
        }
    }
}