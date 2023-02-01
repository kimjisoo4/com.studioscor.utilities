using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StudioScor.Utilities
{
    public class GameObjectList<T> : BaseScriptableObject, ISerializationCallbackReceiver where T : MonoBehaviour
    {
        #region Events
        public delegate void GameObjectsEventHandler(GameObjectList<T> gameObjectList, List<T> gameObjects);
        public delegate void AddGameObjectHandler(GameObjectList<T> gameObjectList, T addGameObject);
        public delegate void RemoveGameObjectHandler(GameObjectList<T> gameObjectList, T removeGameObject);
        #endregion

        private List<T> _GameObjects;

        public IReadOnlyList<T> GameObjects;

        public event GameObjectsEventHandler OnClearedGameObjects;
        public event AddGameObjectHandler OnAddedGameObject;
        public event RemoveGameObjectHandler OnRemovedGameObject;

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
            _GameObjects = new();

            OnClearedGameObjects = null;
            OnAddedGameObject = null;
            OnRemovedGameObject = null;
        }

        public void Clear()
        {
            var gameObjects = GameObjects.ToList();

            _GameObjects = new();

            OnClearGameObjects(gameObjects);
        }
        public void Add(T addObject)
        {
            if (_GameObjects is null)
                _GameObjects = new();

            _GameObjects.Add(addObject);

            OnAddGameObject(addObject);
        }

        public void Remove(T removeObject)
        {
            if (_GameObjects is null)
                _GameObjects = new();


            _GameObjects.Remove(removeObject);

            OnRemoveGameObject(removeObject);
        }

        #region CallBack
        protected void OnClearGameObjects(List<T> clearObjects)
        {
            Log("On Clear GameObjects");

            OnClearedGameObjects?.Invoke(this, clearObjects);
        }
        protected void OnAddGameObject(T addObject)
        {
            Log("On Added GameObject - " + addObject);

            OnAddedGameObject?.Invoke(this, addObject);
        }
        protected void OnRemoveGameObject(T removeObject)
        {
            Log("On Removed GameObject - " + removeObject);

            OnRemovedGameObject?.Invoke(this, removeObject);
        }
        #endregion
    }

}
