using UnityEngine;
using UnityEngine.SceneManagement;

namespace StudioScor.Utilities
{
    public abstract class SOVariable<T> : BaseScriptableVariable
    {
		#region Events
		public delegate void ValueChangeEventHandler(SOVariable<T> variable, T currentValue, T prevValue);
		#endregion

		public enum ESaveMode
		{
			None,
			Auto,
			Manual,
		}
		public enum EResetType
		{
			None,
			ApplicationLoaded,
			SceneLoaded,
		}

		[Header(" [ Variable Object ]  ")]
		[SerializeField] protected T _initialValue;
		[SerializeField][SReadonly] protected T _runtimeValue;
		[SerializeField] private ESaveMode _saveMode = ESaveMode.None;
		[SerializeField] private EResetType _resetType;

		public T InitialValue => _initialValue;
		public T Value
		{
			get
			{
				return _runtimeValue;
			}
			set
			{
                if (Equals(_runtimeValue, value))
                    return;

                var prevValue = _runtimeValue;
                _runtimeValue = value;

                RaiseOnValueChanged(prevValue);

                if (_saveMode == ESaveMode.Auto)
                    Save();
            }
		}
		
		public event ValueChangeEventHandler OnValueChanged;

        private void Awake()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            switch (_resetType)
            {
                case EResetType.None:
                    break;
                case EResetType.ApplicationLoaded:
					if(_saveMode == ESaveMode.None)
					{
						Value = _initialValue;
                    }
					else
					{
						Load();
					}
					break;
                case EResetType.SceneLoaded:
					SceneManager.sceneLoaded += SceneManager_sceneLoaded;
                    break;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
			if (loadSceneMode == LoadSceneMode.Additive)
				return;

			if(_saveMode == ESaveMode.None)
			{
				Value = _initialValue;
			}
			else
			{
				Load();
			}
        }

        protected override void OnReset()
        {
			_runtimeValue = _initialValue;

			OnValueChanged = null;
		}

		public void ResetValue()
		{
			T prevValue = _runtimeValue;
			_runtimeValue = _initialValue;

			RaiseOnValueChanged(prevValue);
		}

        public void Delete()
		{
			Log(nameof(Delete));

			SPlayerPrefs.DeleteKey(ID, true);
		}

		public void Save()
		{
			Log($"{nameof(Save)} : {_runtimeValue}");

            SPlayerPrefs.SetValue(ID, _runtimeValue, true);
		}

		public void Load()
		{
			Value = SPlayerPrefs.GetValue<T>(ID, _initialValue);

            Log($"{nameof(Load)} : {Value}");

        }

		protected void RaiseOnValueChanged(T prevValue)
        {
			Log($"{nameof(OnValueChanged)} - Current Value : {_runtimeValue} Prev Value : {prevValue}");

			OnValueChanged?.Invoke(this, _runtimeValue, prevValue);
		}

        public override string ToString()
        {
			var sb = SUtility.GetStringBuilder();

			sb.Append(name).Append("[").Append(ID).Append("]").Append(" : ").Append(Value);

			return sb.ToString();
        }

        public static implicit operator T(SOVariable<T> variable) => variable.Value;
    }
}
