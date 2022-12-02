using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace StudioScor.Utilities.Editor
{
    public class AutoLabels : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            //foreach (var asset in importedAssets)
            //    ProcessAssetLabels(asset);
        }

        [MenuItem("Utilities/Reset Labels")]
        private static void ResetLables()
        {
            var selectObjects = Selection.objects;

            foreach (var select in selectObjects)
            {
                AssetDatabase.ClearLabels(select);
            }
        }

        [MenuItem("Utilities/Set Labels")]
        private static void SetLabels()
        {
            var selectObjects = Selection.objects;

            foreach (var select in selectObjects)
            {
                AssetDatabase.ClearLabels(select);

                List<string> lables = new();

                SetLabelToName(select, ref lables);

                SetLabelToScriptableObject(select, ref lables);

                SetLabelToPrefab(select, ref lables);

                if (LabelsChanged(AssetDatabase.GetLabels(select), lables.ToArray()))
                {
                    AssetDatabase.SetLabels(select, lables.ToArray());
                }
            }
        }

        private static bool LabelsChanged(string[] oldLabels, string[] newLabels)
        {
            if (oldLabels.Length == 0)
                return true;

            if (newLabels.Length == 0)
                return false;

            foreach (var oldLabel in oldLabels)
            {
                bool notEqual = true;

                foreach (var newLabel in newLabels)
                {
                    if (oldLabel == newLabel)
                    {
                        notEqual = false;

                        break;
                    }
                }

                if (notEqual)
                    return true;
            }

            return false;
        }
        private static void SetLabelToName(Object obj, ref List<string> labels)
        {
            char[] split = { '_', ' ' };

            var objNames = obj.name.Split(split);

            foreach (var objName in objNames)
            {
                if (!labels.Contains(objName))
                {
                    labels.Add(objName);
                }
            }
        }
        private static void SetLabelToScriptableObject(Object obj, ref List<string> labels)
        {
            if (obj as ScriptableObject)
            {
                char[] split = { '_', ' ' };

                var currentType = obj.GetType();

                for (int i = 0; i < 10; i++)
                {
                    var splitName = currentType.Name.Split(split);

                    foreach (var part in splitName)
                    {
                        if (!labels.Contains(part))
                        {
                            labels.Add(part);
                        }
                    }

                    if (currentType.BaseType is null || currentType.BaseType == typeof(Object))
                    {
                        break;
                    }

                    currentType = currentType.BaseType;
                }
            }
        }
        private static void SetLabelToPrefab(Object obj, ref List<string> lables)
        {
            if (obj as GameObject)
            {
                char[] split = { '_', ' ' };

                GameObject current = obj as GameObject;

                for (int i = 0; i < 10; i++)
                {
                    current = PrefabUtility.GetCorrespondingObjectFromSource(current);

                    if (current is null)
                    {
                        break;

                    }
                    var splitName = current.name.Split(split);

                    foreach (var part in splitName)
                    {
                        if (!lables.Contains(part))
                        {
                            lables.Add(part);
                        }
                    }
                }
            }
        }
        private static void ProcessAssetLabels(string assetPath)
        {
            var obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

            List<string> lables = new();

            SetLabelToName(obj, ref lables);

            SetLabelToScriptableObject(obj,ref lables);
            
            SetLabelToPrefab(obj, ref lables);

            if (LabelsChanged(AssetDatabase.GetLabels(obj), lables.ToArray()))
            {
                AssetDatabase.SetLabels(obj, lables.ToArray());
            }
        }
    }
}

