using UnityEngine;
using StudioScor.Utilities.DataContainer;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StudioScor.Utilities
{
    public abstract class BaseScriptableData : BaseScriptableObject, IData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Base Scriptable Data", GroupID = "Data")]
        [BoxGroup("Data")]
        [ReadOnly]
#endif
        [SerializeField]private int _id = 0;
        public int ID => _id;

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (_id == 0)
                ResetID();
#endif
        }


#if ODIN_INSPECTOR
        [ButtonGroup("Data/Buttons")]
#endif
        [AddComponentMenu(nameof(ResetID), 1000000)]
        public void ResetID()
        {
#if UNITY_EDITOR
            _id = this.GUIDToHash();
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }


#if ODIN_INSPECTOR
        [ButtonGroup("Data/Buttons")]
#endif
        [AddComponentMenu(nameof(ResetID), 1000000)]
        public void Ping()
        {
#if UNITY_EDITOR
            UnityEditor.EditorGUIUtility.PingObject(this);
#endif
        }
    }

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
            OnReset();

#if UNITY_EDITOR
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