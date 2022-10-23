using UnityEngine;
using System.Diagnostics;

namespace KimScor.Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		[SerializeField] private bool _UseDontDestroy = false;
		[SerializeField] protected bool _UseDebug = false;

		private static T _Instance = default;
		public static T Instance
		{
			get
			{
				if (!_Instance)
				{
					_Instance = FindObjectOfType<T>();

					_Instance.Initialization();
				}

				return _Instance;
			}
		}

        [Conditional("UNITY_EDITOR")]
		protected void Log(object massage)
        {
#if UNITY_EDITOR
			if (_UseDebug)
                UnityEngine.Debug.Log("Singleton [ " + gameObject.name + " ] : " + massage, this);
#endif
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
