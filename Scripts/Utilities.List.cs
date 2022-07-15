using System.Collections.Generic;
using UnityEngine;

namespace KimScor.Utilities
{
    public static partial class Utilities
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            int random1, random2;
            T temp;

            for (int i = 0; i < list.Count; ++i)
            {
                random1 = Random.Range(0, list.Count);
                random2 = Random.Range(0, list.Count);

                temp = list[random1];
                list[random1] = list[random2];
                list[random2] = temp;
            }

            return list;
        }
    }
}
