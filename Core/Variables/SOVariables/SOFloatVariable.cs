using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/SOVariable/new Scriptable Float Variable", fileName = "SOVariable_Flaot_")]
    public class SOFloatVariable : SOVariable<float>
	{
        [ContextMenu(nameof(OnDelete), false, 1000000)]
        private void OnDelete()
        {
            Delete();
        }
    }

}
