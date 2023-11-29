using System.Collections.Generic;
using UnityEngine;


namespace StudioScor.Utilities
{
    public abstract class ChangeShader<T> : BaseClass
    {
        protected readonly int _propertyID;
        protected readonly List<Renderer> _renderers = new();

        public ChangeShader(string propertyName, IEnumerable<Renderer> renderers)
        {
            _propertyID = Shader.PropertyToID(propertyName);

            _renderers = new();
            _renderers.AddRange(renderers);
        }
        public ChangeShader(string propertyName)
        {
            _propertyID = Shader.PropertyToID(propertyName);
        }
        
        public void AddRenderer(GameObject addGameObject)
        {
            if (addGameObject.TryGetComponent(out Renderer renderers))
            {
                AddRenderer(renderers);
            }
        }
        public void RemoveRenderer(GameObject removeGameObject)
        {
            if (removeGameObject.TryGetComponent(out Renderer renderers))
            {
                RemoveRenderer(renderers);
            }
        }

        public void AddRenderer(Renderer renderer)
        {
            _renderers.Add(renderer);
        }
        public void RemoveRenderer(Renderer renderer)
        {
            _renderers.Remove(renderer);
        }

        public abstract void SetValue(T value);
    }
}
