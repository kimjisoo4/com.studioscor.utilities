using UnityEngine;

namespace StudioScor.Utilities
{

    public class ApplyRootMotionWithCharacterController : ApplyRootMotionComponent
    {
        [Header(" [ Apply Root Motion With Character Controller ] ")]
        [SerializeField] private CharacterController characterController;


        protected override void Reset()
        {
            base.Reset();

#if UNITY_EDITOR
            characterController = GetComponentInParent<CharacterController>();
#endif
        }

        protected override void Awake()
        {
            base.Awake();
         
            if(!characterController)
                characterController = GetComponentInParent<CharacterController>();
        }

        protected override void UpdateRootMotion()
        {
            characterController.Move(DeltaPosition);

            characterController.transform.rotation *= DeltaRotation;
        }
    }
}