using UnityEngine;

namespace StudioScor.Utilities
{
	public abstract class BaseScriptableVariable : BaseScriptableObject
    {
        [Header(" [ Scriptable Variable ] ")]
        [SerializeField] private string _id;
		[SerializeField, TextArea] protected string _description;

        public string ID => _id;

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (SUtility.IsPlayingOrWillChangePlaymode)
                return;

            if(string.IsNullOrEmpty(_id))
            {
                _id = SUtility.GUID(this);
            }
#endif
        }

        [ContextMenu(nameof(GuidToID))]
        [System.Diagnostics.Conditional(SUtility.DEFINE_UNITY_EDITOR)]
        private void GuidToID()
        {
#if UNITY_EDITOR
            _id = SUtility.GUID(this);
#endif
        }
    }
}
