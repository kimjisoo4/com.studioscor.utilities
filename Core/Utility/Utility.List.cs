using System;
using System.Collections.Generic;
using System.Linq;

namespace StudioScor.Utilities
{
    public static partial class SUtility
    {
        public static int IndexOf<T>(this IReadOnlyCollection<T> array, T findItem)
        {
            for(int i =0; i < array.Count; i++)
            {
                var arrayItem = array.ElementAt(i);

                if (Equals(arrayItem, findItem))
                    return i;
            }

            return -1;
        }

        public static int LastIndex<T>(this IReadOnlyCollection<T> array)
        {
            return array.Count - 1;
        }
       public static int LastIndex<T>(this T[] array)
        {
            return array.Length - 1;
        }
        public static int LastIndex<T>(this IReadOnlyList<T> list)
        {
            return list.Count - 1;
        }
        public static int LastIndex<T>(this List<T> list)
        {
            return list.Count - 1;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection is null || collection.Count() == 0;
        }
        public static T RandomElement<T>(this IEnumerable<T> colliection)
        {
            if (colliection is null || colliection.Count() == 0)
                return default;

            return colliection.ElementAt(UnityEngine.Random.Range(0, colliection.Count()));
        }
        public static T[] RandomElements<T>(this T[] array, int count)
        {
            if (count > array.Count())
            {
                return array.Shuffle();
            }

            T[] copy = (T[])array.Clone();

            System.Random rnd = new System.Random();
            int n = copy.Length;

            for(int i = 0; i < count; i++)
            {
                int randomIndex = rnd.Next(i, n);

                T temp = copy[i];
                copy[i] = copy[randomIndex];
                copy[randomIndex] = temp;
            }

            T[] result = new T[count];
            Array.Copy(copy, result, count);

            return result;
        }

        public static T[] Shuffle<T>(this T[] array)
        {
            int random1, random2;
            T temp;

            int listCount = array.Length;

            for (int i = 0; i < listCount; ++i)
            {
                random1 = UnityEngine.Random.Range(0, listCount);
                random2 = UnityEngine.Random.Range(0, listCount);

                temp = array[random1];
                array[random1] = array[random2];
                array[random2] = temp;
            }

            return array;
        }
        public static List<T> Shuffle<T>(this List<T> list)
        {
            int random1, random2;
            T temp;

            int listCount = list.Count;

            for (int i = 0; i < listCount; ++i)
            {
                random1 = UnityEngine.Random.Range(0, listCount);
                random2 = UnityEngine.Random.Range(0, listCount);

                temp = list[random1];
                list[random1] = list[random2];
                list[random2] = temp;
            }

            return list;
        }
    }
}
