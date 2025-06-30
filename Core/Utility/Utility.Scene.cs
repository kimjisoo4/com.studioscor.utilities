using UnityEngine.SceneManagement;

namespace StudioScor.Utilities
{
    public static partial class SUtility
    {
        public static bool IsLoaded(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == sceneName && scene.isLoaded)
                    return true;
            }
            return false;
        }
    }
}