using UnityEngine;

namespace StudioScor.Utilities
{
    public class AnimationPreviewerBehaviour : StateMachineBehaviour
    {
        [Header(" [ Animation Previewer Behaviour ]")]
        [SerializeField][Range(0f, 1f)] private float _previewTime;

        public float PreviewTime => _previewTime;
    }
}
