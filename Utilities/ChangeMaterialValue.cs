using System.Collections.Generic;
using UnityEngine;


namespace StudioScor.Utilities
{
    public class ChangeMaterialValue : BaseMonoBehaviour
    {
        [field: Header(" [ Change Material Value ] ")]
        [field: SerializeField] public Renderer[] Renderers { get; private set; }
        [field: SerializeField] public string PropertyName { get; private set; }

        private List<Material> _Materials;
        private int _PropertyID;

        private void Awake()
        {
            _PropertyID = Shader.PropertyToID(PropertyName);

            _Materials = new();

            foreach (var renderer in Renderers)
            {
                _Materials.AddRange(renderer.materials);
            }
        }

        public void AddMaterials(Material[] materials)
        {
            _Materials.AddRange(materials);
        }
        public void RemoveMaterials(Material[] materials)
        {
            foreach (var removeMaterial in materials)
            {
                _Materials.Remove(removeMaterial);
            }
        }
        public void SetValue(float value)
        {
            for(int i = _Materials.Count - 1; i >= 0; i--)
            {
                if(_Materials[i])
                {
                    _Materials[i].SetFloat(_PropertyID, value);
                }
                else
                {
                    _Materials.RemoveAt(i);
                }
            }
        }
    }
}
