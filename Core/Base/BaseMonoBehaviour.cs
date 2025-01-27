﻿using UnityEngine;
using System.Diagnostics;

namespace StudioScor.Utilities
{

    public abstract class BaseMonoBehaviour : MonoBehaviour
    {
        private bool _isApplicationQuit = false;
        protected bool IsApplicationQuit => _isApplicationQuit;

        #region EDITOR ONLY
#if UNITY_EDITOR
        [Header(" [ Base MonoBehaviour ] ")]
        [Header(" Use Debug ")]
        public bool UseDebug;
#else
        [HideInInspector] public bool UseDebug = false;
#endif

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected virtual void Log(object log, string color = SUtility.STRING_COLOR_GREY)
        {
#if UNITY_EDITOR
            if (UseDebug)
                SUtility.Debug.Log($"{GetType().Name} [{name}] : {log}", this, color);
#endif
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected virtual void LogError(object log, string color = SUtility.STRING_COLOR_GREY)
        {
#if UNITY_EDITOR
            SUtility.Debug.LogError($"{GetType().Name} [{name}] : {log}", this, color);
#endif
        }
        #endregion


        public virtual void Tick(float deltaTime) { }
        public virtual void FixedTick(float deltaTime) { }

        protected virtual void OnApplicationQuit()
        {
            _isApplicationQuit = true;
        }
    }
}