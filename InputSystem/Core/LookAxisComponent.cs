using UnityEngine;

using StudioScor.Utilities;

namespace StudioScor.InputSystem
{
    public class LookAxisComponent : BaseMonoBehaviour
    {
        [Header(" [ Look Axis Compoent ] ")]
        [SerializeField] private InputLookAxis _LookAxis;

        private void OnEnable()
        {
            _LookAxis.OnChangedLookValue += LookAxis_OnChangedLookValue;
        }
        private void OnDisable()
        {
            _LookAxis.OnChangedLookValue -= LookAxis_OnChangedLookValue;
        }

        private void LookAxis_OnChangedLookValue(InputLookAxis lookAxis, float picth, float yaw)
        {
            transform.eulerAngles = new Vector3(picth, yaw);
        }
    }
}
