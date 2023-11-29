using System.Collections.Generic;
using UnityEngine;


namespace StudioScor.Utilities
{
    public class ChangeShaderFloatValue : ChangeShader<float>
    {
        private readonly List<Material> materials = new();

        public ChangeShaderFloatValue(string propertyName) : base(propertyName)
        {
        }

        public ChangeShaderFloatValue(string propertyName, IEnumerable<Renderer> renderers) : base(propertyName, renderers)
        {
        }

        public override void SetValue(float value)
        {
            foreach (var renderer in _renderers)
            {
                renderer.GetMaterials(materials);

                foreach (var material in materials)
                {
                    material.SetFloat(_propertyID, value);
                }
            }
        }
    }
}
