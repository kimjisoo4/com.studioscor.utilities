using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimScor.Utilities
{
    public static partial class Utilities
    {
        #region Vector3.Direction
        public static Vector3 Direction(this Vector3 start, Vector3 target)
        {
            return (target - start).normalized;
        }
        public static Vector3 Direction(this Transform start, Vector3 target)
        {
            return start.position.Direction(target);
        }
        public static Vector3 Direction(this Transform start, Transform target)
        {
            return start.Direction(target.position);
        }
        public static Vector3 Direction(this Vector3 start, Transform target)
        {
            return start.Direction(target.position);
        }
        #endregion
    }
}
