using UnityEngine;

namespace StudioScor.Utilities
{
    public abstract class VariableObject<T> : ScriptableObject, ISerializationCallbackReceiver
    {
		#region
		public delegate void ValueHandler(VariableObject<T> variable, T currentValue, T prevValue);
		#endregion

		[SerializeField] protected T _InitialValue;

		[SerializeField] protected T _RuntimeValue;

		public T InitialValue => _InitialValue;

		public T Value => _RuntimeValue;


		public event ValueHandler OnChangedValue;

		public void OnAfterDeserialize()
		{
			_RuntimeValue = _InitialValue;
		}

		public void OnBeforeSerialize()
		{

		}

		public void ResetValue()
		{
			T prevValue = _RuntimeValue;

			_RuntimeValue = _InitialValue;

			OnChangeValue(prevValue);
		}
		public abstract void AddValue(T value);
		public abstract void SubtractValue(T value);
		public abstract void SetValue(T value);

		protected void OnChangeValue(T prevValue)
        {
			OnChangedValue?.Invoke(this, _RuntimeValue, prevValue);
		}
	}

}
