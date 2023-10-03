using UnityEngine;

namespace StudioScor.Utilities
{
    public class ApplyRootMotionWithCharacterController : BaseMonoBehaviour
    {
        [Header(" [ Apply Root Motion With Character Controller ] ")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;

        private void Reset()
        {
#if UNITY_EDITOR
            characterController = GetComponentInParent<CharacterController>();
            animator = GetComponent<Animator>();
#endif
        }
        private void Awake()
        {
            if(!characterController)
                characterController = GetComponentInParent<CharacterController>();

            if(!animator)
                animator = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            characterController.Move(animator.deltaPosition);
            characterController.transform.rotation *= animator.deltaRotation;
        }
    }
}