using UnityEngine;

using System.Diagnostics;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StudioScor.Utilities
{
    public abstract class BaseScriptableObject : ScriptableObject
    {
#if UNITY_EDITOR
        [Header(" [ Use Debug ] ")]
        public bool UseDebug;
#else
        [HideInInspector]public bool UseDebug = false;
#endif

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, string color = "gray")
        {
#if UNITY_EDITOR
            if (UseDebug)
                SUtility.Debug.Log(GetType().Name + " [ " + name + " ] : " + log, this, color);
#endif
        }
        [Conditional("UNITY_EDITOR")]
        protected virtual void LogError(object log, string color = "red")
        {
#if UNITY_EDITOR
            SUtility.Debug.LogError(GetType().Name + " [ " + name + " ] : " + log, this, color);
#endif
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            OnReset();

            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }
        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
        }
        private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnReset();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    OnReset();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
                default:
                    break;
            }
        }
#endif

        protected virtual void OnReset() { }
    }
}