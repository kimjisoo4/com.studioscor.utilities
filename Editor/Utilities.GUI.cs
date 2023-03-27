using UnityEngine;
using UnityEditor;

namespace StudioScor.Utilities.Editor
{



    public static partial class SEditorUtility
    {
        public static class GUI
        {
            public static void DrawLine(float lineHeight = 1f, Color color = default)
            {
                if (color == default)
                {
                    color = Color.gray;
                }

                Rect rect = EditorGUILayout.GetControlRect(false, lineHeight);
                rect.height = lineHeight;
                EditorGUI.DrawRect(rect, color);
            }
        }
    }
}