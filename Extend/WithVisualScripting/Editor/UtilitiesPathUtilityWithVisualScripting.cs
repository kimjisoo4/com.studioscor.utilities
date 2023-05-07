#if SCOR_ENABLE_VISUALSCRIPTING
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace StudioScor.Utilities.Editor.VisualScripting
{
    public static class UtilitiesPathUtilityWithVisualScripting
    {
        public static string VisualScriptingResources => UtilitiesPathUtility.RootFolder + "Editor/Extend/WithVisualScripting/Icons/";

        private readonly static Dictionary<string, EditorTexture> _EditorTextures = new Dictionary<string, EditorTexture>();

        public static EditorTexture Load(string name)
        {
            if (_EditorTextures.ContainsKey(name))
            {
                return GetStateTexture(name);
            }

            var _path = VisualScriptingResources;

            var editorTexture = EditorTexture.Single(AssetDatabase.LoadAssetAtPath<Texture2D>(_path + name + ".png"));

            _EditorTextures.Add(name, editorTexture);

            return GetStateTexture(name);
        }

        private static EditorTexture GetStateTexture(string name)
        {
            return _EditorTextures[name];
        }


    }
}
#endif