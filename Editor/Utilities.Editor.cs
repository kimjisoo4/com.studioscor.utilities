using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace StudioScor.Utilities.Editor
{
    public static class Lables
    {
        [MenuItem("Tools/StudioScor/Transform/Round Position to Int")]
        public static void RoundPositionToInt()
        {
            RoundPosition(1f);
        }
        [MenuItem("Tools/StudioScor/Transform/Round Rotation to Int")]
        public static void RoundRotationToInt()
        {
            RoundRotation(1f);
        }

        private static void RoundPosition(float snapValue)
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Undo.RecordObject(obj.transform, "Round Position");

                Vector3 pos = obj.transform.position;

                pos.x = Mathf.Round(pos.x / snapValue) * snapValue;
                pos.y = Mathf.Round(pos.y / snapValue) * snapValue;
                pos.z = Mathf.Round(pos.z / snapValue) * snapValue;

                obj.transform.position = pos;

                EditorUtility.SetDirty(obj.transform);
            }
        }
        private static void RoundRotation(float snapValue)
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Undo.RecordObject(obj.transform, "Round Rotation");

                Vector3 angles = obj.transform.eulerAngles;

                angles.x = Mathf.Round(angles.x / snapValue) * snapValue;
                angles.y = Mathf.Round(angles.y / snapValue) * snapValue;
                angles.z = Mathf.Round(angles.z / snapValue) * snapValue;

                obj.transform.eulerAngles = angles;

                EditorUtility.SetDirty(obj.transform);
            }
        }


        [MenuItem("Tools/StudioScor/Label/Reset Labels")]
        private static void ResetLables()
        {
            var selectObjects = Selection.objects;

            foreach (var select in selectObjects)
            {
                AssetDatabase.ClearLabels(select);
            }
        }

        [MenuItem("Tools/StudioScor/Label/Set Labels")]
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

        [MenuItem("Tools/StudioScor/Animation/Set Auto Animation Clip Naming")]
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

        [MenuItem("Tools/StudioScor/Animation/Set Auto Rename MeshtintStudio Asset")]
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