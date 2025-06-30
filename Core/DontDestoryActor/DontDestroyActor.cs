namespace StudioScor.Utilities
{
	public class DontDestroyActor : BaseMonoBehaviour
	{
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}