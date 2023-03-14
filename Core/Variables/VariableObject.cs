using UnityEngine;

namespace StudioScor.Utilities
{
    public abstract class VariableObject<T> : EmptyVariable, ISerializationCallbackReceiver
    {
		#region Events
		public delegate void ValueHandler(VariableObject<T> variable, T currentValue, T prevValue);
		#endregion

		[SerializeField] protected T _InitialValue;
		[SerializeField][SReadOnly] protected T _RuntimeValue;

		public T InitialValue => _InitialValue;
		public T Value => _RuntimeValue;

		public event ValueHandler OnChangedValue;

		public virtual void OnAfterDeserialize()
		{
			OnReset();
		}

		public void OnBeforeSerialize()
		{

		}

		protected override void OnReset()
        {
			_RuntimeValue = _InitialValue;

			OnChangedValue = null;
		}

		public void Clear()
		{
			T prevValue = _RuntimeValue;

			_RuntimeValue = _InitialValue;

			Callback_OnChangeValue(prevValue);
		}

		public abstract void SetValue(T value);

		protected void Callback_OnChangeValue(T prevValue)
        {
			Log("On Changed Value - Current Value : " + _RuntimeValue + " PrevValue : " + prevValue);
			OnChangedValue?.Invoke(this, _RuntimeValue, prevValue);
		}
	}

}
