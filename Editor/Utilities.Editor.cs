using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace StudioScor.Utilities.Editor
{
    public static class Lables
    {

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

        #region SetLabel
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
        #endregion

        [MenuItem("Utilities/Set Auto Animation Naming")]
        private static void OnAutoAnimationNaming()
        {
            var selects = Selection.objects;

            foreach (var select in selects)
            {
                var path = AssetDatabase.GetAssetPath(select);

                var model = AssetImporter.GetAtPath(path) as ModelImporter;

                if (!model)
                    continue;

                if (!model.importAnimation)
                    continue;

                ModelImporterClipAnimation[] clipAnimations = model.defaultClipAnimations;

                if (clipAnimations.Length == 1)
                {
                    clipAnimations[0].name = select.name;
                }
                else
                {
                    for (int i = 0; i < clipAnimations.Length; i++)
                    {
                        clipAnimations[i].name = select.name + "_" + i;
                    }
                }

                model.clipAnimations = clipAnimations;
                model.SaveAndReimport();
            }
        }

        [MenuItem("Utilities/Set Auto Rename MeshtintStudio Asset")]
        private static void OnAutoRenameMeshtintStudioAsset()
        {
            var selects = Selection.objects;

            foreach (var select in selects)
            {
                var path = AssetDatabase.GetAssetPath(select);

                var model = AssetImporter.GetAtPath(path) as ModelImporter;

                if (!model)
                    continue;

                string name = select.name;

                if (!model.importAnimation)
                {
                    name = "SM_" + name.Replace(" ", "");
                }
                else
                {
                    var nameBlocks = name.Split("@");

                    var frontName = nameBlocks[0].Replace(" ", "");
                    var backName = nameBlocks[1].Replace(" ", "_");

                    name = "AN_" + frontName + "_" + backName;
                }

                AssetDatabase.RenameAsset(path, name + ".FBX");
            }
        }
    }
}