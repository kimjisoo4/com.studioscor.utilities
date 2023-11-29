using System.Collections.Generic;
using UnityEngine;


namespace StudioScor.Utilities
{
    public class ChangeShaderIntValue : ChangeShader<int>
    {
        private readonly List<Material> materials = new();

        public ChangeShaderIntValue(string propertyName) : base(propertyName)
        {
        }

        public ChangeShaderIntValue(string propertyName, IEnumerable<Renderer> renderers) : base(propertyName, renderers)
        {
        }

        public override void SetValue(int value)
        {
            foreach (var renderer in _renderers)
            {
                renderer.GetMaterials(materials);

                foreach (var material in materials)
                {
                    material.SetInt(_propertyID, value);
                }
            }
        }
    }
}
