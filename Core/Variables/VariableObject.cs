using UnityEngine;

namespace StudioScor.Utilities
{
    public abstract class VariableObject<T> : EmptyVariable, ISerializationCallbackReceiver
    {
		#region Events
		public delegate void ValueHandler(VariableObject<T> variable, T currentValue, T prevValue);
		#endregion

		[SerializeField] protected T initialValue;
		[SerializeField][SReadOnly] protected T runtimeValue;

		public T InitialValue => initialValue;
		public T Value => runtimeValue;

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
			runtimeValue = initialValue;

			OnChangedValue = null;
		}

		public void Clear()
		{
			T prevValue = runtimeValue;

			runtimeValue = initialValue;

			Callback_OnChangeValue(prevValue);
		}

		public abstract void SetValue(T value);

		protected void Callback_OnChangeValue(T prevValue)
        {
			Log("On Changed Value - Current Value : " + runtimeValue + " PrevValue : " + prevValue);

			OnChangedValue?.Invoke(this, runtimeValue, prevValue);
		}
	}

}
