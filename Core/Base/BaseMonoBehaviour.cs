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
        protected virtual void Log(object log, bool isError = false)
        {
#if UNITY_EDITOR
            if (isError)
            {
                SUtility.Debug.LogError(GetType().Name + " [ " + name + " ] : " + log, this);

                return;
            }

            if (UseDebug)
                SUtility.Debug.Log(GetType().Name + " [ " + name + " ] : " + log, this);
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

    public abstract class BaseClass
    {

        [field: Header(" [ Use Debug ] ")]
        public virtual bool UseDebug { get; private set; } = false;
        [HideInInspector] public virtual Object Context { get; private set; } = null;

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, bool isError = false)
        {
#if UNITY_EDITOR
            if (isError)
            {
                SUtility.Debug.LogError($"{GetType().Name} [ {(Context? Context.name : "Empty")} ] : {log}", Context);

                return;
            }
            
            if (UseDebug)
                SUtility.Debug.Log($"{GetType().Name} [ {(Context ? Context.name : "Empty")} ] : {log}", Context);
#endif
        }
    }

    public abstract class BaseMonoBehaviour : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header(" [ Use Debug ] ")]
        public bool UseDebug;
#else
        [HideInInspector]public bool UseDebug = false;
#endif

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, bool isError = false)
        {
#if UNITY_EDITOR
            if (isError)
            {
                SUtility.Debug.LogError(GetType().Name + " [ " + name + " ] : " + log, this);
                
                return;
            }

            if (UseDebug)
                SUtility.Debug.Log(GetType().Name + " [ " + name + " ] : " + log, this);
#endif
        }
    }
}