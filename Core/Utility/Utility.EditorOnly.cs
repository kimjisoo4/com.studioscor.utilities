using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public static partial class SUtility
    {
        /// <summary>
        /// 지금 Editor 에서 게임 플레이 중일 경우 True.
        /// </summary>
        public static bool IsPlayingOrWillChangePlaymode
        {
            get
            {
#if UNITY_EDITOR
                return UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode;
#else
                return false;
#endif
            }
        }

        public static T FindAssetByType<T>() where T : Object
        {
#if UNITY_EDITOR
            string[] guids = UnityEditor.AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T).Name));
            foreach (string guid in guids)
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    return asset;
                }
            }

            return null;
#else
            return null;
#endif
        }
        public static List<T> FindAssetsByType<T>() where T : Object
        {
#if UNITY_EDITOR
            List<T> assets = new List<T>();
            string[] guids = UnityEditor.AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T).Name));
            foreach (string guid in guids)
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
#else
            return null;
#endif
        }
        public static int GetSceneLocalUniqueID(this Object lhs)
        {
#if UNITY_EDITOR
            var globalID = UnityEditor.GlobalObjectId.GetGlobalObjectIdSlow(lhs).ToString();

            return globalID.GetHashCode();
#else
        return -1;
#endif
        }

        public static string GUID(this Object target)
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.AssetPathToGUID(UnityEditor.AssetDatabase.GetAssetPath(target));
#else
            return "";
#endif
        }
        public static int GUIDToHash(this Object target)
        {
#if UNITY_EDITOR
            string guid = UnityEditor.AssetDatabase.AssetPathToGUID(UnityEditor.AssetDatabase.GetAssetPath(target));

            return FNV1aHash(guid);
#else
            return -1;
#endif
        }

        public static int FNV1aHash(string input)
        {
            const int offsetBasis = unchecked((int)2166136261);
            const int prime = 16777619;

            int hash = offsetBasis;

            foreach (char c in input)
            {
                hash ^= c;
                hash *= prime;
            }

            return hash;
        }
    }
}
