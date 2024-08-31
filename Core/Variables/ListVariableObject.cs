using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace StudioScor.Utilities
{

    public abstract class ListVariableObject<T> : EmptyVariable, ISerializationCallbackReceiver
	{
		#region Events
		public delegate void ListMemberEventHandler(ListVariableObject<T> variable, T value);
		#endregion

		[SerializeField] protected List<T> _initialValues;
		[SerializeField][SReadOnly] protected List<T> _runtimeValues;

		public IReadOnlyList<T> InitialValues => _initialValues;
		public IReadOnlyList<T> Values => _runtimeValues;


		public event ListMemberEventHandler OnAdded;
		public event ListMemberEventHandler OnRemoved;

		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
			OnReset();
		}

		protected override void OnReset()
        {
			if(_initialValues is not null)
			{
                _runtimeValues = _initialValues.ToList();
            }

			OnAdded = null;
			OnRemoved = null;
		}

		public void Clear()
        {
			if (_runtimeValues is null)
				_runtimeValues = new();

            _runtimeValues.Clear();
		}

        public virtual void Add(T member)
        {
			if (_runtimeValues is null)
				_runtimeValues = new();

            if (_runtimeValues.Contains(member))
				return;

			_runtimeValues.Add(member);

			Invoke_OnAdded(member);
		}
		public virtual void Remove(T member)
        {
            if (_runtimeValues is null)
                _runtimeValues = new();

            if (_runtimeValues.Remove(member))
            {
				Invoke_OnRemoved(member);
            }
        }
		public void RemoveAt(int index)
        {
            if (_runtimeValues is null)
                _runtimeValues = new();

            if (_runtimeValues.Count <= index)
				return;

			var member = _runtimeValues[index];

			_runtimeValues.RemoveAt(index);

			Invoke_OnRemoved(member);
        }

        protected void Invoke_OnAdded(T addMember)
        {
			Log($"{nameof(OnAdded)} - {addMember} ");

			OnAdded?.Invoke(this, addMember);
		}
		protected void Invoke_OnRemoved(T removeMember)
        {
			Log($"{nameof(OnRemoved)} - {removeMember}");

			OnRemoved?.Invoke(this, removeMember);
		}
    }

}
