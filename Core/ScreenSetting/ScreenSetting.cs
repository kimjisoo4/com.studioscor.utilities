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
        [SerializeField] private int _fps = 60;
        [SerializeField] private Vector2 _resolution = new Vector2(1920, 1080);
        [SerializeField] private FullScreenMode _screenMode = FullScreenMode.ExclusiveFullScreen;

        private int currentFPS;
        private Vector2 currentResolution;
        private FullScreenMode currentMode;

        public event ChangeFPSHandler OnChangedFPS;
        public event ChangeResolutionHandler OnChangedResolution;
        public event ChangeFullScreenHandler OnChangedFullScreen;

        protected override void OnReset()
        {
            currentFPS = 0;
            currentResolution = default;
            currentMode = default;
        }
        public void OnScreenSetting()
        {
            SetFPS(_fps);
            SetUseFullScreen(_screenMode);
            SetResolution(_resolution);
        }

        public void Setup(int fps, Vector2 resolution, FullScreenMode mode)
        {
            SetFPS(fps);
            SetUseFullScreen(mode);
            SetResolution(resolution);
        }

        public void SetFPS(int newFPS)
        {
            var prevFPS = currentFPS;
            currentFPS = newFPS;

            Application.targetFrameRate = currentFPS;

            Invoke_OnChangedFPS(prevFPS);
        }

        public void SetUseFullScreen(FullScreenMode newMode)
        {
            var prevMode = currentMode;
            currentMode = newMode;

            Screen.fullScreenMode = currentMode;

            Invoke_OnChangedUseFullScreen(prevMode);
        }

        public void SetResolution(Vector2 newResolution) 
        {
            var prevResolution = currentResolution;
            currentResolution = newResolution;

            Screen.SetResolution((int)currentResolution.x, (int)currentResolution.y, currentMode);

            Invoke_OnChangedResolution(prevResolution);
        }

        #region Callback
        private void Invoke_OnChangedUseFullScreen(FullScreenMode prevMode)
        {
            Log($"On Changed Use FullScreen - CurrentMode : {currentMode} PrevMode : {prevMode}");

            OnChangedFullScreen?.Invoke(this, currentMode, prevMode);
        }

        private void Invoke_OnChangedFPS(int prevFPS)
        {
            Log($"On Changed FSP - CurrentFPS : {currentFPS} PrevFPS : {prevFPS}");

            OnChangedFPS?.Invoke(this, currentFPS, prevFPS);
        }
        private void Invoke_OnChangedResolution(Vector2 prevResolution)
        {
            Log($"On Changed Resolution - CurrentResolution : X {currentResolution.x } /  Y {currentResolution.y} PrevResolution : X {currentResolution.x} /  Y {currentResolution.y}");

            OnChangedResolution?.Invoke(this, currentResolution, prevResolution);
        }
        #endregion
    }

}