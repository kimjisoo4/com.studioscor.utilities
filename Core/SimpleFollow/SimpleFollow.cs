using UnityEngine;

namespace StudioScor.Utilities
{
    public class SimpleFollow : BaseMonoBehaviour
    {
        [Header(" [ Simple Follow ] ")]
        [SerializeField] private Transform target;
        [SerializeField] private float speed = 5f;

        [Header(" [ Following Condition ] ")]
        [SerializeField] private bool useCondition = true;
        [SerializeField][SCondition(nameof(useCondition))] private float ignoreDistance = 1f;
        [SerializeField][SCondition(nameof(useCondition))] private float reachDistance = 0.01f;

        [Header(" [ Following Limit ] ")]
        [SerializeField] private bool useLimit = true;
        [SerializeField][SCondition(nameof(useLimit))] private float maxDistance = 2f;

        [Header(" [ Auto Playing ] ")]
        [SerializeField] private bool isAutoPlaying = true;

        private bool isPlaying;
        private bool shouldFollow;
        private Vector3 position;

        private bool hasTarget;
        public bool IsPlaying => isPlaying;

        public Vector3 FollowPosition
        {
            get
            {
                return hasTarget ? target.position : position;
            }
        }

        private void OnEnable()
        {
            if (isAutoPlaying)
                OnFollow();
        }
        private void OnDisable()
        {
            EndFollow();
        }

        private void LateUpdate()
        {
            float deltaTime = Time.deltaTime;

            UpdateFollow(deltaTime);
        }


        public void SetPosition(GameObject target)
        {
            SetPosition(target.transform.position);
        }
        public void SetPosition(Component component)
        {
            SetPosition(component.transform.position);
        }
        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }

        public void SetTarget(GameObject gameObject)
        {
            SetTarget(gameObject.transform);
        }
        public void SetTarget(Component component)
        {
            SetTarget(component.transform);
        }
        public void SetTarget(Transform target = null)
        {
            this.target = target;

            hasTarget = this.target;
        }

        public void OnFollow()
        {
            if (isPlaying)
                return;

            isPlaying = true;

            hasTarget = target;
        }

        public void EndFollow()
        {
            if (!isPlaying)
                return;

            isPlaying = false;
        }

        public void UpdateFollow(float deltaTime)
        {
            if(useCondition)
                CheckFollow();

            if(!useCondition || shouldFollow)
                FollowMove(deltaTime);
        }

        private void CheckFollow()
        {
            Vector3 targetPosition = FollowPosition;
            float distance = transform.SqrDistance(targetPosition);

            if (!shouldFollow)
            {
                if (distance > ignoreDistance)
                {
                    shouldFollow = true;
                }
            }
            else
            {
                if (distance < reachDistance)
                {
                    shouldFollow = false;
                }
            }
        }
        private void FollowMove(float deltaTime)
        {
            Vector3 targetPosition = FollowPosition;

            if (!useLimit)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, deltaTime * speed);
            }
            else
            {
                Vector3 direction = transform.Direction(targetPosition, false);
                Vector3 velocity = direction * deltaTime * speed;
                Vector3 newPosition = transform.position + velocity;

                float distance = Vector3.Distance(newPosition, targetPosition);

                if (distance > maxDistance)
                {
                    transform.position = targetPosition - direction.normalized * maxDistance;
                }
                else
                {
                    transform.position = newPosition;
                }
            }
        }
    }
}