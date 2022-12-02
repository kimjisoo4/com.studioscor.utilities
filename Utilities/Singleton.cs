using UnityEngine;
using System.Diagnostics;

namespace StudioScor.Utilities
{
    public class Singleton<T> : BaseMonoBehaviour where T : Singleton<T>
	{
        [Header(" [ Singleton ] ")]
		[SerializeField] private bool _UseDontDestroy = false;

		private static T _Instance = default;
		public static T Instance
		{
			get
			{
				if (!_Instance)
				{
					_Instance = FindObjectOfType<T>();

					if(_Instance)
						_Instance.Initialization();
				}

				return _Instance;
			}
		}

        private void Awake()
        {
            if (!_Instance)
            {
				Initialization();
			}
            else if(_Instance != this)
            {
				Log("Initialization - Destory");

				Destroy(gameObject);
            }
        }

		private void Initialization()
        {
			Log("Initialization");

			if (_UseDontDestroy)
            {
				Log("Don't Destroy On Load");

				DontDestroyOnLoad(gameObject);
			}
				
			_Instance = GetComponent<T>();
			_Instance.Setup();
		}
		protected virtual void Setup() 
		{
			Log("Setup");
		}

    }

}
