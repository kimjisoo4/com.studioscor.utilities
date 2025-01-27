using UnityEngine;

namespace StudioScor.Utilities
{
    public abstract class VariableObject<T> : EmptyVariable, ISerializationCallbackReceiver
    {
		#region Events
		public delegate void ValueHandler(VariableObject<T> variable, T currentValue, T prevValue);
		#endregion

		public enum ESaveMode
		{
			None,
			Auto,
			Manual,
		}

		[Header(" [ Variable Object ]  ")]
		[SerializeField] protected T _initialValue;
		[SerializeField][Readonly] protected T _runtimeValue;
		[SerializeField] private ESaveMode _saveMode = ESaveMode.None;

		public T InitialValue => _initialValue;
		public T Value => _runtimeValue;

		public event ValueHandler OnChangedValue;

		public virtual void OnAfterDeserialize()
		{
			OnReset();
		}

		public void OnBeforeSerialize()
		{

		}

        protected override void OnEnable()
        {
            base.OnEnable();

			if(_saveMode == ESaveMode.Auto)
			{
				LoadData();
			}
        }
        protected override void OnDisable()
        {
            base.OnDisable();

            if (_saveMode == ESaveMode.Auto)
            {
				SaveData();
            }
        }

		protected virtual void OnDeleteData()
		{
            Debug.LogError(" Not Defined Save/Load");
        }
        protected virtual void OnLoadData()
		{
			Debug.LogError(" Not Defined Save/Load");
		}
		protected virtual void OnSaveData()
		{
            Debug.LogError(" Not Defined Save/Load");
        }

        protected override void OnReset()
        {
			_runtimeValue = _initialValue;

			OnChangedValue = null;
		}

		public void Clear()
		{
			T prevValue = _runtimeValue;

			_runtimeValue = _initialValue;

			Callback_OnChangeValue(prevValue);
		}
		[ContextMenu(nameof(DeleteData), false, 1000000)]
		public void DeleteData()
		{
			Log(nameof(DeleteData));
		}
		public void SaveData()
		{
			Log(nameof(SaveData));

			OnSaveData();
		}
		public void LoadData()
		{
            Log(nameof(LoadData));

			OnLoadData();
        }

		public abstract void SetValue(T value);

		protected void Callback_OnChangeValue(T prevValue)
        {
			Log("On Changed Value - Current Value : " + _runtimeValue + " PrevValue : " + prevValue);

			OnChangedValue?.Invoke(this, _runtimeValue, prevValue);
		}
	}

}
