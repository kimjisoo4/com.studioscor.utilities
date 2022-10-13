using UnityEngine;

namespace KimScor.Utilities
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		[SerializeField] private bool _UseDontDestroy = false;

		private static T _Instance = default;
		public static T Instance
		{
			get
			{
				if (!_Instance)
				{
					_Instance = FindObjectOfType<T>();
				}

				return _Instance;
			}
		}

        private void Awake()
        {
			if (_UseDontDestroy)
				DontDestroyOnLoad(gameObject);
        }
    }

}
