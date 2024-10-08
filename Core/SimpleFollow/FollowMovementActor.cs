using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IFollowMovementActor
    {
        public delegate void FollowMovementStateHandler(IFollowMovementActor followMovementActor);

        public GameObject gameObject { get; }
        public Transform transform { get; }
        public bool IsPlaying { get; }

        public float FollowSpeed { get; set; }

        public GameObject Target { get; }
        public Vector3 FollowPosition { get; }
        
        public void SetTarget(GameObject gameObject);
        public void SetPosition(Vector3 position);

        public void OnFollow();
        public void EndFollow();
        public void UpdateFollow(float deltaTime);

        public event FollowMovementStateHandler OnStartedFollow;
        public event FollowMovementStateHandler OnEndedFollow;

    }
    public class FollowMovementActor : BaseMonoBehaviour, IFollowMovementActor
    {
        [Header(" [ Follow Movement Actor ] ")]
        [SerializeField] private GameObject _target;
        [SerializeField][Min(-1f)] private float _followSpeed = 5f;

        [Header(" Following Condition ")]
        [SerializeField] private bool _useCondition = true;
        [SerializeField][SCondition(nameof(_useCondition))] private float _ignoreDistance = 1f;
        [SerializeField][SCondition(nameof(_useCondition))] private float _reachDistance = 0.01f;

        [Header(" Following Limit ")]
        [SerializeField] private bool _useLimit = true;
        [SerializeField][SCondition(nameof(_useLimit))] private float _maxDistance = 2f;

        [Header(" Auto Playing ")]
        [SerializeField] private bool _isAutoPlaying = true;

        private bool _isPlaying;
        private bool _shouldFollow;
        private Vector3 _position;

        public bool IsPlaying => _isPlaying;
        public GameObject Target => _target;

        public Vector3 FollowPosition
        {
            get
            {
                return _target ? _target.transform.position : _position;
            }
        }

        public float FollowSpeed
        {
            get
            {
                return _followSpeed;
            }
            set
            {
                _followSpeed = value;
            }
        }

        public event IFollowMovementActor.FollowMovementStateHandler OnStartedFollow;
        public event IFollowMovementActor.FollowMovementStateHandler OnEndedFollow;

        private void Awake()
        {
            if (_isAutoPlaying)
            {
                OnFollow();
            }
            else
            {
                enabled = false;
            }
        }
        private void OnEnable()
        {
            if (_isAutoPlaying)
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

        public void SetPosition(Vector3 position)
        {
            this._position = position;
        }

        public void SetTarget(GameObject gameObject)
        {
            if (_target == gameObject)
                return;

            _target = gameObject;
        }

        public void OnFollow()
        {
            if (_isPlaying)
                return;

            _isPlaying = true;
            enabled = true;

            Invoke_OnStartedFollow();
        }

        public void EndFollow()
        {
            if (!_isPlaying)
                return;

            _isPlaying = false;
            enabled = false;

            Invoke_OnEndedFollow();
        }

        public void UpdateFollow(float deltaTime)
        {
            if(_useCondition)
                CheckFollow();

            if(!_useCondition || _shouldFollow)
                FollowMovement(deltaTime);
        }

        private void CheckFollow()
        {
            Vector3 targetPosition = FollowPosition;
            float distance = transform.SqrDistance(targetPosition);

            if (!_shouldFollow)
            {
                if (distance > _ignoreDistance)
                {
                    _shouldFollow = true;
                }
            }
            else
            {
                if (distance < _reachDistance)
                {
                    _shouldFollow = false;
                }
            }
        }
        private void FollowMovement(float deltaTime)
        {
            Vector3 targetPosition = FollowPosition;

            if (!_useLimit)
            {
                if(_followSpeed > 0f)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, deltaTime * _followSpeed);
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
            else
            {
                Vector3 direction = transform.Direction(targetPosition, false);
                Vector3 velocity = direction * deltaTime * _followSpeed;
                Vector3 newPosition = transform.position + velocity;

                float distance = Vector3.Distance(newPosition, targetPosition);

                if (distance > _maxDistance)
                {
                    transform.position = targetPosition - direction.normalized * _maxDistance;
                }
                else
                {
                    transform.position = newPosition;
                }
            }
        }

        private void Invoke_OnStartedFollow()
        {
            Log($"{nameof(OnStartedFollow)}");

            OnStartedFollow?.Invoke(this);
        }
        private void Invoke_OnEndedFollow()
        {
            Log($"{nameof(OnEndedFollow)}");

            OnEndedFollow?.Invoke(this);
        }

    }
}