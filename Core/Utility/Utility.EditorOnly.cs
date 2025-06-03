using UnityEngine;

namespace StudioScor.Utilities
{
    public static partial class SUtility
    {
        public static int GUIDToHash(this Object target)
        {
#if UNITY_EDITOR
            string guid = UnityEditor.AssetDatabase.AssetPathToGUID(UnityEditor.AssetDatabase.GetAssetPath(target));

            return FNV1aHash(guid);
#else
            return -1;
#endif
        }

        /// <summary>
        /// Fowler–Noll–Vo variant 1a.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
