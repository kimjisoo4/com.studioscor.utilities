using UnityEngine;

namespace StudioScor.Utilities
{
    public class ScreenSettingComponenet : BaseMonoBehaviour
    {
        [Header(" [ Screen Setting Component ]")]
        [SerializeField] private ScreenSetting _screenSetting;

        [SerializeField] private bool _useOverrideSetting = false;
        [SerializeField][SCondition(nameof(_useOverrideSetting))] private int _fps = 60;
        [SerializeField][SCondition(nameof(_useOverrideSetting))] private Vector2Int _resolution = new Vector2Int(1920, 1080);
        [SerializeField][SCondition(nameof(_useOverrideSetting))] private FullScreenMode _screenMode = FullScreenMode.ExclusiveFullScreen;

        private void Awake()
        {
            if (_screenSetting && !_useOverrideSetting)
            {
                Log("On Screen Setting.");

                _screenSetting.OnScreenSetting();
            }
            else
            {
                Log("Override Screen Setting.");

                Application.targetFrameRate = _fps;
                Screen.fullScreenMode = _screenMode;
                Screen.SetResolution(_resolution.x, _resolution.y, _screenMode);
            }
        }
    }
}