using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class ForceMove
    {
        [field: Header(" [ Force Move ] ")]
        [field: SerializeField] public float Mass { get; private set; } = 1f;
        [field: SerializeField] public float Drag { get; private set; } = 1f;
        [field: SerializeField] public float MinimumForce { get; private set; } = 0.2f;

        public Vector3 Force { get; private set; }
        public bool IsPlaying { get; private set; }

        public ForceMove(float mass, float drag, float minimumForce = 0.2f)
        {
            Setup(mass, drag, minimumForce);
        }

        public void Setup(float newMass, float newDrag, float newMinimumForce = 0.2f)
        {
            Mass = Mathf.Max(0.001f, newMass);
            Drag = newDrag;
            MinimumForce = newMinimumForce;
        }

        public void OnAddForce(Vector3 force, bool isOverride = false)
        {
            IsPlaying = true;

            if(isOverride)
            {
                Force = force / Mass;
            }
            else
            {
                Force += force / Mass;
            }
        }
        public void EndForce(bool isClear = true)
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;

            if(isClear)
            {
                Force = Vector3.zero;
            }
            
        }

        public void UpdateForce(float deltaTime)
        {
            if (!IsPlaying)
                return;

            Force = Vector3.MoveTowards(Force, Vector3.zero, Drag * deltaTime);

            if (Force.magnitude < MinimumForce)
            {
                EndForce(true);
            }
        }
    }
    [System.Serializable]
    public class AccelerateMove
    {
        [field: Header(" [ Accelerate Move ]")]
        [field: SerializeField] public float StartSpeed { get; set; } = 10f;
        [field: SerializeField] public float TargetSpeed { get; set; } = 5f;
        [field: SerializeField][field: Range(0f, 1f)] public float Strength { get; set; } = 1f;
        [field: SerializeField] public float AccelerateSpeed { get; set; } = 20f;
        [field: SerializeField] public float DecelerateSpeed { get; set; } = 20f;
        [field: SerializeField] public bool IsStopped { get; set; } = false;

        [SerializeField] private bool isPlaying = false;
        [SerializeField] private float speed = 0f;

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
                    speed = Mathf.MoveTowards(speed, TargetSpeed * Strength, AccelerateSpeed * deltaTime);
                }
                else
                {
                    speed = Mathf.MoveTowards(speed, TargetSpeed * Strength, DecelerateSpeed * deltaTime);
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