using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class LookAtDirection
    {
        [field: Header(" Look At Direction")]
        [field: SerializeField] public Transform Owner;
        [field: SerializeField] public Transform Target { get; set; } = null;
        [field: SerializeField] public Vector3 Direction { get; set; } = Vector3.forward;
        [field: SerializeField] public float TurnSpeed { get; set; } = 180f;

        [SerializeField][SReadOnly] private bool isPlaying = false;
        [SerializeField] private Quaternion turnRotation;

        public bool IsPlaying => isPlaying;
        public Quaternion Rotation => turnRotation;

        public void OnLookAtDirection(Transform newTarget)
        {
            if (IsPlaying)
                return;

            Target = newTarget;

            OnLookAtDirection();
        }
        public void OnLookAtDirection(Vector3 newDirection)
        {
            if (IsPlaying)
                return;

            Direction = newDirection;
            Target = null;

            OnLookAtDirection();
        }
        public void OnLookAtDirection()
        {
            if (IsPlaying)
                return;

            isPlaying = true;

        }
        public void EndLookAtDirection()
        {
            if (!IsPlaying)
                return;

            isPlaying = false;
        }
        public void UpdateRotation(float deltaTime)
        {
            if (!isPlaying)
                return;

            if(Target)
                Direction = Owner.Direction(Target);

            if (Direction.SafeEquals(Vector3.zero))
            {
                turnRotation = Owner.rotation;
                return;
            }

            Quaternion rotation = Owner.rotation;
            Quaternion newRotation = Quaternion.LookRotation(Direction);

            Quaternion angle = Quaternion.RotateTowards(rotation, newRotation, deltaTime * TurnSpeed);

            turnRotation = angle;
        }
    }
}