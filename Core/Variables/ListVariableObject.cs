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

		[SerializeField] protected List<T> _InitialValues;
		[SerializeField][SReadOnly] protected List<T> _RuntimeValues;

		public IReadOnlyList<T> InitialValues => _InitialValues;
		public IReadOnlyList<T> Values => _RuntimeValues;


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
			_RuntimeValues = _InitialValues.ToList();

			OnAdded = null;
			OnRemoved = null;
		}

		public void Clear()
        {
			_RuntimeValues = _InitialValues.ToList();
		}

        public virtual void Add(T member)
        {
			if (_RuntimeValues.Contains(member))
				return;

			_RuntimeValues.Add(member);

			Callback_OnAdded(member);
		}
		public virtual void Remove(T member)
        {
            if (_RuntimeValues.Remove(member))
            {
				Callback_OnRemoved(member);
            }
        }
		public void RemoveAt(int index)
        {
			if (_RuntimeValues.Count <= index)
				return;

			var member = _RuntimeValues[index];

			_RuntimeValues.RemoveAt(index);

			Callback_OnRemoved(member);
        }

        protected void Callback_OnAdded(T addMember)
        {
			Log($" On Added - {addMember} ");

			OnAdded?.Invoke(this, addMember);
		}
		protected void Callback_OnRemoved(T removeMember)
        {
			Log($" On Removed - {removeMember}");

			OnRemoved?.Invoke(this, removeMember);
		}
    }

}
