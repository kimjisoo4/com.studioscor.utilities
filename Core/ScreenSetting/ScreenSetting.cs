using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/new Screen Setting", fileName = "ScreenSetting_")]
    public class ScreenSetting : BaseScriptableObject
    {
        #region Evenet
        public delegate void ChangeFPSHandler(ScreenSetting screenSetting,int currentFps, int prevFPS);
        public delegate void ChangeResolutionHandler(ScreenSetting screenSetting, Vector2 currentResolution, Vector2 prevResolution);
        public delegate void ChangeFullScreenHandler(ScreenSetting screenSetting, FullScreenMode currentMode, FullScreenMode prevMode);
        #endregion

        [Header(" [ Screen Setting ] ")]
        [SerializeField] private int _FPS = 60;
        [SerializeField] private Vector2 _Resolution = new Vector2(1920, 1080);
        [SerializeField] private FullScreenMode _Mode = FullScreenMode.ExclusiveFullScreen;

        private int _CurrentFPS;
        private Vector2 _CurrentResolution;
        private FullScreenMode _CurrentMode;

        public event ChangeFPSHandler OnChangedFPS;
        public event ChangeResolutionHandler OnChangedResolution;
        public event ChangeFullScreenHandler OnChangedFullScreen;

        protected override void OnReset()
        {
            _CurrentFPS = 0;
            _Mode = default;
            _Resolution = default;
        }
        public void OnScreenSetting()
        {
            SetFPS(_FPS);
            SetUseFullScreen(_Mode);
            SetResolution(_Resolution);
        }

        public void Setup(int fps, Vector2 resolution, FullScreenMode mode)
        {
            SetFPS(fps);
            SetUseFullScreen(mode);
            SetResolution(resolution);
        }

        public void SetFPS(int newFPS)
        {
            var prevFPS = _CurrentFPS;
            _CurrentFPS = newFPS;

            Application.targetFrameRate = _CurrentFPS;

            Callback_OnChangedFPS(prevFPS);
        }

        public void SetUseFullScreen(FullScreenMode newMode)
        {
            var prevMode = _CurrentMode;
            _CurrentMode = newMode;

            Screen.fullScreenMode = _CurrentMode;

            Callback_OnChangedUseFullScreen(prevMode);
        }

        public void SetResolution(Vector2 newResolution) 
        {
            var prevResolution = _CurrentResolution;
            _CurrentResolution = newResolution;

            Screen.SetResolution((int)_CurrentResolution.x, (int)_CurrentResolution.y, _CurrentMode);

            Callback_OnChangedResolution(prevResolution);
        }

        #region Callback
        private void Callback_OnChangedUseFullScreen(FullScreenMode prevMode)
        {
            Log($"On Changed Use FullScreen - CurrentMode : {_CurrentMode} PrevMode : {prevMode}");

            OnChangedFullScreen?.Invoke(this, _CurrentMode, prevMode);
        }

        private void Callback_OnChangedFPS(int prevFPS)
        {
            Log($"On Changed FSP - CurrentFPS : {_CurrentFPS} PrevFPS : {prevFPS}");

            OnChangedFPS?.Invoke(this, _CurrentFPS, prevFPS);
        }
        private void Callback_OnChangedResolution(Vector2 prevResolution)
        {
            Log($"On Changed Resolution - CurrentResolution : X {_CurrentResolution.x } /  Y {_CurrentResolution.y} PrevResolution : X {_CurrentResolution.x} /  Y {_CurrentResolution.y}");

            OnChangedResolution?.Invoke(this, _CurrentResolution, prevResolution);
        }
        #endregion
    }

}