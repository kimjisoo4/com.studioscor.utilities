using UnityEngine;

#if UNITY_EDITOR
#endif

namespace StudioScor.Utilities
{
    public class ScreenSettingComponenet : BaseMonoBehaviour
    {
        [Header(" [ Screen Setting Component ]")]
        [SerializeField] private ScreenSetting _ScreenSetting;

        [SerializeField] private bool _UseOverrideSetting = false;
        [SerializeField][SCondition(nameof(_UseOverrideSetting))] private int _FPS = 60;
        [SerializeField][SCondition(nameof(_UseOverrideSetting))] private Vector2 _Resolution = new Vector3(1920, 1080);
        [SerializeField][SCondition(nameof(_UseOverrideSetting))] private FullScreenMode _Mode = FullScreenMode.ExclusiveFullScreen;

        private void Awake()
        {
            if (_ScreenSetting)
            {
                if (_UseOverrideSetting)
                {
                    Log("Override Screen Setting.");

                    _ScreenSetting.Setup(_FPS, _Resolution, _Mode);
                }
                else
                {
                    Log("On Screen Setting.");

                    _ScreenSetting.OnScreenSetting();
                }
            }
        }
    }

}