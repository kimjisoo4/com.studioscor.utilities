using UnityEngine;

#if UNITY_EDITOR
#endif

namespace StudioScor.Utilities
{
    public class ScreenSettingComponenet : BaseMonoBehaviour
    {
        [Header(" [ Screen Setting Component ]")]
        [SerializeField] private ScreenSetting screenSetting;

        [SerializeField] private bool useOverrideSetting = false;
        [SerializeField][SCondition(nameof(useOverrideSetting))] private int fps = 60;
        [SerializeField][SCondition(nameof(useOverrideSetting))] private Vector2 resolution = new Vector3(1920, 1080);
        [SerializeField][SCondition(nameof(useOverrideSetting))] private FullScreenMode screenMode = FullScreenMode.ExclusiveFullScreen;

        private void Awake()
        {
            if (screenSetting)
            {
                if (useOverrideSetting)
                {
                    Log("Override Screen Setting.");

                    screenSetting.Setup(fps, resolution, screenMode);
                }
                else
                {
                    Log("On Screen Setting.");

                    screenSetting.OnScreenSetting();
                }
            }
        }
    }

}