using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace StudioScor.Utilities.Editor
{
    [CustomEditor(typeof(AnimationPreviewerBehaviour))]
    public class AnimationPreviewerBehaviourEditor : UnityEditor.Editor
    {
        AnimationClip _previewClip;
        float _previewTime;
        bool _isPreviewing;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AnimationPreviewerBehaviour stateBehaviour = (AnimationPreviewerBehaviour)target;

            if (Validate(stateBehaviour, out string errorMessage))
            {
                GUILayout.Space(10);

                if (_isPreviewing)
                {
                    if (GUILayout.Button("Stop Preview"))
                    {
                        EnforceTPose();
                        _isPreviewing = false;
                        AnimationMode.StopAnimationMode();
                    }
                    else
                    {
                        PreviewAnimationClip(stateBehaviour);
                    }
                }
                else if (GUILayout.Button("Preview"))
                {
                    _isPreviewing = true;
                    AnimationMode.StartAnimationMode();
                }

                GUILayout.Label($"Previewing at {_previewTime:F2}s", EditorStyles.helpBox);
            }
            else
            {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Info);
            }
        }

        void PreviewAnimationClip(AnimationPreviewerBehaviour stateBehaviour)
        {
            if (_previewClip == null) return;

            _previewTime = stateBehaviour.PreviewTime * _previewClip.length;

            AnimationMode.SampleAnimationClip(Selection.activeGameObject, _previewClip, _previewTime);
        }

        bool Validate(AnimationPreviewerBehaviour stateBehaviour, out string errorMessage)
        {
            AnimatorController animatorController = GetValidAnimatorController(out errorMessage);
            if (animatorController == null) return false;

            ChildAnimatorState matchingState = animatorController.layers
                .SelectMany(layer => layer.stateMachine.states)
                .FirstOrDefault(state => state.state.behaviours.Contains(stateBehaviour));

            if (matchingState.state == null)
            {
                var stateMachines = animatorController.layers.SelectMany(layer => layer.stateMachine.stateMachines);
                matchingState = stateMachines.SelectMany(stateMachine => stateMachine.stateMachine.states).FirstOrDefault(state => state.state.behaviours.Contains(stateBehaviour));
            }

            _previewClip = matchingState.state?.motion as AnimationClip;
            if (_previewClip == null)
            {
                errorMessage = "No valid AnimationClip found for the current state.";
                return false;
            }

            return true;
        }

        AnimatorController GetValidAnimatorController(out string errorMessage)
        {
            errorMessage = string.Empty;

            GameObject targetGameObject = Selection.activeGameObject;
            if (targetGameObject == null)
            {
                errorMessage = "Please select a GameObject with an Animator to preview.";
                return null;
            }

            Animator animator = targetGameObject.GetComponent<Animator>();
            if (animator == null)
            {
                errorMessage = "The selected GameObject does not have an Animator component.";
                return null;
            }

            AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
            if (animatorController == null)
            {
                errorMessage = "The selected Animator does not have a valid AnimatorController.";
                return null;
            }

            return animatorController;
        }

        [MenuItem("GameObject/Enforce T-Pose", false, 0)]
        static void EnforceTPose()
        {
            GameObject selected = Selection.activeGameObject;
            if (!selected || !selected.TryGetComponent(out Animator animator) || !animator.avatar) return;

            SkeletonBone[] skeletonBones = animator.avatar.humanDescription.skeleton;

            foreach (HumanBodyBones hbb in Enum.GetValues(typeof(HumanBodyBones)))
            {
                if (hbb == HumanBodyBones.LastBone) continue;

                Transform boneTransform = animator.GetBoneTransform(hbb);
                if (!boneTransform) continue;

                SkeletonBone skeletonBone = skeletonBones.FirstOrDefault(sb => sb.name == boneTransform.name);
                if (skeletonBone.name == null) continue;

                if (hbb == HumanBodyBones.Hips) boneTransform.localPosition = skeletonBone.position;
                boneTransform.localRotation = skeletonBone.rotation;
            }
        }
    }
}