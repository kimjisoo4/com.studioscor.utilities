using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace StudioScor.Editor
{
#if UNITY_EDITOR
    public class AutoLabels : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            //foreach (var asset in importedAssets)
            //    ProcessAssetLabels(asset);
        }

    }
#endif
}

