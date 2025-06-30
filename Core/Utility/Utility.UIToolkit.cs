using UnityEngine.UIElements;

namespace StudioScor.Utilities
{
    public static partial class SUtility
    {
        public static int GetAllChildrenCount(this VisualElement element)
        {
            int count = 0;

            foreach (var child in element.Children())
            {
                count++;
                count += GetAllChildrenCount(child);
            }

            return count;
        }
    }
}