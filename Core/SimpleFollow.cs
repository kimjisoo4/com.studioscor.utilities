using UnityEngine;

namespace StudioScor.Utilities
{
    public class SimpleFollow : BaseMonoBehaviour
    {
        [Header(" [ Simple Follow ] ")]
        [SerializeField] private Transform _Target;
        [SerializeField] private float _Speed = 5f;

        [Header(" [ Following Condition ] ")]
        [SerializeField] private bool _UseCondition = true;
        [SerializeField][SCondition(nameof(_UseCondition))] private float _IgnoreDistance = 1f;
        [SerializeField][SCondition(nameof(_UseCondition))] private float _ReachDistance = 0.01f;

        [Header(" [ Following Limit ] ")]
        [SerializeField] private bool _UseLimit = true;
        [SerializeField][SCondition(nameof(_UseLimit))] private float _MaxDistance = 2f;

        [Header(" [ Auto Playing ] ")]
        [SerializeField] private bool _AutoPlaying = true;

        private bool _IsPlaying;
        private bool _ShouldFollow;
        private Vector3 _Position;

        private bool _HasTarget;
        public bool IsPlaying => _IsPlaying;

        public Vector3 FollowPosition
        {
            get
            {
                return _HasTarget ? _Target.position : _Position;
            }
        }

        private void OnEnable()
        {
            if (_AutoPlaying)
                OnFollow();
        }
        private void OnDisable()
        {
            EndFollow();
        }

        private void Update()
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
            _Position = position;
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
            _Target = target;

            _HasTarget = _Target;
        }

        public void OnFollow()
        {
            if (_IsPlaying)
                return;

            _IsPlaying = true;

            _HasTarget = _Target;
        }

        public void EndFollow()
        {
            if (!_IsPlaying)
                return;

            _IsPlaying = false;
        }

        public void UpdateFollow(float deltaTime)
        {
            if(_UseCondition)
                CheckFollow();

            if(!_UseCondition || _ShouldFollow)
                FollowMove(deltaTime);
        }

        private void CheckFollow()
        {
            Vector3 targetPosition = FollowPosition;
            float distance = transform.SqrDistance(targetPosition);

            if (!_ShouldFollow)
            {
                if (distance > _IgnoreDistance)
                {
                    _ShouldFollow = true;
                }
            }
            else
            {
                if (distance < _ReachDistance)
                {
                    _ShouldFollow = false;
                }
            }
        }
        private void FollowMove(float deltaTime)
        {
            Vector3 targetPosition = FollowPosition;

            if (!_UseLimit)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, deltaTime * _Speed);
            }
            else
            {
                Vector3 direction = transform.Direction(targetPosition, false);
                Vector3 velocity = direction * deltaTime * _Speed;
                Vector3 newPosition = transform.position + velocity;

                float distance = Vector3.Distance(newPosition, targetPosition);

                if (distance > _MaxDistance)
                {
                    transform.position = targetPosition - direction.normalized * _MaxDistance;
                }
                else
                {
                    transform.position = newPosition;
                }
            }
        }
    }
}