using UnityEngine;


namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/Aiming/Aiming Actor", order: 0)]
    public class AimingActor : BaseMonoBehaviour
    {
        [Header(" [ Aim Component ] ")]
        [SerializeField] private GameObject aimingSystemActor;
        [SerializeField][Min(0f)] private float lerpSpeed = 10f;

        private Transform cameraTransform;
        private IAimingSystem aimingSystem;

        private void Awake()
        {
            cameraTransform = Camera.main.transform;
            aimingSystem = aimingSystemActor.GetComponent<IAimingSystem>();
        }
        private void LateUpdate()
        {
            Vector3 position;
            Quaternion rotation;

            if(lerpSpeed > 0)
            {
                float deltaTime = Time.deltaTime;

                position = Vector3.Lerp(transform.position, aimingSystem.AimPosition, deltaTime * lerpSpeed);
                rotation = Quaternion.Slerp(transform.rotation, cameraTransform.rotation, deltaTime * lerpSpeed);

            }
            else
            {
                position = aimingSystem.AimPosition;
                rotation = cameraTransform.rotation;
            }

            transform.SetPositionAndRotation(position, rotation);
        }
    }
}