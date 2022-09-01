using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KimScor.Utilities
{
    public static partial class Utilities
    {
        [Conditional("UNITY_EDITOR")]
        public static void Log(object message, Object context = null)
        {
            UnityEngine.Debug.Log(message, context);
        }
    }

}
