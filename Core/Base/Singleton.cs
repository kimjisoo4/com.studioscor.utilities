using UnityEngine;
using UnityEngine.SceneManagement;

namespace StudioScor.Utilities
{
    public class Singleton<T> : BaseMonoBehaviour where T : Singleton<T>
	{
        [Header(" [ Singleton ] ")]
		[SerializeField] private bool _useDontDestroy = false;

		private static T instance = default;

		public static T Instance
		{
			get
			{
				if (!instance)
				{
#if UNITY_6000_0_OR_NEWER
					instance = FindAnyObjectByType<T>();
#else
					instance = FindObjectOfType<T>();
#endif

					if (instance)
						instance.Initialization();
					else
                    {
						GameObject gameObject = new GameObject(typeof(T).Name);

						instance = gameObject.AddComponent<T>();
                    }
				}

				return instance;
			}
		}

        private void Awake()
        {
            if (!instance)
            {
				Initialization();
			}
            else if(instance != this)
            {
				Log("Initialization - Destory", SUtility.STRING_COLOR_RED);

				Destroy(gameObject);
            }
        }

		private void Initialization()
        {
			Log("Initialization", SUtility.STRING_COLOR_GREEN);

			if (_useDontDestroy)
            {
				Log("Don't Destroy On Load", SUtility.STRING_COLOR_GREEN);

				DontDestroyOnLoad(gameObject);
			}
				
			instance = GetComponent<T>();

			instance.Setup();
		}

		protected virtual void Setup() 
		{
			Log("Setup");
		}
    }

}
