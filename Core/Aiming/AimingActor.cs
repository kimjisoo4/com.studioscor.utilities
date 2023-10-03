using UnityEngine;


namespace StudioScor.Utilities
{
    [AddComponentMenu("StudioScor/Utilities/Aiming/Aiming Actor", order: 0)]
    public class AimingActor : BaseMonoBehaviour
    {
        [Header(" [ Aim Component ] ")]
        [SerializeField] private GameObject aimingSystemActor;

        private Camera mainCamera;
        private IAimingSystem aimingSystem;

        private void Awake()
        {
            mainCamera = Camera.main;
            aimingSystem = aimingSystemActor.GetComponent<IAimingSystem>();
        }
        private void LateUpdate()
        {
            transform.position = aimingSystem.AimPosition;
            transform.rotation = mainCamera.transform.rotation;
        }
    }
}