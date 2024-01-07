using UnityEngine;

namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/Aiming/Aiming Actor", order: 0)]
    public class AimingActor : BaseMonoBehaviour
    {
        [Header(" [ Aim Component ] ")]
        [SerializeField] private GameObject _aimingSystemActor;
        [SerializeField][Min(0f)] private float _lerpSpeed = 10f;

        private Transform _cameraTransform;
        private IAimingSystem _aimingSystem;


        private void OnValidate()
        {
#if UNITY_EDITOR
            if (gameObject.TryGetComponentInParent(out IAimingSystem aimingSystem))
            {
                _aimingSystemActor = aimingSystem.gameObject;

            }
#endif
        }
        private void Awake()
        {
            _cameraTransform = Camera.main.transform;

            if(_aimingSystemActor)
            {
                _aimingSystem = _aimingSystemActor.GetComponent<IAimingSystem>();
            }
            else
            {
                if (gameObject.TryGetComponentInParent(out _aimingSystem))
                {
                    _aimingSystemActor = _aimingSystem.gameObject;
                }
            }
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            UpdateAimingActor(deltaTime);
        }
        private void UpdateAimingActor(float deltaTime)
        {
            Vector3 position;
            Quaternion rotation;

            if (_lerpSpeed > 0)
            {
                position = Vector3.Lerp(transform.position, _aimingSystem.AimPosition, deltaTime * _lerpSpeed);
                rotation = Quaternion.Slerp(transform.rotation, _cameraTransform.rotation, deltaTime * _lerpSpeed);
            }
            else
            {
                position = _aimingSystem.AimPosition;
                rotation = _cameraTransform.rotation;
            }

            transform.SetPositionAndRotation(position, rotation);
        }
    }
}