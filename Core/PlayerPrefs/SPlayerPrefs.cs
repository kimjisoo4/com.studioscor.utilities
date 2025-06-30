using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace StudioScor.Utilities
{
    public static class SPlayerPrefs
    {
        private readonly static Dictionary<string, object> _loaded = new();

        static SPlayerPrefs()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, mode) => Release();
        }

        public static T GetValue<T>(string name, T defaultValue = default)
        {
            if (!_loaded.TryGetValue(name, out var data))
            {
                if (HasKey(name))
                {
                    data = LoadValue<T>(name);
                }
                else
                {
                    data = defaultValue;
                }

                _loaded.Add(name, data);
            }
            return (T)data;
        }

        public static void SetValue<T>(string name, T data, bool autoSave = false)
        {
            _loaded[name] = data;

            string json;

            if (IsPrimitiveOrUnityStruct(typeof(T)))
            {
                var wrapper = new Wrapper<T>{ value = data };
                json = JsonUtility.ToJson(wrapper);
            }
            else
            {
                json = JsonUtility.ToJson(data);
            }

            PlayerPrefs.SetString(name, json);

            if(autoSave)
            {
                Save();
            }
        }

        public static void Release()
        {
            _loaded.Clear();
        }

        private static T LoadValue<T>(string name)
        {
            if (!PlayerPrefs.HasKey(name))
            {
                return default;
            }

            string json = PlayerPrefs.GetString(name);

            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            if (IsPrimitiveOrUnityStruct(typeof(T)))
            {
                var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
                return wrapper != null ? wrapper.value : default;
            }

            return JsonUtility.FromJson<T>(json);
        }

        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public static void DeleteKey(string key, bool autoSave = false)
        {
            PlayerPrefs.DeleteKey(key);

            _loaded.Remove(key);

            if(autoSave)
            {
                Save();
            }
        }
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
            _loaded.Clear();
        }

        public static void Save()
        {
            PlayerPrefs.Save();

        }

        [Serializable]
        private class Wrapper<T>
        {
            public T value;
        }

        private static bool IsPrimitiveOrUnityStruct(Type type)
        {
            return type.IsPrimitive ||
                   type == typeof(string) ||
                   type == typeof(Vector2) ||
                   type == typeof(Vector3) ||
                   type == typeof(Vector4) ||
                   type == typeof(Color) ||
                   type == typeof(Quaternion) ||
                   type == typeof(Rect) ||
                   type == typeof(Bounds);
        }
    }
}