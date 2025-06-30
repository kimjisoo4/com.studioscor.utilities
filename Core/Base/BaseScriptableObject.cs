using UnityEngine;


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

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, string color = SUtility.STRING_COLOR_DEFAULT)
        {
#if UNITY_EDITOR
            if (UseDebug)
                SUtility.Debug.Log(GetType().Name + " [ " + name + " ] : " + log, this, color);
#endif
        }
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected virtual void LogError(object log, string color = SUtility.STRING_COLOR_FAIL)
        {
#if UNITY_EDITOR
            SUtility.Debug.LogError(GetType().Name + " [ " + name + " ] : " + log, this, color);
#endif
        }

        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            OnReset();

            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
#endif
        }

        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
#endif
        }

#if UNITY_EDITOR
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