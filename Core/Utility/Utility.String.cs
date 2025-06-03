using System.Text;
using UnityEngine;

namespace StudioScor.Utilities
{
    public static partial class SUtility
    {
        public const string DEFINE_UNITY_EDITOR = "UNITY_EDITOR";

        public const string STRING_COLOR_DEFAULT = "#CCCCCCFF";
        public const string STRING_COLOR_FAIL = "#e02929ff";
        public const string STRING_COLOR_SUCCESS = "#29e029ff";
        public const string STRING_COLOR_ERROR = "#e02929ff";

        public const string STRING_COLOR_ADD = "grenn";
        public const string STRING_COLOR_REMOVE = "red";
        public const string STRING_COLOR_RED = "red";
        public const string STRING_COLOR_YELLOW = "yellow";
        public const string STRING_COLOR_BLUE = "blue";
        public const string STRING_COLOR_GREEN = "green";
        public const string STRING_COLOR_GREY = "grey";
        public const string STRING_COLOR_WHITE = "white";

        public static readonly StringBuilder StringBuilder = new();

        public static StringBuilder GetStringBuilder()
        {
            StringBuilder.Clear();

            return StringBuilder;
        }

        public static string ToBold(this string message)
        {
            var sb = GetStringBuilder();
            sb.Append("<b>").Append(message).Append("</b>");

            return sb.ToString();
        }
        public static string ToColor(this string message, string color)
        {
            var sb = GetStringBuilder();
            sb.Append("<color=").Append(color).Append(">").Append(message).Append("</color>");

            return sb.ToString();
        }
        public static string ToColor(this string message, Color color)
        {
            var sb = GetStringBuilder();
            sb.Append("<color=#").Append(ColorUtility.ToHtmlStringRGBA(color)).Append(">").Append(message).Append("</color>");

            return sb.ToString();
        }
    }
    

}
