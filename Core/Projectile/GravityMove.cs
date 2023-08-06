using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class GravityMove
    {
        [field: Header(" [ Gravity Move ] ")]
        [field: SerializeField] public float Gravity { get; set; } = 9.81f;
        [field: SerializeField] public Vector3 Direction { get; set; } = Vector3.down;

        [SerializeField][SReadOnly] private bool isPlaying = false;
        [SerializeField][SReadOnly] private float currentGravity = 0f;
        [SerializeField][SReadOnly] private Vector3 velocity;

        public bool IsPlaying => isPlaying;
        public float CurrentGravity => currentGravity;
        public Vector3 Velocity => velocity;


        public void OnGravity(float startedGravity = 0f)
        {
            if (isPlaying)
                return;

            isPlaying = true;

            currentGravity = startedGravity;

            velocity = Direction * currentGravity;

        }
        public void UpdateGravity(float deltaTime)
        {
            if (!isPlaying)
                return;

            currentGravity += Gravity * deltaTime;

            currentGravity = Mathf.Min(currentGravity, Gravity);

            velocity = Direction * currentGravity;
        }
        public void EndGravity()
        {
            if (!isPlaying)
                return;

            isPlaying = false;

            currentGravity = 0f;

            velocity = Vector3.zero;
        }
    }
}