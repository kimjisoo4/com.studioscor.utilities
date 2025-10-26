using UnityEngine;

namespace StudioScor.Utilities
{
    public class ChangeShaderFloatValueComponent : BaseMonoBehaviour
    {
        [Header(" [ Change Shader Int Value Component ] ")]
        [SerializeField] private string _propertyID;
        [SerializeField] private Renderer[] _renderers;

        private ChangeShaderFloatValue _changeShaderFloatValue;
        public ChangeShaderFloatValue ChangeShaderIntValue => _changeShaderFloatValue;

        private void Awake()
        {
            _changeShaderFloatValue = new(_propertyID, _renderers);
        }
    }
}