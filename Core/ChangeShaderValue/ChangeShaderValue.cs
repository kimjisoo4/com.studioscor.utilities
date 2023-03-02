using System.Collections.Generic;
using UnityEngine;


namespace StudioScor.Utilities
{
    public abstract class ChangeShaderValue<T> : BaseMonoBehaviour
    {
        [Header(" [ Change Material Value ] ")]
        [SerializeField] private List<Renderer> _Renderers;
        [SerializeField] private string _PropertyName;

        protected int _PropertyID;
        public IReadOnlyList<Renderer> Renderers => _Renderers;
        public string PropertyName => _PropertyName;

        private void Awake()
        {
            _PropertyID = Shader.PropertyToID(PropertyName);
        }

        public void AddRenderer(Transform transform)
        {
            if (transform.TryGetComponent(out Renderer renderers))
            {
                AddRenderer(renderers);
            }
        }
        public void RemoveRenderer(Transform transform)
        {
            if (transform.TryGetComponent(out Renderer renderers))
            {
                RemoveRenderer(renderers);
            }
        }

        public void AddRenderer(Renderer renderer)
        {
            _Renderers.Add(renderer);
        }
        public void RemoveRenderer(Renderer renderer)
        {
            _Renderers.Remove(renderer);
        }

        public abstract void SetValue(T value);
    }
}
