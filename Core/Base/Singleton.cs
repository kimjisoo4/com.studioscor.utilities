using UnityEngine;
using System.Diagnostics;

namespace StudioScor.Utilities
{
    public class Singleton<T> : BaseMonoBehaviour where T : Singleton<T>
	{
        [Header(" [ Singleton ] ")]
		[SerializeField] private bool useDontDestroy = false;

		private static T instance = default;
		public static T Instance
		{
			get
			{
				if (!instance)
				{
					instance = FindObjectOfType<T>();

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
				Log("Initialization - Destory", SUtility.NAME_COLOR_RED);

				Destroy(gameObject);
            }
        }

		private void Initialization()
        {
			Log("Initialization", SUtility.NAME_COLOR_GREEN);

			if (useDontDestroy)
            {
				Log("Don't Destroy On Load", SUtility.NAME_COLOR_GREEN);

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
