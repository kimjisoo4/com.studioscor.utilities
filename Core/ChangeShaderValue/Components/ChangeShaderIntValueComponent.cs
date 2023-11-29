using UnityEngine;

namespace StudioScor.Utilities
{
    public class ChangeShaderIntValueComponent : BaseMonoBehaviour
    {
        [Header(" [ Change Shader Int Value Component ] ")]
        [SerializeField][SReadOnlyWhenPlaying] private string _propertyID;
        [SerializeField][SReadOnlyWhenPlaying] private Renderer[] _renderers;

        private ChangeShaderIntValue _changeShaderIntValue;
        public ChangeShaderIntValue ChangeShaderIntValue => _changeShaderIntValue;

        private void Awake()
        {
            _changeShaderIntValue = new(_propertyID, _renderers);
        }
    }
}
